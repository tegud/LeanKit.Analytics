TLRGRP.namespace('TLRGRP.dashboards.graphs');

TLRGRP.dashboards.graphs.renderers = (function() {
	var renderers = {};

	return {
		register: function(name, renderer) {
			renderers[name.toLowerCase()] = renderer;
		},
		get: function(name) {
			return renderers[name.toLowerCase()];
		}
	};
})();

(function() {
	TLRGRP.dashboards.graphs.renderers.register('line', function (canvas, series) {
		var lines = {},
			colors = {};
		
		series.forEach(function(seriesEntry) {
			colors[seriesEntry.id] = seriesEntry.color;
			
			lines[seriesEntry.id] = d3.svg.line()
				.x(function(d) { 
					return canvas.x(d.time); 
				})
				.y(function(d) { 
					return canvas.y(d.value); 
				});
		});
		
		return {
			render: function(allData) {
				for(var data in allData) {
					if(!allData.hasOwnProperty(data)) {
						continue;
					}	
					
					var elementId = 'line' + data,
						lineElement = canvas.svg.select("#" + elementId);
					
					if(lineElement[0][0]) {
						lineElement
						   .datum(allData[data])
						   .attr("d", lines[data]);
					}
					else {
						canvas.svg.append("path")
						   .datum(allData[data])
						   .attr('id', elementId)
						   .attr("class", "line")
						   .attr("style", "stroke: " + colors[data] + ";")
						   .attr("d", lines[data]);
					}
				}
			}
		};
	});
	
	TLRGRP.dashboards.graphs.renderers.register('stacked-area', function (canvas, series) {
		var stack = d3.layout.stack().values(function(d) { 
			return d.values; 
		});
		var area = d3.svg.area()
			.x(function(d) { 
				return canvas.x(d.time); 
			})
			.y0(function(d) { 
				return canvas.y(d.y0); 
			})
			.y1(function(d) { 
				return canvas.y(d.y0 + d.y); 
			});
		var getArea = function(d) {
				return area(d.values); 
			},
			getColor = function(d) {
				return colors[d.name]; 
			};
		var fields = [];
		var colors = {};
		
		series.forEach(function(seriesEntry) {
			fields[fields.length] = seriesEntry.id;
			colors[seriesEntry.id] = seriesEntry.color;
		});
		
		function getSeries(data) {
			return stack(fields.map(function(name) {
				return {
					name: name,
					values: data.data.map(function(d) {
						return {
							time: d.time,
							y: d[name]
						};
					})
				};
			}));
		}
		
		function aggregateData(ds) {
			var aggregatedData = [];
			var firstSeries = series[0].id
		
			for(var m = 0; m < ds[firstSeries].length; m++) {
				var aggregate = {
					time: ds[firstSeries][m].time
				};
				
				aggregate[firstSeries] = ds[firstSeries][m].value;
				
				for(var n = 1; n < series.length; n++) {
					aggregate[series[n].id] = ds[series[n].id][m].value;
				}
				
				aggregatedData[m] = aggregate;
			}
			
			var colorData = {};
			
			for(m = 0; m < series.length; m++) {
				colorData[series[m].id] = series[m].color;
			}
			
			return {
				data: aggregatedData,
				color: colorData
			};
		}
		
		return {
			render: function(allData) {
				var data = aggregateData(allData);
				
				if(canvas.svg.selectAll(".series")[0][0]) {
					var allSeries = getSeries(data);
					
					var series = canvas.svg.selectAll(".series")
						.data(allSeries)
						.attr("d", getArea);
				}
				else {
					var allSeries = getSeries(data);
					
					var series = canvas.svg.selectAll(".series")
						.data(allSeries)
						.enter().append("g");
					
					series.append("path")
						.attr("class", "series")
						.attr("d", getArea)
						.style("fill", getColor);
				}
			}
		};
	});
})();