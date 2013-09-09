(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    TLRGRP.BADGER.Dashboard.Overview = function () {
        var subMetrics = {
            'Summary': [
                { id: 'TrafficByType', slots: 2 },
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
                { id: 'StatusCodes', slots: 2 }
            ],
            'Traffic': [
                'TrafficByPage',
                { id: 'TrafficByType', slots: 2 },
                { id: 'TrafficByChannel', slots: 2 }
            ],
            'Errors': [{
                    id: 'AllErrors',
                    chartOptions: {
                        dimensions: {
                            margin: { right: 20 }
                        }
                    }
                },
                { id: 'UserJourneyErrors', slots: 2 },
                { id: 'BookingErrors', slots: 2 }],
            'IPG': [
                'IPGErrors', 'IPGResponseTime'
            ]
        };

        return new TLRGRP.BADGER.Dashboard.GraphSet({
            metric: 'Overview',
            subMetrics: subMetrics
        });
    };
})();