(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard.Graphs');

    var colors = new TLRGRP.BADGER.ColorPalette();

    TLRGRP.BADGER.Dashboard.Graphs.register({
        id: 'TrafficByType',
        source: 'IIS',
        title: 'Traffic by Type',
        expressions: ['AllTraffic', 'BotTraffic', 'MobileTraffic'],
        chartOptions: {
            dimensions: {
                margin: { left: 50 }
            }
        }
    }, {
        id: 'ResponseTimeByPage',
        source: 'IIS',
        title: 'Response Time by Page',
        expressions: ['HomePageServerResponseTime', 'SearchServerResponseTime', 'HotelDetailsServerResponseTime', 'BookingFormServerResponseTime'],
        chartOptions: {
            dimensions: {
                margin: { left: 50 }
            }
        }
    }, {
        id: 'StatusCodes',
        source: 'IIS',
        title: 'Status Codes (non 200)',
        expressions: [{ id: 'NotFoundResponse', color: colors.getColorByIndex(2) },
            'ErrorResponse',
            { id: 'RedirectResponse', color: colors.getColorByIndex(0) }],
        chartOptions: {
            dimensions: {
                margin: {
                    left: 50,
                    right: 100
                }
            }
        }
    }, {
        id: 'TrafficByChannel',
        source: 'IIS',
        title: 'Traffic by Channel',
        expressions: ['ChannelDirectRequests', 'ChannelMobileRequests', 'ChannelAffiliateRequests'],
        chartOptions: {
            dimensions: {
                margin: {
                    left: 50,
                    right: 100
                }
            }
        }
    }, {
        id: 'TrafficByPage',
        source: 'IIS',
        title: 'Traffic by Page',
        expressions: ['HomePageRequests', 'SearchRequests', 'HotelDetailsRequests', 'BookingFormRequests'],
        chartOptions: {
            dimensions: {
                margin: {
                    left: 50
                }
            }
        }
    }, {
        id: 'IPGResponseTime',
        source: 'IIS',
        title: 'Average Time for Tokenisation',
        expressions: ['IPGResponseTime'],
        chartOptions: {
            legend: false
        }
    });
})();