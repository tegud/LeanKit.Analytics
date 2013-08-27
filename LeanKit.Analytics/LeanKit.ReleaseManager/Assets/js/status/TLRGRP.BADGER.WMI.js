(function () {
    TLRGRP.namespace('TLRGRP.BADGER.WMI');

    TLRGRP.BADGER.WMI = (function () {
        var wmiEventType = 'lr_web_wmi';
        var allMetrics = {
            'RequestsExecuting': {
                name: 'Requests Executing',
                metric: 'ASPNET2__Total_RequestsExecuting',
                group: 'ASPNET2',
                eventType: wmiEventType,
                chartOptions: {
                    lockToZero: true,
                    yAxisLabel: 'requests'
                }
            },
            'RequestsPerSec': {
                name: 'Requests /s',
                metric: 'ASPNET2__Total_RequestsPerSec',
                group: 'ASPNET2',
                eventType: wmiEventType,
                chartOptions: {
                    lockToZero: true,
                    yAxisLabel: 'requests /s'
                }
            },
            'ExecutionTime': {
                name: 'Execution Time',
                metric: 'ASPNET2__Total_RequestExecutionTime',
                group: 'ASPNET2',
                eventType: wmiEventType,
                chartOptions: {
                    lockToZero: true,
                    dimensions: {
                        margin: { left: 50 }
                    },
                    yAxisLabel: 'time (s)',
                }
            },
            'CPU': {
                name: 'CPU',
                metric: 'cpu__Total_PercentProcessorTime',
                group: 'cpu',
                eventType: wmiEventType,
                chartOptions: {
                    axisExtents: {
                        y: [0, 100]
                    },
                    yAxisLabel: '%',
                }
            },
            'Memory': {
                name: 'Memory',
                metric: 'memory_AvailableMBytes',
                group: 'memory',
                eventType: wmiEventType,
                divideBy: '/1024',
                chartOptions: {
                    yAxisLabel: 'GB Available',
                    lockToZero: true
                },
                defaults: {
                    timePeriod: '4hours'
                }
            },
            'DiskSpaceC': {
                name: 'Disk (C:)',
                metric: 'disk_0_FreeSpace',
                group: 'disk',
                eventType: wmiEventType,
                chartOptions: {
                    lockToZero: true,
                    yAxisLabel: 'GB Remaining'
                },
                divideBy: '/1073741824',
                defaults: {
                    timePeriod: '4hours'
                }
            },
            'DiskSpaceD': {
                name: 'Disk (D:)',
                metric: 'disk_1_FreeSpace',
                group: 'disk',
                eventType: wmiEventType,
                chartOptions: {
                    lockToZero: true,
                    yAxisLabel: 'GB Remaining'
                },
                divideBy: '/1073741824',
                defaults: {
                    timePeriod: '4hours'
                }
            }
        };
        
        function appendGarbageCollectionCounters() {
            var templates = {
                'Gen0GarbageCollection': {
                    name: 'Gen 0',
                    metric: 'GarbageCollection_{ID}_NumberGen0Collections'
                },
                'Gen1GarbageCollection': {
                    name: 'Gen 1',
                    metric: 'GarbageCollection_{ID}_NumberGen1Collections'
                },
                'Gen2GarbageCollection': {
                    name: 'Gen 2',
                    metric: 'GarbageCollection_{ID}_NumberGe2Collections'
                },
                'PercentTimeinGC': {
                    name: 'GC {ID}',
                    metric: 'GarbageCollection_{ID}_PercentTimeinGC',
                }
            };

            for (var x = 1; x < 4; x++) {
                for (var template in templates) {
                    if (!templates.hasOwnProperty(template)) {
                        continue;
                    }

                    var metricTemplate = templates[template];
                    var metricName = template + x;

                    allMetrics[metricName] = {
                        name: metricTemplate.name.replace('{ID}', x),
                        metric: metricTemplate.metric.replace('{ID}', x),
                        group: 'GarbageCollection',
                        eventType: wmiEventType
                    };
                }
            }
        }

        appendGarbageCollectionCounters();

        return {
            metricInfo: function (metricName) {
                return allMetrics[metricName];
            }
        };
    })();
})();