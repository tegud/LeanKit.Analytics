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

        if (selectedView.expressionBuilder && $.isFunction(selectedView.expressionBuilder)) {
            return selectedView.expressionBuilder(eventType, metric, machineName, metricGroup, stepAndLimit);
        }

        return ['median(' + eventType + '(' + metric + ')',
            '.eq(source_host,"' + machineName + '")',
            '.eq(metricGroup,"' + metricGroup + '"))&',
            stepAndLimit].join('');
    }
    
    TLRGRP.BADGER.Dashboard.WebHosts = function () {
        function metricGroupedByHost(selectedView, currentTimitSelectDataString) {
            var maxPerGroup = 5;
            var metricGroups = [];
            var currentMetricGroup = -1;
            var graphs = [];

            for (var i = 0; i < 19; i++) {
                if (!(i % maxPerGroup)) {
                    metricGroups[++currentMetricGroup] = [];
                }

                metricGroups[currentMetricGroup][metricGroups[currentMetricGroup].length] = i + 1;
            }

            for (var n = 0; n < metricGroups.length; n++) {
                var expressions = [];
                var title = selectedView.name + ' by hosts ';

                for (var m = 0; m < metricGroups[n].length; m++) {
                    var machineId = metricGroups[n][m];
                    var machineName = getMachineName(machineId);

                    if (!m) {
                        title += machineId + '-';
                    } else if (m === metricGroups[n].length - 1) {
                        title += machineId;
                    }

                    expressions[expressions.length] = {
                        id: machineName,
                        title: machineName,
                        color: colors[m % colors.length],
                        expression: buildExpression(selectedView, machineName, currentTimitSelectDataString)
                    };
                }

                graphs[graphs.length] = {
                    title: title,
                    'class': 'half',
                    expressions: expressions,
                    chartOptions: selectedView.chartOptions || {}
                };
            }

            return graphs;
        }

        var diskExpression = function (eventType, metric, machineName, metricGroup, stepAndLimit) {
            return ['min(' + eventType + '(' + metric + ')',
                '.eq(source_host,"' + machineName + '")',
                '.eq(metricGroup,"' + metricGroup + '")) / 1073741824',
                stepAndLimit].join('');
        };
        
        var views = {
            'Requests': {
                defaultSubMetric: 'RequestsExecuting',
                subMetrics: {
                    'RequestsExecuting': {
                        name: 'Requests Executing',
                        metric: 'ASPNET2__Total_RequestsExecuting',
                        group: 'ASPNET2',
                        eventType: 'lr_web_wmi',
                        chartOptions: {
                            yAxisLabel: 'requests',
                        }
                    },
                    'RequestsPerSec': {
                        name: 'Requests /s',
                        metric: 'ASPNET2__Total_RequestsPerSec',
                        group: 'ASPNET2',
                        eventType: 'lr_web_wmi',
                        chartOptions: {
                            yAxisLabel: 'requests',
                        }
                    },
                    'ExecutionTime': {
                        name: 'Execution Time',
                        metric: 'ASPNET2__Total_RequestExecutionTime',
                        group: 'ASPNET2',
                        eventType: 'lr_web_wmi',
                        chartOptions: {
                            dimensions: {
                                margin: { left: 50 }
                            },
                            yAxisLabel: 'time (s)',
                        }
                    }
                }
            },
            'Performance': {
                defaultSubMetric: 'CPU',
                subMetrics: {
                    'CPU': {
                        name: 'CPU',
                        metric: 'cpu__Total_PercentProcessorTime',
                        group: 'cpu',
                        eventType: 'lr_web_wmi',
                        chartOptions: {
                            axisExtents: {
                                y: [0, 100]
                            },
                            yAxisLabel: '%',
                        }
                    }
                }
            },
            'Disk': {
                defaultSubMetric: 'DiskSpaceD',
                subMetrics: {
                    'DiskSpaceC': {
                        name: 'Disk (C:)',
                        metric: 'disk_0_FreeSpace',
                        group: 'disk',
                        eventType: 'lr_web_wmi',
                        chartOptions: {
                            yAxisLabel: 'GB Remaining',
                        },
                        defaults: {
                            step: '3e5',
                            limit: 48
                        },
                        expressionBuilder: diskExpression
                    },
                    'DiskSpaceD': {
                        name: 'Disk (D:)',
                        metric: 'disk_1_FreeSpace',
                        group: 'disk',
                        eventType: 'lr_web_wmi',
                        chartOptions: {
                            yAxisLabel: 'GB Remaining',
                        },
                        defaults: {
                            step: '3e5',
                            limit: 48
                        },
                        expressionBuilder: diskExpression
                    }
                }
            }
        };

        return {
            toString: function () {
                return 'Webhosts';
            },
            supportsView: function (view) {
                if (views[view]) {
                    return true;
                }

                return false;
            },
            appendViews: function (allViews) {
                return $.extend(allViews, views);
            },
            getGraphs: metricGroupedByHost
        };
    };
})();