TLRGRP.namespace('TLRGRP.dashboards.graphs');

(function() {
	var defaultDimensions = {
		width: 0,
		height: 0,
		margin: {
			left: 0,
			top: 0,
			right: 0,
			bottom: 0
		}
	};

	TLRGRP.dashboards.graphs.ChartDimensions = function(dimensions) {
		dimensions = $.extend(true, {}, defaultDimensions, dimensions);
	
		var width = dimensions.width;
		var height = dimensions.height;
		
		if(!width && !height) {
			width = $(window).width() - 40;
			height = $(window).height() - 40;
		}
		
		width = width - dimensions.margin.left - dimensions.margin.right;
		height = height - dimensions.margin.top - dimensions.margin.bottom;
		
		return {
			width: width,
			height: height,
			margin: dimensions.margin
		};
	};
})();