(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard.Graphs');
    var colors = new TLRGRP.BADGER.ColorPalette();

    function build(options) {
        var baseChartOptions = {};

        return {
            title: options.title,
            expressions: _.map(options.expressions, function (expressionKey, i) {
                if (typeof expressionKey !== 'object') {
                    expressionKey = {
                        id: expressionKey
                    };
                }
                var expression = TLRGRP.BADGER[options.source].metricInfo(expressionKey.id);

                $.extend(baseChartOptions, expression.chartOptions);

                return {
                    title: expression.title,
                    expression: expression.expression,
                    color: colors.getColorByKey(expressionKey, i)
                };
            }),
            chartOptions: $.extend(baseChartOptions, options.chartOptions)
        };
    }

    TLRGRP.BADGER.Dashboard.Graphs = (function () {
        var allGraphs = {};

        return {
            register: function () {
                _(arguments).each(function (graph) {
                    allGraphs[graph.id] = build(graph);
                });
            },
            get: function (graphName) {
                return allGraphs[graphName];
            }
        };
    })();
})();