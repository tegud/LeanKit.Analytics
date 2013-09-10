(function() {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    var colors = ['steelblue', 'red', 'orange', 'green', 'purple'];

    TLRGRP.BADGER.Dashboard.ByHost = function () {
        var currentTimePeriod = '1hour';
        var currentViewName;
        var currentSubMetricName;
        var currentView;
        var currentSubMetric;
        var isSelected;

        return {
            toString: function () {
                return 'Host View';
            },
            supportsView: function (view) {
                return view === 'HostView';
            },
            appendViewModel: function (viewModel) {
                if (currentViewName) {
                    viewModel.pageName = 'View Metrics for ' + (currentSubMetricName || 'Host');
                    viewModel.timePeriod = currentTimePeriod;
                }

                viewModel.dashboardViews[viewModel.dashboardViews.length] = {
                    name: 'By Server',
                    metric: 'HostView',
                    isSelected: isSelected
                };
                    
                if (isSelected) {
                    var allServers = TLRGRP.BADGER.Machines.getAllServers();
                    var allServerLength = allServers.length;
                    var x;
                    var serverNameRegex = /TELWEB[0]{0,3}([0-9]{1,3})P/;
                    
                    for (x = 0; x < allServerLength; x++) {
                        viewModel.subMetrics[viewModel.subMetrics.length] = {
                            name: allServers[x].replace(serverNameRegex, '$1'),
                            metric: 'HostView',
                            subMetric: allServers[x],
                            isSelected: currentSubMetricName === allServers[x]
                        };
                    }
                }
            },
            setView: function (view, subMetric) {
                currentViewName = view;
                currentSubMetricName = subMetric || 'TELWEB001P';
                isSelected = true;
            },
            clearView: function () {
                isSelected = false;
                currentViewName = '';
                currentView = '';
                currentSubMetric = '';
            },
            setTimePeriod: function (timePeriod) {
                currentTimePeriod = timePeriod;
            },
            getComponents: function () {
                var currentTimitSelectDataString = TLRGRP.BADGER.Cube.convertTimePeriod(currentTimePeriod);
                var chartOptions = {
                    lockToZero: true,
                    legend: false,
                    dimensions: {
                        margin: {
                            right: 10
                        }
                    }
                };

                var defaultGraphBuilderOptions = {
                    graphOptions: {}
                };

                function buildGraph(metrics, options) {
                    var metricsLength;
                    var x;
                    var expressions = [];
                    var name = 'Untitled';
                    var allMetricChartOptions = [true, {}, chartOptions];

                    options = $.extend({}, defaultGraphBuilderOptions, options);

                    if (typeof metrics === 'string') {
                        if (options.name) {
                            name = options.name;
                        }
                        
                        metrics = [metrics];
                    }

                    metricsLength = metrics.length;

                    for (x = 0; x < metricsLength; x++) {
                        var wmiMetric = TLRGRP.BADGER.WMI.metricInfo(metrics[x]);

                        if (name === 'Untitled') {
                            name = wmiMetric.name;
                        }

                        allMetricChartOptions[allMetricChartOptions.length] = wmiMetric.chartOptions;

                        expressions[expressions.length] = {
                            id: metrics[x],
                            title: wmiMetric.name,
                            color: colors[x % colors.length],
                            expression: TLRGRP.BADGER.Cube.WMI.buildExpression(wmiMetric, currentSubMetricName, currentTimitSelectDataString)
                        };
                    }

                    allMetricChartOptions[allMetricChartOptions.length] = options.graphOptions;

                    return {
                        title: options.name || name,
                        'class': 'half',
                        expressions: expressions,
                        chartOptions: $.extend.apply(this, allMetricChartOptions)
                    };
                }

                return [
                    buildGraph('RequestsExecuting'),
                    buildGraph('CPU'),
                    buildGraph('Memory'),
                    buildGraph(['Gen0GarbageCollection1', 'Gen1GarbageCollection1', 'Gen2GarbageCollection1'], {
                        name: 'Garbage Collection 1',
                        graphOptions: {
                            dimensions: {
                                margin: {
                                    left: 50,
                                    right: 90
                                }
                            },
                            legend: {
                                textAlign: 'end'
                            }
                        }
                    }),
                    buildGraph(['Gen0GarbageCollection2', 'Gen1GarbageCollection2', 'Gen2GarbageCollection2'], {
                        name: 'Garbage Collection 2',
                        graphOptions: {
                            dimensions: {
                                margin: {
                                    left: 50,
                                    right: 90
                                }
                            },
                            legend: {
                                textAlign: 'end'
                            }
                        }
                    }),
                    buildGraph(['Gen0GarbageCollection3', 'Gen1GarbageCollection3', 'Gen2GarbageCollection3'], {
                        name: 'Garbage Collection 3',
                        graphOptions: {
                            dimensions: {
                                margin: {
                                    left: 50,
                                    right: 90
                                }
                            },
                            legend: {
                                textAlign: 'end'
                            }
                        }
                    }),
                    buildGraph(['PercentTimeinGC1', 'PercentTimeinGC2', 'PercentTimeinGC3'], {
                        name: 'Time in GC',
                        graphOptions: {
                            dimensions: {
                                margin: {
                                    left: 50,
                                    right: 90
                                }
                            },
                            legend: {
                                textAlign: 'end'
                            }
                        }
                    })];
            }
        };
    };
})();