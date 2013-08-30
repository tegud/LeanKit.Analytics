(function () {
    TLRGRP.namespace('TLRGRP.BADGER.WMI');

    TLRGRP.BADGER.IIS = (function () {
        var allMetrics = {
            'AllRequests': {
                name: 'Requests',
                expression: 'sum(lr_web_request)',
                chartOptions: {
                    lockToZero: true,
                    yAxisLabel: 'requests'
                }
            }
        };
        
        //{
        //    id: 'iis-all',
        //    title: 'All',
        //    color: colors[0],
        //    expression: 'sum(lr_web_request)&' + currentTimitSelectDataString
        //}
        
        return {
            metricInfo: function (metricName) {
                return allMetrics[metricName];
            }
        };
    })();
})();