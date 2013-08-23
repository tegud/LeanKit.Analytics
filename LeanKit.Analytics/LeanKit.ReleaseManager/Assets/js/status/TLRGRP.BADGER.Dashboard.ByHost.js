(function() {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    var colors = ['steelblue', 'red', 'orange', 'green', 'purple'];
    
    function getMachineName(machineNumber) {
        var machineNumberLength = (machineNumber + '').length;
        var id = machineNumber;

        for (var x = machineNumberLength; x < 3; x++) {
            id = '0' + id;
        }

        return 'TELWEB' + id + 'P';
    }

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
                }

                viewModel.timePeriod = currentTimePeriod;

                viewModel.dashboardViews[viewModel.dashboardViews.length] = {
                    name: 'By Server',
                    metric: 'HostView',
                    isSelected: 'HostView' === currentViewName
                };
                    
                if ('HostView' === currentViewName) {
                    for (var x = 1; x < 20; x++) {
                        var machine = getMachineName(x);

                        viewModel.subMetrics[viewModel.subMetrics.length] = {
                            name: x,
                            metric: 'HostView',
                            subMetric: machine,
                            isSelected: currentSubMetricName === machine
                        };
                    }
                    
                    for (x = 107; x < 110; x++) {
                        var machine = getMachineName(x);

                        viewModel.subMetrics[viewModel.subMetrics.length] = {
                            name: x,
                            metric: 'HostView',
                            subMetric: machine,
                            isSelected: currentSubMetricName === machine
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
                    legend: false,
                    dimensions: {
                        margin: {
                            right: 10
                        }
                    }
                };

                return [{
                        title: 'Requests Executing',
                        'class': 'half',
                        expressions: [{
                            id: 'requests-executing',
                            color: colors[0],
                            expression: buildExpression({
                                metric: 'ASPNET2__Total_RequestsExecuting',
                                group: 'ASPNET2',
                                eventType: 'lr_web_wmi'
                            }, currentSubMetricName, currentTimitSelectDataString)
                        }],
                        chartOptions: $.extend({}, chartOptions, { yAxisLabel: 'requests' })
                    }, {
                        title: 'CPU',
                        'class': 'half',
                        expressions: [{
                                id: 'cpu',
                                color: colors[0],
                                expression: buildExpression({
                                    metric: 'cpu__Total_PercentProcessorTime',
                                    group: 'cpu',
                                    eventType: 'lr_web_wmi'
                                }, currentSubMetricName, currentTimitSelectDataString)
                            }
                        ],
                        chartOptions: $.extend({}, chartOptions, {
                            yAxisLabel: '%',
                            axisExtents: {
                                y: [0, 100]
                            }
                        })
                    },
                    {
                        title: 'Available Memory',
                        'class': 'half',
                        expressions: [{
                            id: 'cpu',
                            color: colors[0],
                            expression: buildExpression({
                                metric: 'memory_AvailableMBytes',
                                group: 'memory',
                                eventType: 'lr_web_wmi',
                                divideBy: '/1024'
                            }, currentSubMetricName, currentTimitSelectDataString)
                        }],
                        chartOptions: $.extend({}, chartOptions, {
                            yAxisLabel: 'GB'
                        })
                    },
                    {
                        title: 'Garbage Collection',
                        'class': 'half',
                        expressions: [{
                            id: 'gen0',
                            color: colors[0],
                            title: 'Gen0',
                            expression: buildExpression({
                                    metric: 'GarbageCollection_3_NumberGen0Collections',
                                    group: 'GarbageCollection',
                                    eventType: 'lr_web_wmi'
                                },
                                currentSubMetricName,
                                currentTimitSelectDataString)
                        }, {
                            id: 'gen1',
                            color: colors[1],
                            title: 'Gen1',
                            expression: buildExpression({
                                metric: 'GarbageCollection_3_NumberGen1Collections',
                                group: 'GarbageCollection',
                                eventType: 'lr_web_wmi'
                            },
                                currentSubMetricName,
                                currentTimitSelectDataString)
                        }, {
                            id: 'gen1',
                            color: colors[2],
                            title: 'Gen2',
                            expression: buildExpression({
                                metric: 'GarbageCollection_3_NumberGen2Collections',
                                group: 'GarbageCollection',
                                eventType: 'lr_web_wmi'
                            },
                                currentSubMetricName,
                                currentTimitSelectDataString)
                        }],
                        chartOptions: $.extend({}, chartOptions, {
                            dimensions: {
                                margin: {
                                    left: 50,
                                    right: 90
                                }
                            },
                            yAxisLabel: '',
                            legend: {
                                textAlign: 'end'
                            }
                        })
                    }
                ];
            }
        };
    };
})();