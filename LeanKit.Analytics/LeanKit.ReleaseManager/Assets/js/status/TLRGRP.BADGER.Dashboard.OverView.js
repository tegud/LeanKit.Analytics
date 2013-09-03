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

        function iisExpressionBuilder() {
            return (function () {
                var currentTimitSelectDataString = TLRGRP.BADGER.Cube.convertTimePeriod(currentTimePeriod);
                return new TLRGRP.BADGER.Cube.ExpressionBuilder('lr_web_request').setTimePeriod(currentTimitSelectDataString);
            });
        }

        function errorExpressionBuilder() {
            return (function () {
                var currentTimitSelectDataString = TLRGRP.BADGER.Cube.convertTimePeriod(currentTimePeriod);
                return new TLRGRP.BADGER.Cube.ExpressionBuilder('no_type').setTimePeriod(currentTimitSelectDataString);
            });
        }

        var iis = iisExpressionBuilder();
        var errors = errorExpressionBuilder();
        var graphs = {
            'TrafficByType': {
                title: 'Traffic by Type',
                expressions: [{
                        title: 'All',
                        color: colors[0],
                        expression: iis().sum()
                    },
                    {
                        title: 'Bot',
                        color: colors[1],
                        expression: iis().sum().equalTo('isbot', true)
                    },
                    {
                        title: 'Mobile',
                        color: colors[4],
                        expression: iis().sum().equalTo('isbot', false).equalTo('ismobile', true)
                    }],
                chartOptions: {
                    yAxisLabel: 'requests',
                    dimensions: {
                        margin: { left: 50 }
                    }
                }
            },
            'AllErrors': {
                title: 'Errors',
                expressions: [{
                    title: 'All Errors',
                    color: colors[1],
                    expression: errors().sum()
                }],
                chartOptions: {
                    legend: false
                }
            },
            'ResponseTimeByPage': {
                title: 'Response Time by Page',
                expressions: [{
                        title: 'Home Page',
                        color: homePageColor,
                        expression: iis().median('duration').equalTo('pagetype', 'home-page')
                    },
                    {
                        title: 'Search',
                        color: searchColor,
                        expression: iis().median('duration').equalTo('pagetype', 'search')
                    },
                    {
                        title: 'Hotel Details',
                        color: hotelColor,
                        expression: iis().median('duration').equalTo('pagetype', 'hotel-details')
                    },
                    {
                        title: 'Booking Form',
                        color: bookingPageColor,
                        expression: iis().median('duration').equalTo('pagetype', 'booking-form')
                    }],
                chartOptions: {
                    yAxisLabel: 'request time (ms)',
                    dimensions: {
                        margin: { left: 50 }
                    }
                }
            },
            'StatusCodes': {
                title: 'Status Codes (non 200)',
                expressions: [{
                        title: '404 (Not Found)',
                        color: colors[2],
                        expression: iis().sum().equalTo('status', 404)
                    }, {
                        title: '500 (Error)',
                        color: colors[1],
                        expression: iis().sum().equalTo('status', 500)
                    }, {
                        title: '30x (Redirect)',
                        color: colors[0],
                        expression: iis().sum().in('status', [301, 302])
                    }],
                chartOptions: {
                    yAxisLabel: 'requests',
                    dimensions: {
                        margin: {
                            left: 50,
                            right: 100
                        }
                    }
                }
            },
            'TrafficByChannel': {
                title: 'Traffic by Channel',
                expressions: [{
                        title: 'Direct',
                        color: homePageColor,
                        expression: iis().sum().equalTo('pagechannel', 'web')
                    },
                    {
                        title: 'Mobile',
                        color: searchColor,
                        expression: iis().sum().equalTo('pagechannel', 'mobile')
                    },
                    {
                        title: 'Affiliate',
                        color: hotelColor,
                        expression: iis().sum().equalTo('pagechannel', 'affiliate')
                    }],
                chartOptions: {
                    yAxisLabel: 'requests',
                    dimensions: {
                        margin: { left: 50 }
                    }
                }
            },
            'TrafficByPage': {
                title: 'Traffic by Page',
                expressions: [{
                        title: 'Home Page',
                        color: homePageColor,
                        expression: iis().sum().equalTo('pagetype', 'home-page')
                    },
                    {
                        title: 'Search',
                        color: searchColor,
                        expression: iis().sum().equalTo('pagetype', 'search')
                    },
                    {
                        title: 'Hotel Details',
                        color: hotelColor,
                        expression: iis().sum().equalTo('pagetype', 'hotel-details')
                    },
                    {
                        title: 'Booking Form',
                        color: bookingPageColor,
                        expression: iis().sum().equalTo('pagetype', 'booking-form')
                    }],
                chartOptions: {
                    yAxisLabel: 'requests',
                    dimensions: {
                        margin: { left: 50 }
                    }
                }
            },
            'UserJourneyErrors': {
                title: 'User Journey (pre-booking form) Errors',
                expressions: [{
                        title: 'Search',
                        color: searchColor,
                        expression: errors().sum().matchesRegEx('Url', 'Search|(H|h)otels')
                    }, {
                        title: 'Hotel Details',
                        color: hotelColor,
                        expression: errors().sum().matchesRegEx('Url', 'hotel-reservations')
                    }]
            },
            'BookingErrors': {
                title: 'Booking Errors',
                expressions: [{
                    title: 'Booking',
                    color: colors[0],
                    expression: errors().sum().matchesRegEx('Url', '(BookingError/LogError\.mvc|Booking/Online|HotelReservationsSubmit/Submit|Booking/Submit)')
                }],
                chartOptions: {
                    legend: false,
                    dimensions: {
                        margin: { right: 20 }
                    }
                }
            },
            'IPGErrors': {
                title: 'IPG Errors',
                expressions: [{
                        title: 'Request Timeout',
                        color: colors[0],
                        expression: errors().sum()
                            .matchesRegEx('Url', '(BookingError/LogError\.mvc)')
                            .matchesRegEx('Exception.Message', 'request_timeout')
                }, {
                    title: 'Session Timeout',
                    color: colors[2],
                    expression: errors().sum()
                        .matchesRegEx('Url', '(BookingError/LogError\.mvc)')
                        .matchesRegEx('Exception.Message', 'session_timeout')
                }, {
                    title: 'Invalid Session',
                    color: colors[4],
                    expression: errors().sum()
                        .matchesRegEx('Url', '(BookingError/LogError\.mvc)')
                        .matchesRegEx('Exception.Message', 'invalid_session')
                }],
                chartOptions: {
                    dimensions: {
                        margin: { right: 110 }
                    }
                }
            },
            'IPGResponseTime': {
                title: 'Average Time for Tokenisation',
                expressions: [{
                    color: colors[0],
                    expression: iis().sum().matchesRegEx('url', '/beacon/pageresponse')
                }],
                chartOptions: {
                    legend: false,
                    yAxisLabel: 'duration (ms)'
                }
            }
        };

        function getGraphFor(graph) {
            var graphClass;
            var instanceChartOptions = graph.chartOptions;

            if (typeof graph === 'object') {
                if (graph.slots === 2) {
                    graphClass = 'half';
                }
                graph = graph.id;
            }

            var selectedGraph = graphs[graph];
            var currentTimitSelectDataString = TLRGRP.BADGER.Cube.convertTimePeriod(currentTimePeriod);

            return $.extend(true, {}, selectedGraph, {
                'class': graphClass,
                expressions: _.map(selectedGraph.expressions, function (expression) {
                    expression.expression = expression.expression.setTimePeriod(currentTimitSelectDataString).build();

                    if (!expression.id) {
                        var autoTitle = (selectedGraph.title ? selectedGraph.title + '-' : '') + expression.title;
                        expression.id = autoTitle.toLowerCase().replace(/\s/g, '-').replace(/[()]/g, '');
                    }

                    return expression;
                }),
                chartOptions: $.extend({}, chartOptions, selectedGraph.chartOptions, instanceChartOptions)
            });
        }

        function getGraphsFor() {
            return _.map(arguments, function (graphItem) {
                return getGraphFor(graphItem);
            });
        }

        var subMetrics = {
            'Summary': {
                getGraphs: function() {
                    return getGraphsFor({ id: 'TrafficByType', slots: 2 },
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
                        { id: 'StatusCodes', slots: 2 });
                }
            },
            'Traffic': {
                getGraphs: function() {
                    return getGraphsFor('TrafficByPage',
                        { id: 'TrafficByType', slots: 2 },
                        { id: 'TrafficByChannel', slots: 2 });
                }
            },
            'Errors': {
                getGraphs: function() {
                    return getGraphsFor({
                            id: 'AllErrors',
                            chartOptions: {
                                dimensions: {
                                    margin: { right: 20 }
                                }
                            }
                        },
                        { id: 'UserJourneyErrors', slots: 2 },
                        { id: 'BookingErrors', slots: 2 });
                }
            },
            'IPG': {
                defaultTimePeriod: '4hours',
                getGraphs: function() {
                    return getGraphsFor('IPGErrors', 'IPGResponseTime');
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
                if (subMetrics[currentSubMetric].defaultTimePeriod) {
                    currentTimePeriod = subMetrics[currentSubMetric].defaultTimePeriod;
                }
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