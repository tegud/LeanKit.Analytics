TLRGRP.namespace('TLRGRP.dashboards.graphs');

(function() {
	var defaultOptions = {
			expressions: [],
			refreshRate: 0,
			evaluator: ':1081/1.0/metric?expression=',
			server: 'http://172.31.168.140'
		};

    TLRGRP.dashboards.graphs.DashboardChartStore = function(options) {
        var expressions;
        var chart;
        var timeout;
        var stopped;

        options = $.extend({}, defaultOptions, options);

        expressions = options.expressions;
        chart = options.chart;

        function getData() {
            var deferreds = [];
            var datasetCount = expressions.length;
            var evaluator = options.evaluator;
            var server = options.server;
            var retrievedData = {};
            var n = 0;

            for (n; n < datasetCount; n++) {
                (function(expression) {
                    var deferred = deferreds[deferreds.length] = $.Deferred();

                    d3.json(server + evaluator + expression.expression, function(data) {
                        data.forEach(function(d) {
                            d.time = new Date(d.time);
                            d.value = +(d.value || 0);
                        });

                        retrievedData[expression.id] = data;
                        deferred.resolve();
                    });
                })(expressions[n]);
            }

            $.when.apply(this, deferreds).then(function() {
                chart.drawChart(retrievedData);

                if (options.refreshRate && !stopped) {
                    timeout = setTimeout(getData, options.refreshRate);
                }
            });
        }

        getData();

        return {
            start: function () {
                stopped = false;
                getData();
            },
            stop: function () {
                stopped = true;
                clearTimeout(timeout);
                timeout = null;
            }
        };
    };
})();