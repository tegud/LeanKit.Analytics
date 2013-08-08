TLRGRP.namespace('TLRGRP.dashboards.graphs');

(function() {
	TLRGRP.dashboards.graphs.Legend = function(svg, options) {
		var ds = options.data;
		var width = options.width;
		var functionCalculateLegendPosition = function() {
			var x = 0,
				y = 0;
				
			if(options) {
				if(typeof options.position === 'string') {
					return {
						x: 20,
						y: 0,
						textAlign: 'start',
						textXPositionOffset: 4
					};
				}
			
				if(options.position) {
					x = options.position.right || x;
					y = options.position.top || y;
				}
			}
			
			return {
				x: x,
				y: y,
				textAlign: options.textAlign || 'end',
				textXPositionOffset: options.textAlign === 'start' ? 4 : -24
			};
		};
		
		var legendBasePosition = functionCalculateLegendPosition();
		
		var legend = svg.selectAll(".legend")
			.data(ds)
			.enter().append("g")
			.attr("class", "legend")
			.attr("transform", function(d, i) { return "translate(" + legendBasePosition.x + "," + legendBasePosition.y + i * 20 + ")"; });

		legend.append("rect")
			.attr("x", width - 18)
			.attr("width", 18)
			.attr("height", 18)
			.style("fill", function(d) { return d.color; });

		legend.append("text")
			.attr("x", width + legendBasePosition.textXPositionOffset)
			.attr("y", 9)
			.attr("dy", ".35em")
			.style("text-anchor", legendBasePosition.textAlign)
			.text(function(d) { return d.title; });
	}
})();