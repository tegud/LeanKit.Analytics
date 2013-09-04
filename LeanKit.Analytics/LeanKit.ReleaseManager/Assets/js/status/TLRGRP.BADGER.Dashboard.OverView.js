(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    var colors = ['steelblue', 'red', 'orange', 'green', 'purple'];
    var searchColor = colors[2];
    var hotelColor = colors[4];

    var presetColors = {
        homepage: colors[0],
        search: colors[2],
        bookingpage: colors[3],
        hoteldetails: colors[4],
        mobile: colors[2],
        direct: colors[0],
        affiliate: colors[4]
    };

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

        function getColor(expressionKey, i) {
            if (expressionKey.color) {
                return expressionKey.color;
            }

            var lowerCaseId = expressionKey.id.toLowerCase();

            for (var preset in presetColors) {
                if (lowerCaseId.indexOf(preset) > -1) {
                    return presetColors[preset];
                }
            }

            return colors[i];
        }

        function buildGraph(options) {
            var chartOptions = {};

            return {
                title: options.title,
                expressions: _.map(options.expressions, function (expressionKey, i) {
                    if (typeof expressionKey !== 'object') {
                        expressionKey = {
                            id: expressionKey
                        };
                    }
                    var expression = TLRGRP.BADGER[options.source].metricInfo(expressionKey.id);

                    $.extend(chartOptions, expression.chartOptions);

                    return {
                        title: expression.title,
                        expression: expression.expression,
                        color: getColor(expressionKey, i)
                    };
                }),
                chartOptions: $.extend(chartOptions, options.chartOptions)
            };
        }

        var graphs = {
            'TrafficByType': buildGraph({
                source: 'IIS',
                title: 'Traffic by Type',
                expressions: ['AllTraffic', 'BotTraffic', 'MobileTraffic'],
                chartOptions: {
                    dimensions: {
                        margin: { left: 50 }
                    }
                }
            }),
            'ResponseTimeByPage': buildGraph({
                source: 'IIS',
                title: 'Response Time by Page',
                expressions: ['HomePageServerResponseTime', 'SearchServerResponseTime', 'HotelDetailsServerResponseTime', 'BookingFormServerResponseTime'],
                chartOptions: {
                    dimensions: {
                        margin: { left: 50 }
                    }
                }
            }),
            'StatusCodes': buildGraph({
                source: 'IIS',
                title: 'Status Codes (non 200)',
                expressions: [{ id: 'NotFoundResponse', color: colors[2] },
                { id: 'ErrorResponse', color: colors[1] },
                { id: 'RedirectResponse', color: colors[0] }],
                chartOptions: {
                    dimensions: {
                        margin: {
                            left: 50,
                            right: 100
                        }
                    }
                }
            }),
            'TrafficByChannel': buildGraph({
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
            }),
            'TrafficByPage': buildGraph({
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
            }),
            'IPGResponseTime': buildGraph({
                source: 'IIS',
                title: 'Average Time for Tokenisation',
                expressions: ['IPGResponseTime'],
                chartOptions: {
                    legend: false,
                    yAxisLabel: 'duration (ms)'
                }
            }),
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
                getGraphs: function () {
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
                getGraphs: function () {
                    return getGraphsFor('TrafficByPage',
                        { id: 'TrafficByType', slots: 2 },
                        { id: 'TrafficByChannel', slots: 2 });
                }
            },
            'Errors': {
                getGraphs: function () {
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
                getGraphs: function () {
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