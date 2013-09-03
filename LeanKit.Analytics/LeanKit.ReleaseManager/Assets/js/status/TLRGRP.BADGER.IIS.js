(function () {
    TLRGRP.namespace('TLRGRP.BADGER.WMI');

    TLRGRP.BADGER.IIS = (function () {
        function iisExpressionBuilder() {
            return (function () {
                var currentTimitSelectDataString = TLRGRP.BADGER.Cube.convertTimePeriod(currentTimePeriod);
                return new TLRGRP.BADGER.Cube.ExpressionBuilder('lr_web_request').setTimePeriod(currentTimitSelectDataString);
            });
        }

        var iis = iisExpressionBuilder();

        var allMetrics = {
            'TrafficByType': {
                title: 'Traffic by Type',
                expressions: [{
                        title: 'All',
                        expression: iis().sum()
                    },
                    {
                        title: 'Bot',
                        expression: iis().sum().equalTo('isbot', true)
                    },
                    {
                        title: 'Mobile',
                        expression: iis().sum().equalTo('isbot', false).equalTo('ismobile', true)
                    }],
                chartOptions: {
                    yAxisLabel: 'requests'
                }
            },
            'ResponseTimeByPage': {
                title: 'Response Time by Page',
                expressions: [{
                        title: 'Home Page',
                        expression: iis().median('duration').equalTo('pagetype', 'home-page')
                    },
                    {
                        title: 'Search',
                        expression: iis().median('duration').equalTo('pagetype', 'search')
                    },
                    {
                        title: 'Hotel Details',
                        expression: iis().median('duration').equalTo('pagetype', 'hotel-details')
                    },
                    {
                        title: 'Booking Form',
                        expression: iis().median('duration').equalTo('pagetype', 'booking-form')
                    }],
                chartOptions: {
                    yAxisLabel: 'request time (ms)'
                }
            },
            'StatusCodes': {
                title: 'Status Codes (non 200)',
                expressions: [{
                        title: '404 (Not Found)',
                        expression: iis().sum().equalTo('status', 404)
                    }, {
                        title: '500 (Error)',
                        expression: iis().sum().equalTo('status', 500)
                    }, {
                        title: '30x (Redirect)',
                        expression: iis().sum().in('status', [301, 302])
                    }],
                chartOptions: {
                    yAxisLabel: 'requests'
                }
            },
            'TrafficByChannel': {
                title: 'Traffic by Channel',
                expressions: [{
                        title: 'Direct',
                        expression: iis().sum().equalTo('pagechannel', 'web')
                    },
                    {
                        title: 'Mobile',
                        expression: iis().sum().equalTo('pagechannel', 'mobile')
                    },
                    {
                        title: 'Affiliate',
                        expression: iis().sum().equalTo('pagechannel', 'affiliate')
                    }],
                chartOptions: {
                    yAxisLabel: 'requests'
                }
            },
            'TrafficByPage': {
                title: 'Traffic by Page',
                expressions: [{
                        title: 'Home Page',
                        expression: iis().sum().equalTo('pagetype', 'home-page')
                    },
                    {
                        title: 'Search',
                        expression: iis().sum().equalTo('pagetype', 'search')
                    },
                    {
                        title: 'Hotel Details',
                        expression: iis().sum().equalTo('pagetype', 'hotel-details')
                    },
                    {
                        title: 'Booking Form',
                        expression: iis().sum().equalTo('pagetype', 'booking-form')
                    }],
                chartOptions: {
                    yAxisLabel: 'requests'
                }
            },
            'IPGResponseTime': {
                title: 'Average Time for Tokenisation',
                expressions: [{
                    expression: iis().sum().matchesRegEx('url', '/beacon/pageresponse')
                }],
                chartOptions: {
                    yAxisLabel: 'duration (ms)'
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