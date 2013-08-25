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

        return {
            metricInfo: function (metricName) {
                return allMetrics[metricName];
            }
        };
    })();
})();