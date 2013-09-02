(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    var colors = ['steelblue', 'red', 'orange', 'green', 'purple'];
    var homePageColor = colors[0];
    var searchColor = colors[2];
    var bookingPageColor = colors[3];
    var hotelColor = colors[4];

    TLRGRP.BADGER.Dashboard.Overview = function () {
        var isSelected;
        var currentTimePeriod = '1hour';
        var chartOptions = {
            lockToZero: true
        };
        var currentSubMetric;

        function iisExpressionBuilder(timePeriod) {
            return (function () {
                return new TLRGRP.BADGER.Cube.ExpressionBuilder('lr_web_request').setTimePeriod(timePeriod);
            });
        }
        
        var subMetrics = {
            'Summary': {
                defaultTimePeriod: '1hour',
                getGraphs: function (currentTimitSelectDataString) {
                    var iis = iisExpressionBuilder(currentTimitSelectDataString);

                    return [{
                        title: 'Traffic by Type',
                        'class': 'half',
                        expressions: [{
                                id: 'iis-all',
                                title: 'All',
                                color: colors[0],
                                expression: iis().sum().build()
                            },
                            {
                                id: 'iis-bot',
                                title: 'Bot',
                                color: colors[1],
                                expression: iis().sum().equalTo('isbot', true).build()
                            },
                            {
                                id: 'iis-mobile',
                                title: 'Mobile',
                                color: colors[4],
                                expression: iis().sum().equalTo('isbot', false).equalTo('ismobile', true).build()
                            }],
                        chartOptions: $.extend({}, chartOptions, {
                            yAxisLabel: 'requests',
                            dimensions: {
                                margin: { left: 50 }
                            }
                        })
                    }, {
                        title: 'Errors',
                        'class': 'half',
                        expressions: [{
                            id: 'all-errors',
                            title: 'All Errors',
                            color: colors[1],
                            expression: 'sum(no_type)&' + currentTimitSelectDataString
                        }],
                        chartOptions: $.extend({}, chartOptions, {
                            legend: false
                        })
                    }, {
                        title: 'Response Time by Page',
                        'class': 'half',
                        expressions: [{
                            id: 'page-requests-home-page',
                            title: 'Home Page',
                            color: homePageColor,
                            expression: iis().median('duration').equalTo('pagetype', 'home-page').build()
                        },
                            {
                                id: 'page-requests-search',
                                title: 'Search',
                                color: searchColor,
                                expression: iis().median('duration').equalTo('pagetype', 'search').build()
                            },
                            {
                                id: 'page-requests-hotel-details',
                                title: 'Hotel Details',
                                color: hotelColor,
                                expression: iis().median('duration').equalTo('pagetype', 'hotel-details').build()
                            },
                            {
                                id: 'page-requests-booking-form',
                                title: 'Booking Form',
                                color: bookingPageColor,
                                expression: iis().median('duration').equalTo('pagetype', 'booking-form').build()
                            }],
                        chartOptions: $.extend({}, chartOptions, {
                            yAxisLabel: 'request time (ms)',
                            dimensions: {
                                margin: { left: 50 }
                            }
                        })
                    }, {
                        title: 'Status Codes (non 200)',
                        'class': 'half',
                        expressions: [ {
                            id: 'status-missing',
                            title: '404 (Not Found)',
                            color: colors[2],
                            expression: iis().sum().equalTo('status', 404).build()
                        }, {
                            id: 'status-error',
                            title: '500 (Error)',
                            color: colors[1],
                            expression: iis().sum().equalTo('status', 500).build()
                        }, {
                            id: 'status-redirect',
                            title: '30x (Redirect)',
                            color: colors[0],
                            expression: iis().sum().in('status', [301, 302]).build()
                        }],
                        chartOptions: $.extend({}, chartOptions, {
                            yAxisLabel: 'requests',
                            dimensions: {
                                margin: {
                                    left: 50,
                                    right: 100
                                }
                            }
                        })
                    }];
                }
            },
            'Traffic': {
                defaultTimePeriod: '1hour',
                getGraphs: function (currentTimitSelectDataString) {
                    var iis = iisExpressionBuilder(currentTimitSelectDataString);
                    
                    return [
                        {
                            title: 'Traffic by Page',
                            expressions: [{
                                id: 'page-requests-home-page',
                                title: 'Home Page',
                                color: homePageColor,
                                expression: iis().sum().equalTo('pagetype', 'home-page').build()
                            },
                                {
                                    id: 'page-requests-search',
                                    title: 'Search',
                                    color: searchColor,
                                    expression: iis().sum().equalTo('pagetype', 'search').build()
                                },
                                {
                                    id: 'page-requests-hotel-details',
                                    title: 'Hotel Details',
                                    color: hotelColor,
                                    expression: iis().sum().equalTo('pagetype', 'hotel-details').build()
                                },
                                {
                                    id: 'page-requests-booking-form',
                                    title: 'Booking Form',
                                    color: bookingPageColor,
                                    expression: iis().sum().equalTo('pagetype', 'booking-form').build()
                                }],
                            chartOptions: $.extend({}, chartOptions, {
                                yAxisLabel: 'requests',
                                dimensions: {
                                    margin: { left: 50 }
                                }
                            })
                        }, {
                            title: 'Traffic by Type',
                            'class': 'half',
                            expressions: [{
                                    id: 'iis-all',
                                    title: 'All',
                                    color: colors[0],
                                    expression: iis().sum().build()
                                },
                                {
                                    id: 'iis-bot',
                                    title: 'Bot',
                                    color: colors[1],
                                    expression: iis().sum().equalTo('isbot', true).build()
                                },
                                {
                                    id: 'iis-mobile',
                                    title: 'Mobile',
                                    color: colors[4],
                                    expression: iis().sum().equalTo('isbot', false).equalTo('ismobile', true).build()
                                }],
                            chartOptions: $.extend({}, chartOptions, {
                                yAxisLabel: 'requests',
                                dimensions: {
                                    margin: { left: 50 }
                                }
                            })
                        },
                        {
                            title: 'Traffic by Channel',
                            'class': 'half',
                            expressions: [{
                                    id: 'page-requests-web',
                                    title: 'Direct',
                                    color: homePageColor,
                                    expression: 'sum(lr_web_request.eq(pagechannel,"web"))&' + currentTimitSelectDataString
                                },
                                {
                                    id: 'page-requests-mobile',
                                    title: 'Mobile',
                                    color: searchColor,
                                    expression: 'sum(lr_web_request.eq(pagechannel,"mobile"))&' + currentTimitSelectDataString
                                },
                                {
                                    id: 'page-requests-affiliate',
                                    title: 'Affiliate',
                                    color: hotelColor,
                                    expression: 'sum(lr_web_request.eq(pagechannel,"affiliate"))&' + currentTimitSelectDataString
                                }],
                            chartOptions: $.extend({}, chartOptions, {
                                yAxisLabel: 'requests',
                                dimensions: {
                                    margin: { left: 50 }
                                }
                            })
                        }];
                }
            },
            'Errors': {
                defaultTimePeriod: '1hour',
                getGraphs: function (currentTimitSelectDataString) {
                    var iis = iisExpressionBuilder(currentTimitSelectDataString);
                    return [{
                        title: 'Errors',
                        expressions: [{
                            id: 'all-errors',
                            title: 'All Errors',
                            color: colors[1],
                            expression: 'sum(no_type)&' + currentTimitSelectDataString
                        }],
                        chartOptions: $.extend({}, chartOptions, { })
                    },{
                        title: 'User Journey (pre-booking form) Errors',
                        'class': 'half',
                        expressions: [{
                            id: 'search-errors',
                            title: 'Search',
                            color: searchColor,
                            expression: 'sum(no_type.re(Url,"Search|(H|h)otels"))&' + currentTimitSelectDataString
                        }, {
                            id: 'hotel-details-errors',
                            title: 'Hotel Details',
                            color: hotelColor,
                            expression: 'sum(no_type.re(Url,"hotel-reservations"))&' + currentTimitSelectDataString
                        }],
                        chartOptions: {}
                    },
                        {
                            title: 'Booking Errors',
                            'class': 'half',
                            expressions: [{
                                id: 'booking-errors',
                                title: 'Booking',
                                color: colors[0],
                                expression: 'sum(no_type.re(Url,"(Booking/Online|HotelReservationsSubmit/Submit|Booking/Submit)"))&' + currentTimitSelectDataString
                            }],
                            chartOptions: {}
                        }];
                }
            }
        };

        return {
            toString: function () {
                return 'Overview';
            },
            appendViews: function (allViews) {
                return $.extend(allViews, {
                    'Overview': {}
                });
            },
            appendViewModel: function (viewModel) {
                viewModel.dashboardViews[viewModel.dashboardViews.length] = {
                    name: 'Overview',
                    metric: 'Overview',
                    isSelected: isSelected
                };
                
                if (isSelected) {
                    viewModel.timePeriod = currentTimePeriod;
                    viewModel.pageName = 'Overview';

                    for (var subMetric in subMetrics) {
                        if (!subMetrics.hasOwnProperty(subMetric)) {
                            continue;
                        }
                        
                        viewModel.subMetrics[viewModel.subMetrics.length] = {
                            name: subMetric,
                            metric: 'Overview',
                            subMetric: subMetric,
                            isSelected: currentSubMetric === subMetric
                        };
                    }
                }
            },
            supportsView: function (view) {
                return view === 'Overview';
            },
            setView: function (view, subMetric) {
                isSelected = true;
                currentSubMetric = subMetric || 'Summary';
                currentTimePeriod = subMetrics[currentSubMetric].defaultTimePeriod;
            },
            clearView: function () {
                isSelected = false;
                currentSubMetric = '';
            },
            setTimePeriod: function (timePeriod) {
                currentTimePeriod = timePeriod;
            },
            getGraphs: function () {
                var currentTimitSelectDataString = TLRGRP.BADGER.Cube.convertTimePeriod(currentTimePeriod);
                var graphs = subMetrics[currentSubMetric].getGraphs(currentTimitSelectDataString);

                return graphs;
            }
        };
    };
})();