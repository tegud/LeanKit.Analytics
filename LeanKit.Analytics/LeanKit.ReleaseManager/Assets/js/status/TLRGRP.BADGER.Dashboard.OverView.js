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
        var subMetrics = {
            'Traffic': {
                defaultTimePeriod: '1hour',
                getGraphs: function(currentTimitSelectDataString) {
                    
                    return [{
                            title: 'Traffic by Type',
                            'class': 'half',
                            expressions: [{
                                    id: 'iis-all',
                                    title: 'All',
                                    color: colors[0],
                                    expression: 'sum(lr_web_request)&' + currentTimitSelectDataString
                                },
                                {
                                    id: 'iis-bot',
                                    title: 'Bot',
                                    color: colors[1],
                                    expression: 'sum(lr_web_request.eq(isbot,true))&' + currentTimitSelectDataString
                                },
                                {
                                    id: 'iis-mobile',
                                    title: 'Mobile',
                                    color: colors[4],
                                    expression: 'sum(lr_web_request.eq(isbot,false).eq(ismobile,true))&' + currentTimitSelectDataString
                                }],
                            chartOptions: $.extend({}, chartOptions, {
                                yAxisLabel: 'requests',
                                dimensions: {
                                    margin: { left: 50 }
                                }
                            })
                        },
                        {
                            title: 'Traffic by Page',
                            'class': 'half',
                            expressions: [{
                                    id: 'page-requests-home-page',
                                    title: 'Home Page',
                                    color: homePageColor,
                                    expression: 'sum(lr_web_request.eq(pagetype,"home-page"))&' + currentTimitSelectDataString
                                },
                                {
                                    id: 'page-requests-search',
                                    title: 'Search',
                                    color: searchColor,
                                    expression: 'sum(lr_web_request.eq(pagetype,"search"))&' + currentTimitSelectDataString
                                },
                                {
                                    id: 'page-requests-hotel-details',
                                    title: 'Hotel Details',
                                    color: hotelColor,
                                    expression: 'sum(lr_web_request.eq(pagetype,"hotel-details"))&' + currentTimitSelectDataString
                                },
                                {
                                    id: 'page-requests-booking-form',
                                    title: 'Booking Form',
                                    color: bookingPageColor,
                                    expression: 'sum(lr_web_request.eq(pagetype,"booking-form"))&' + currentTimitSelectDataString
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
                        }, {
                            title: 'Response Time by Page',
                            'class': 'half',
                            expressions: [{
                                    id: 'page-requests-home-page',
                                    title: 'Home Page',
                                    color: homePageColor,
                                    expression: 'median(lr_web_request(duration).eq(pagetype,"home-page"))&' + currentTimitSelectDataString
                                },
                                {
                                    id: 'page-requests-search',
                                    title: 'Search',
                                    color: searchColor,
                                    expression: 'median(lr_web_request(duration).eq(pagetype,"search"))&' + currentTimitSelectDataString
                                },
                                {
                                    id: 'page-requests-hotel-details',
                                    title: 'Hotel Details',
                                    color: hotelColor,
                                    expression: 'median(lr_web_request(duration).eq(pagetype,"hotel-details"))&' + currentTimitSelectDataString
                                },
                                {
                                    id: 'page-requests-booking-form',
                                    title: 'Booking Form',
                                    color: bookingPageColor,
                                    expression: 'median(lr_web_request(duration).eq(pagetype,"booking-form"))&' + currentTimitSelectDataString
                                }],
                            chartOptions: $.extend({}, chartOptions, {
                                yAxisLabel: 'request time (ms)',
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
                        }];
                }
            },
            'Errors': {
                defaultTimePeriod: '1hour',
                getGraphs: function (currentTimitSelectDataString) {
                    return [{
                        title: 'Errors',
                        expressions: [{
                            id: 'all-errors',
                            title: 'All Errors',
                            color: colors[1],
                            expression: 'sum(no_type)&' + currentTimitSelectDataString
                        },{
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
                        chartOptions: $.extend({}, chartOptions, { })
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
                currentSubMetric = subMetric || 'Traffic';
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