TLRGRP.namespace('TLRGRP.dashboards.graphs');

(function() {
	var defaultOptions = {
			expressions: [],
			refreshRate: 0,
			evaluator: ':1081/1.0/metric?expression=',
			server: 'http://monitor02.ad.laterooms.com'
		};
	
	TLRGRP.dashboards.graphs.DashboardChartStore = function (options) {
		var expressions,
			chart;
		
		options = $.extend({}, defaultOptions, options);
		
		expressions = options.expressions;
		chart = options.chart;
		
		function getData() {
			var deferreds = [];
			var datasetcount = expressions.length;
			var evaluator = options.evaluator;
			var server = options.server;
			var retrievedData = {};
			
			for(n = 0; n < datasetcount; n++) {
				(function(expression) {
					var deferred = deferreds[deferreds.length] = $.Deferred();

					d3.json(server + evaluator + expression.expression, function(data) {
						data.forEach(function(d) {
							d.time = new Date(d.time);
							d.value = + d.value;
						});
						
						retrievedData[expression.id] = data;
						deferred.resolve();
					});
				})(expressions[n]);
			};
			
			$.when.apply(this, deferreds).then(function() {
				chart.drawChart(retrievedData);
				
				if(options.refreshRate) {
					setTimeout(getData, options.refreshRate);
				}
			});
		}
		
		getData();
	}
})();