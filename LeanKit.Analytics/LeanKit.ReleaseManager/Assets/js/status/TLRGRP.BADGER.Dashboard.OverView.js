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

        var defaultGraphBuilderOptions = {
            graphOptions: {}
        };

        function buildGraph(metrics, options) {
            var metricsLength;
            var x;
            var expressions = [];
            var name = 'Untitled';
            var allMetricChartOptions = [true, {}, chartOptions];

            options = $.extend({}, defaultGraphBuilderOptions, options);

            if (typeof metrics === 'string') {
                if (options.name) {
                    name = options.name;
                }

                metrics = [metrics];
            }

            metricsLength = metrics.length;

            for (x = 0; x < metricsLength; x++) {
                var wmiMetric = TLRGRP.BADGER.WMI.metricInfo(metrics[x]);

                if (name === 'Untitled') {
                    name = wmiMetric.name;
                }

                allMetricChartOptions[allMetricChartOptions.length] = wmiMetric.chartOptions;

                expressions[expressions.length] = {
                    id: metrics[x],
                    title: wmiMetric.name,
                    color: colors[x % colors.length],
                    expression: TLRGRP.BADGER.Cube.WMI.buildExpression(wmiMetric, currentSubMetricName, currentTimitSelectDataString)
                };
            }

            allMetricChartOptions[allMetricChartOptions.length] = options.graphOptions;

            return {
                title: options.name || name,
                'class': 'half',
                expressions: expressions,
                chartOptions: $.extend.apply(this, allMetricChartOptions)
            };
        }

        var subMetrics = {
            'Summary': {
                defaultTimePeriod: '1hour',
                getGraphs: function (currentTimitSelectDataString) {
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
                        title: 'Status Codes',
                        'class': 'half',
                        expressions: [{
                            id: 'status-ok',
                            title: '200 (Ok)',
                            color: colors[3],
                            expression: 'sum(lr_web_request.eq(status,200))&' + currentTimitSelectDataString
                        }, {
                            id: 'status-missing',
                            title: '404 (Not Found)',
                            color: colors[2],
                            expression: 'sum(lr_web_request.eq(status,404))&' + currentTimitSelectDataString
                        }, {
                            id: 'status-error',
                            title: '500 (Error)',
                            color: colors[1],
                            expression: 'sum(lr_web_request.eq(status,500))&' + currentTimitSelectDataString
                        }, {
                            id: 'status-redirect',
                            title: '30x (Redirect)',
                            color: colors[0],
                            expression: 'sum(lr_web_request.in(status,[301,302]))&' + currentTimitSelectDataString
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
                getGraphs: function(currentTimitSelectDataString) {
                    
                    return [
                        {
                            title: 'Traffic by Page',
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
                        }, {
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