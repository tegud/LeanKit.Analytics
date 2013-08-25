(function() {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    var colors = ['steelblue', 'red', 'orange', 'green', 'purple'];

    function buildExpression(selectedView, machineName, stepAndLimit) {
        var metric = selectedView.metric;
        var metricGroup = selectedView.group;
        var eventType = selectedView.eventType;
        var divideBy = selectedView.divideBy;

        if (selectedView.expressionBuilder && $.isFunction(selectedView.expressionBuilder)) {
            return selectedView.expressionBuilder(eventType, metric, machineName, metricGroup, stepAndLimit);
        }

        return ['median(' + eventType + '(' + metric + ')',
            '.eq(source_host,"' + machineName + '")',
            '.eq(metricGroup,"' + metricGroup + '"))' + (divideBy || '') + '&',
            stepAndLimit].join('');
    }

    TLRGRP.BADGER.Dashboard.ByHost = function () {
        var currentTimePeriod = '1hour';
        var currentViewName;
        var currentSubMetricName;
        var currentView;
        var currentSubMetric;
        
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
                    isSelected: 'HostView' === currentViewName
                };
                    
                if ('HostView' === currentViewName) {
                    var allServers = TLRGRP.BADGER.Machines.getAllServers();
                    var allServerLength = allServers.length;
                    var x;
                    var serverNameRegex = /TELWEB0{0,3}([0-9]){1,3}P/;
                    
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
            },
            clearView: function () {
                currentViewName = '';
                currentView = '';
                currentSubMetric = '';
            },
            setTimePeriod: function (timePeriod) {
                currentTimePeriod = timePeriod;
            },
            getGraphs: function () {
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

                        expressions[expressions.length] = {
                            id: metrics[x],
                            title: wmiMetric.name,
                            color: colors[x % colors.length],
                            expression: buildExpression(wmiMetric, currentSubMetricName, currentTimitSelectDataString)
                        };
                    }

                    return {
                        title: options.name || name,
                        'class': 'half',
                        expressions: expressions,
                        chartOptions: $.extend({}, chartOptions, options.graphOptions)
                    };
                }

                return [
                    buildGraph('RequestsExecuting'),
                    buildGraph('CPU'),
                    buildGraph('Memory'),
                    buildGraph(['Gen0GarbageCollection', 'Gen1GarbageCollection', 'Gen2GarbageCollection'], {
                        name: 'Garbage Collection',
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