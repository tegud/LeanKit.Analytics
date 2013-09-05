(function () {
    TLRGRP.namespace('TLRGRP.BADGER.WMI');

    TLRGRP.BADGER.Errors = (function () {
        var errors = (function () {
            return (function () {
                return new TLRGRP.BADGER.Cube.ExpressionBuilder('no_type');
            });
        })();

        var allMetrics = {
            'AllErrors': {
                title: 'Errors',
                expression: errors().sum(),
                chartOptions: {
                    yAxisLabel: 'errors'
                }
            },
            'SearchErrors': {
                title: 'Search',
                expression: errors().sum().sum().matchesRegEx('Url', 'Search|(H|h)otels'),
                chartOptions: {
                    yAxisLabel: 'errors'
                }
            },
            'HotelDetailsErrors': {
                title: 'Hotel Details',
                expression: errors().sum().sum().matchesRegEx('Url', 'hotel-reservations'),
                chartOptions: {
                    yAxisLabel: 'errors'
                }
            },
            'BookingErrors': {
                title: 'Booking Form',
                expression: errors().sum().sum().matchesRegEx('Url', '(BookingError/LogError\.mvc|Booking/Online|HotelReservationsSubmit/Submit|Booking/Submit)'),
                chartOptions: {
                    yAxisLabel: 'errors'
                }
            },
            'IPGRequestTimeoutErrors': {
                title: 'Request Timeout',
                expression: errors().sum()
                    .matchesRegEx('Url', '(BookingError/LogError\.mvc)')
                    .matchesRegEx('Exception.Message', 'request_timeout'),
                chartOptions: {
                    yAxisLabel: 'errors'
                }
            },
            'IPGSessionTimeoutErrors': {
                title: 'Session Timeout',
                expression: errors().sum()
                    .matchesRegEx('Url', '(BookingError/LogError\.mvc)')
                    .matchesRegEx('Exception.Message', 'session_timeout'),
                chartOptions: {
                    yAxisLabel: 'errors'
                }
            },
            'IPGInvalidSessionErrors': {
                title: 'Invalid Session',
                expression: errors().sum()
                    .matchesRegEx('Url', '(BookingError/LogError\.mvc)')
                    .matchesRegEx('Exception.Message', 'invalid_session'),
                chartOptions: {
                    yAxisLabel: 'errors'
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