(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    TLRGRP.BADGER.Dashboard.ByPage = function () {
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
                        expression.expression = expression.expression
                            .equalTo('pagetype', subMetrics[currentSubMetric].pageType)
                            .setTimePeriod(currentTimitSelectDataString)
                            .build();

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
            'HomePage': {
                name: 'Home Page',
                pageType: 'home-page',
                getGraphs: function () {
                    return getGraphsFor('TrafficByType');
                }
            },
            'Search': {
                getGraphs: function () {
                    return [];
                }
            },
            'HotelDetails': {
                name: 'Hotel Details',
                getGraphs: function () {
                    return [];
                }
            },
            'BookingForm': {
                name: 'Booking Form',
                getGraphs: function () {
                    return [];
                }
            }
        };

        return {
            toString: function () {
                return 'ByPage';
            },
            appendViews: function (allViews) {
                return $.extend(allViews, {
                    'ByPage': {}
                });
            },
            appendViewModel: function (viewModel) {
                viewModel.dashboardViews[viewModel.dashboardViews.length] = {
                    name: 'By Page',
                    metric: 'ByPage',
                    isSelected: isSelected
                };

                if (isSelected) {
                    viewModel.timePeriod = currentTimePeriod;
                    viewModel.pageName = 'By Page';

                    for (var subMetric in subMetrics) {
                        if (!subMetrics.hasOwnProperty(subMetric)) {
                            continue;
                        }

                        viewModel.subMetrics[viewModel.subMetrics.length] = {
                            name: subMetrics[subMetric].name || subMetric,
                            metric: 'ByPage',
                            subMetric: subMetric,
                            isSelected: currentSubMetric === subMetric
                        };
                    }
                }
            },
            supportsView: function (view) {
                return view === 'ByPage';
            },
            setView: function (view, subMetric) {
                isSelected = true;
                currentSubMetric = subMetric || 'HomePage';
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