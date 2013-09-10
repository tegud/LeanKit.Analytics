(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    TLRGRP.BADGER.Dashboard.ByPage = function () {
        var isSelected;
        var currentTimePeriod = '1hour';
        var currentSubMetric;

        function getTrafficByGraph() {
            var trafficTypeGraph = {
                id: 'TrafficByType',
                additionalExpressionFilters: [{
                    filter: 'equalTo',
                    key: 'pagetype',
                    value: TLRGRP.BADGER.Pages.get(currentSubMetric).pagetype
                }],
                chartOptions: {
                    dimensions: { margin: { right: 58 } }
                }
            };

            return trafficTypeGraph;
        }

        var subMetrics = {
            'HomePage': {
                name: 'Home Page',
                pageType: 'home-page',
                getGraphs: function () {
                    var graphFactory = TLRGRP.BADGER.Dashboard.GraphFactory(currentTimePeriod);

                    return graphFactory.getGraphsFor(getTrafficByGraph(),
                        {
                            id: 'ResponseTimeByPage',
                            title: 'Response Time',
                            expressions: ['HomePageServerResponseTime'],
                            chartOptions: {
                                legend: false,
                                dimensions: { margin: { right: 20 } }
                            }
                        });
                }
            },
            'Search': {
                pageType: 'search',
                getGraphs: function () {
                    var graphFactory = TLRGRP.BADGER.Dashboard.GraphFactory(currentTimePeriod);
                    return graphFactory.getGraphsFor(getTrafficByGraph(),
                        {
                            id: 'ResponseTimeByPage',
                            title: 'Response Time',
                            expressions: ['SearchServerResponseTime'],
                            slots: 2,
                            chartOptions: {
                                legend: false,
                                dimensions: { margin: { right: 20 } }
                            }
                        },
                        {
                            id: 'AllErrors',
                            additionalExpressionFilters: [{
                                filter: 'matchesRegEx',
                                key: 'Url',
                                value: TLRGRP.BADGER.Pages.get(currentSubMetric).regex
                            }],
                            slots: 2,
                            chartOptions: {
                                dimensions: { margin: { right: 20 } }
                            }
                        });
                }
            },
            'HotelDetails': {
                name: 'Hotel Details',
                pageType: 'hotel-details',
                getGraphs: function () {
                    var graphFactory = TLRGRP.BADGER.Dashboard.GraphFactory(currentTimePeriod);
                    return graphFactory.getGraphsFor(getTrafficByGraph(),
                        {
                            id: 'ResponseTimeByPage',
                            title: 'Response Time',
                            expressions: ['HotelDetailsServerResponseTime'],
                            slots: 2,
                            chartOptions: {
                                legend: false,
                                dimensions: { margin: { right: 20 } }
                            }
                        },
                        {
                            id: 'AllErrors',
                            additionalExpressionFilters: [{
                                filter: 'matchesRegEx',
                                key: 'Url',
                                value: TLRGRP.BADGER.Pages.get(currentSubMetric).regex
                            }],
                            slots: 2,
                            chartOptions: {
                                dimensions: { margin: { right: 20 } }
                            }
                        });
                }
            },
            'BookingForm': {
                name: 'Booking Form',
                pageType: 'booking-form',
                getGraphs: function () {
                    var graphFactory = TLRGRP.BADGER.Dashboard.GraphFactory(currentTimePeriod);
                    return graphFactory.getGraphsFor(getTrafficByGraph(),
                        {
                            id: 'ResponseTimeByPage',
                            title: 'Response Time',
                            expressions: ['BookingFormServerResponseTime'],
                            slots: 2,
                            chartOptions: {
                                legend: false,
                                dimensions: { margin: { right: 20 } }
                            }
                        },
                        {
                            id: 'AllErrors',
                            additionalExpressionFilters: [{
                                filter: 'matchesRegEx',
                                key: 'Url',
                                value: TLRGRP.BADGER.Pages.get(currentSubMetric).regex
                            }],
                            slots: 2,
                            chartOptions: {
                                dimensions: { margin: { right: 20 } }
                            }
                        });
                }
            }
        };

        return {
            toString: function () {
                return 'ByPage';
            },
            appendViews: function (allViews) {
                return $.extend(allViews, {
                    'ByPage': {}
                });
            },
            appendViewModel: function (viewModel) {
                viewModel.dashboardViews[viewModel.dashboardViews.length] = {
                    name: 'By Page',
                    metric: 'ByPage',
                    isSelected: isSelected
                };

                if (isSelected) {
                    viewModel.timePeriod = currentTimePeriod;
                    viewModel.pageName = 'By Page';

                    for (var subMetric in subMetrics) {
                        if (!subMetrics.hasOwnProperty(subMetric)) {
                            continue;
                        }

                        viewModel.subMetrics[viewModel.subMetrics.length] = {
                            name: subMetrics[subMetric].name || subMetric,
                            metric: 'ByPage',
                            subMetric: subMetric,
                            isSelected: currentSubMetric === subMetric
                        };
                    }
                }
            },
            supportsView: function (view) {
                return view === 'ByPage';
            },
            setView: function (view, subMetric) {
                isSelected = true;
                currentSubMetric = subMetric || 'HomePage';
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
            getComponents: function () {
                var currentTimitSelectDataString = TLRGRP.BADGER.Cube.convertTimePeriod(currentTimePeriod);

                return subMetrics[currentSubMetric].getGraphs(currentTimitSelectDataString);
            }
        };
    };
})();