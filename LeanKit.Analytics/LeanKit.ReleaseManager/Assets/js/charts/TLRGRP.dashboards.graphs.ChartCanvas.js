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
		
		if(options.axisExtents.x) {
			x.domain(options.axisExtents.x);
		}
		
		if(options.axisExtents.y) {
			y.domain(options.axisExtents.y);
		}
		
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
			setAxisExtent: function(extents) {
				if(extents.x && !options.axisExtents.x) {
					x.domain(extents.x);
				}
				
				if(extents.y && !options.axisExtents.y) {
					y.domain(extents.y);
				}
				
				svg.select(".x.axis").call(xAxis);
				svg.select(".y.axis").call(yAxis);
			}
		};
	};
})();