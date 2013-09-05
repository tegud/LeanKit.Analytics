(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard.Graphs');

    var colors = new TLRGRP.BADGER.ColorPalette();

    TLRGRP.BADGER.Dashboard.Graphs.register({
        id: 'AllErrors',
        source: 'Errors',
        title: 'Errors',
        expressions: ['AllErrors'],
        chartOptions: {
            legend: false,
            yAxisLabel: ''
        }
    },
        {
            id: 'UserJourneyErrors',
            source: 'Errors',
            title: 'User Journey (pre-booking form) Errors',
            expressions: ['SearchErrors', 'HotelDetailsErrors']
        },
        {
            id: 'BookingErrors',
            source: 'Errors',
            title: 'Booking Errors',
            expressions: [{ id: 'BookingErrors', color: colors.getColorByIndex(0) }],
            chartOptions: {
                legend: false,
                dimensions: {
                    margin: { right: 20 }
                }
            }
        },
        {
            id: 'IPGErrors',
            source: 'Errors',
            title: 'IPG Booking',
            expressions: [
                { id: 'IPGRequestTimeoutErrors', color: colors.getColorByIndex(0) },
                { id: 'IPGSessionTimeoutErrors', color: colors.getColorByIndex(2) },
                { id: 'IPGInvalidSessionErrors', color: colors.getColorByIndex(4) }
            ],
            chartOptions: {
                dimensions: {
                    margin: { right: 110 }
                }
            }
        });
})();