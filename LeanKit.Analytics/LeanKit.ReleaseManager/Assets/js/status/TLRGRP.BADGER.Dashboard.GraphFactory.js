(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard.GraphFactory');

    TLRGRP.BADGER.Dashboard.GraphFactory = function (currentTimePeriod) {
        var chartOptions = {
            lockToZero: true
        };

        function getGraphsFor() {
            function getGraphFor(graph) {
                var graphClass;
                var instanceChartOptions = graph.chartOptions;
                var expressionFilter = graph.expressions;
                var additionalExpressionFilters = graph.additionalExpressionFilters;
                var graphTitle = graph.title;

                if (typeof graph === 'object') {
                    if (graph.slots === 2) {
                        graphClass = 'half';
                    }
                    graph = graph.id;
                }

                var selectedGraph = TLRGRP.BADGER.Dashboard.Graphs.get(graph);
                var currentTimitSelectDataString = TLRGRP.BADGER.Cube.convertTimePeriod(currentTimePeriod);
                var selectedExpressions = selectedGraph.expressions;

                if (expressionFilter && expressionFilter) {
                    selectedExpressions = _(selectedExpressions).filter(function (graphExpression) {
                        if (_(expressionFilter).contains(graphExpression.id)) {
                            return graphExpression;
                        }
                    });
                }

                delete selectedGraph.expressions;

                return $.extend(true, {}, selectedGraph, {
                    'class': graphClass,
                    title: graphTitle,
                    expressions: _.map(selectedExpressions, function (expression) {
                        var currentExpression = expression.expression;
                        currentExpression = currentExpression.setTimePeriod(currentTimitSelectDataString);

                        if (additionalExpressionFilters) {
                            _(additionalExpressionFilters).each(function (expressionFilter) {
                                currentExpression = currentExpression[expressionFilter.filter](expressionFilter.key, expressionFilter.value);
                            });
                        }

                        expression.expression = currentExpression.build();

                        if (!expression.id) {
                            var autoTitle = (selectedGraph.title ? selectedGraph.title + '-' : '') + expression.title;
                            expression.id = autoTitle.toLowerCase().replace(/\s/g, '-').replace(/[()]/g, '');
                        }

                        return expression;
                    }),
                    chartOptions: $.extend({}, chartOptions, selectedGraph.chartOptions, instanceChartOptions)
                });
            }

            return _.map(arguments, function (graphItem) {
                return getGraphFor(graphItem);
            });
        }

        return {
            getGraphsFor: getGraphsFor
        };
    };
})();