(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    TLRGRP.BADGER.Dashboard.Overview = function () {
        var isSelected;
        var currentTimePeriod = '1hour';
        var chartOptions = {
            lockToZero: true
        };
        var currentSubMetric;
        
        function getGraphsFor() {
            function getGraphFor(graph) {
                var graphClass;
                var instanceChartOptions = graph.chartOptions;

                if (typeof graph === 'object') {
                    if (graph.slots === 2) {
                        graphClass = 'half';
                    }
                    graph = graph.id;
                }

                var selectedGraph = TLRGRP.BADGER.Dashboard.Graphs.get(graph);
                var currentTimitSelectDataString = TLRGRP.BADGER.Cube.convertTimePeriod(currentTimePeriod);

                return $.extend(true, {}, selectedGraph, {
                    'class': graphClass,
                    expressions: _.map(selectedGraph.expressions, function (expression) {
                        expression.expression = expression.expression.setTimePeriod(currentTimitSelectDataString).build();

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

        var subMetrics = {
            'Summary': {
                getGraphs: function () {
                    return getGraphsFor({ id: 'TrafficByType', slots: 2 },
                        {
                            id: 'AllErrors',
                            slots: 2,
                            chartOptions: {
                                dimensions: {
                                    margin: { right: 20 }
                                }
                            }
                        },
                        { id: 'ResponseTimeByPage', slots: 2 },
                        { id: 'StatusCodes', slots: 2 });
                }
            },
            'Traffic': {
                getGraphs: function () {
                    return getGraphsFor('TrafficByPage',
                        { id: 'TrafficByType', slots: 2 },
                        { id: 'TrafficByChannel', slots: 2 });
                }
            },
            'Errors': {
                getGraphs: function () {
                    return getGraphsFor({
                        id: 'AllErrors',
                        chartOptions: {
                            dimensions: {
                                margin: { right: 20 }
                            }
                        }
                    },
                        { id: 'UserJourneyErrors', slots: 2 },
                        { id: 'BookingErrors', slots: 2 });
                }
            },
            'IPG': {
                defaultTimePeriod: '4hours',
                getGraphs: function () {
                    return getGraphsFor('IPGErrors', 'IPGResponseTime');
                }
            }
        };

        return {
            toString: function () {
                return 'Overview';
            },
            appendViews: function (allViews) {
                return $.extend(allViews, {
                    'Overview': {}
                });
            },
            appendViewModel: function (viewModel) {
                viewModel.dashboardViews[viewModel.dashboardViews.length] = {
                    name: 'Overview',
                    metric: 'Overview',
                    isSelected: isSelected
                };

                if (isSelected) {
                    viewModel.timePeriod = currentTimePeriod;
                    viewModel.pageName = 'Overview';

                    for (var subMetric in subMetrics) {
                        if (!subMetrics.hasOwnProperty(subMetric)) {
                            continue;
                        }

                        viewModel.subMetrics[viewModel.subMetrics.length] = {
                            name: subMetric,
                            metric: 'Overview',
                            subMetric: subMetric,
                            isSelected: currentSubMetric === subMetric
                        };
                    }
                }
            },
            supportsView: function (view) {
                return view === 'Overview';
            },
            setView: function (view, subMetric) {
                isSelected = true;
                currentSubMetric = subMetric || 'Summary';
                if (subMetrics[currentSubMetric].defaultTimePeriod) {
                    currentTimePeriod = subMetrics[currentSubMetric].defaultTimePeriod;
                }
            },
            clearView: function () {
                isSelected = false;
                currentSubMetric = '';
            },
            setTimePeriod: function (timePeriod) {
                currentTimePeriod = timePeriod;
            },
            getGraphs: function () {
                var currentTimitSelectDataString = TLRGRP.BADGER.Cube.convertTimePeriod(currentTimePeriod);

                return subMetrics[currentSubMetric].getGraphs(currentTimitSelectDataString);
            }
        };
    };
})();