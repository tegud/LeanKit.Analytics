TLRGRP.namespace('TLRGRP.dashboards.graphs');

(function() {
	var defaultOptions = { 
		axisExtents: {}
	};

	TLRGRP.dashboards.graphs.ChartCanvas = function(dimensions, options) {
		var svg = d3.select(options.elementSelector).append("svg")
					.attr("width", dimensions.width + dimensions.margin.left + dimensions.margin.right)
					.attr("height", dimensions.height + dimensions.margin.top + dimensions.margin.bottom)
					.append("g")
					.attr("transform", "translate(" + dimensions.margin.left + "," + dimensions.margin.top + ")");
			
		var x = d3.time.scale().range([0, dimensions.width]);
		var y = d3.scale.linear().range([dimensions.height, 0]);
		var xAxis = d3.svg.axis().scale(x).orient("bottom");
		var yAxis = d3.svg.axis().scale(y).orient("left");
		
		options = $.extend(true, {}, defaultOptions, options);
		
		return {
			svg: svg,
			appendAxis: function(yAxisLabel) {
				svg.append("g")
				   .attr("class", "x axis")
				   .attr("transform", "translate(0," + dimensions.height + ")")
				   .call(xAxis);

				svg.append("g")
				   .attr("class", "y axis")
				   .call(yAxis)
				   .append("text")
				   .attr("transform", "rotate(-90)")
				   .attr("y", 6)
				   .attr("dy", ".71em")
				   .style("text-anchor", "end")
				   .text(yAxisLabel);
			},
			x: x,
			y: y,
			setAxisExtent: function (extents) {
			    //options.axisExtents.x
			    //options.axisExtents.y

			    var calculatedExtentX = extents.x;
			    var calculatedExtentY = extents.y;

			    if (options.axisExtents.x) {
			        if(options.axisExtents.x[0] !== null) {
			            extents.x[0] = options.axisExtents.x[0];
			        }
			        if (options.axisExtents.x.length == 2 && options.axisExtents.x[1] !== null) {
			            extents.x[1] = options.axisExtents.x[1];
			        }
			    }

			    if (options.axisExtents.y) {
			        if (options.axisExtents.y[0] !== null) {
			            extents.y[0] = options.axisExtents.y[0];
			        }
			        if (options.axisExtents.y.length == 2 && options.axisExtents.y[1] !== null) {
			            extents.y[1] = options.axisExtents.y[1];
			        }
			    }

				x.domain(extents.x);
				y.domain(extents.y);
				
				svg.select(".x.axis").call(xAxis);
				svg.select(".y.axis").call(yAxis);
			}
		};
	};
})();