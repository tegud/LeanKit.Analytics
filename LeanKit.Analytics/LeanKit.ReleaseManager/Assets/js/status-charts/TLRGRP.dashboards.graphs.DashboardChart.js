TLRGRP.namespace('TLRGRP.dashboards.graphs');

(function() {
    TLRGRP.dashboards.graphs.DashboardChart = function(options) {
        var dimensions = new TLRGRP.dashboards.graphs.ChartDimensions(options.dimensions);
        var canvas = new TLRGRP.dashboards.graphs.ChartCanvas(dimensions, options);
        var svg = canvas.svg;
        var parseDate = d3.time.format("2012-10-10").parse;
        var graphTypes = {};
        var renderers = {};

        if (options.legend !== false) {
            new TLRGRP.dashboards.graphs.Legend(svg, $.extend({
                    data: options.series,
                    width: dimensions.width
                },
                options.legend));
        }

        canvas.appendAxis(options.yAxisLabel);

        options.series.forEach(function(expression) {
            var graphType = expression.graphType || 'line';

            if (!graphTypes[graphType]) {
                graphTypes[graphType] = [expression];
            } else {
                graphTypes[graphType][graphTypes[graphType].length] = expression;
            }
        });

        $.each(graphTypes, function(graphType) {
            renderers[graphType] = new TLRGRP.dashboards.graphs.renderers.get(graphType)(canvas, graphTypes[graphType]);
        });

        return {
            drawChart: function(ds) {
                canvas.setAxisExtent(getExtents(graphTypes, ds, options.lockToZero));

                $.each(renderers, function(renderer) {
                    renderers[renderer].render(ds);
                });
            }
        };
    };
	
	
	function getExtents(graphTypes, ds, lockToZero) {
		var stacked = [];
		var minMax = [];
		
		$.each(graphTypes, function(graphType){
			var graphTypesLength = graphTypes[graphType].length;
			var i = 0;
			var data = [];
			
			for(; i < graphTypesLength; i++) {
				data[data.length] = ds[graphTypes[graphType][i].id];
			}
			
			if(graphType.indexOf('stacked') > -1) {
				stacked = data;
			}
			else {
				minMax = data;
			}
		});
		
		var minMaxExtents = new ExtentForExpressions(minMax, MinMaxExtent);
	
		if(stacked.length) {
			var stackedExtents = new ExtentForExpressions(stacked, AddToMaxExtent);
			
			stackedExtents.y[0] = 0;
			
			if(minMax.length) {
				stackedExtents = MinMaxExtent(stackedExtents, minMaxExtents);
			}
		}
		else {
		    if (lockToZero) {
		        minMaxExtents.y[0] = 0;
		    }

		    stackedExtents = minMaxExtents;
		}
		
		return stackedExtents;
	}
	
	function MinMaxExtent(calculatedExtent, currentExtent) {
		if(!calculatedExtent.length || calculatedExtent[0] > currentExtent[0]) {
			calculatedExtent[0] = currentExtent[0];
		}
		
		if(calculatedExtent.length === 1 || calculatedExtent[1] < currentExtent[1]) {
			calculatedExtent[1] = currentExtent[1];
		}
		
		return calculatedExtent;
	}
	
	function AddToMaxExtent(calculatedExtent, currentExtent) {
		if(!calculatedExtent.length || calculatedExtent[0] > currentExtent[0]) {
			calculatedExtent[0] = currentExtent[0];
		}
		
		calculatedExtent[1] = (calculatedExtent[1] || 0) + currentExtent[1];
		
		return calculatedExtent;
	}
	
	function ExtentForExpressions (ds, yExtentFunction) {
		var dsLength = ds.length;
		var xExtent = [];
		var yExtent = [];
		var n;
		
		for(n = 0; n < dsLength; n++) {
			var dsXExtent = d3.extent(ds[n], function(d) { return d.time; });
			
			xExtent = MinMaxExtent(xExtent, dsXExtent);
		}
		
		for(n = 0; n < dsLength; n++) {
			var	dsYExtent = d3.extent(ds[n], function(d) { return d.value; });
			
			yExtent = yExtentFunction(yExtent, dsYExtent);
		}
		
		return {
			x: xExtent,
			y: yExtent
		};
	}
})();