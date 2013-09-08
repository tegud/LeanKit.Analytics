(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    TLRGRP.BADGER.Dashboard.GraphSet = function (configuration) {
        var isSelected;
        var currentTimePeriod = '1hour';
        var currentSubMetric;
        var metric = configuration.metric;
        var name = configuration.name || configuration.metric;
        var subMetrics = configuration.subMetrics;

        return {
            toString: function () {
                return metric;
            },
            appendViewModel: function (viewModel) {
                viewModel.dashboardViews[viewModel.dashboardViews.length] = {
                    name: name,
                    metric: metric,
                    isSelected: isSelected
                };

                if (isSelected) {
                    viewModel.timePeriod = currentTimePeriod;
                    viewModel.pageName = name;

                    for (var subMetric in subMetrics) {
                        if (!subMetrics.hasOwnProperty(subMetric)) {
                            continue;
                        }

                        viewModel.subMetrics[viewModel.subMetrics.length] = {
                            name: subMetric,
                            metric: metric,
                            subMetric: subMetric,
                            isSelected: currentSubMetric === subMetric
                        };
                    }
                }
            },
            supportsView: function (view) {
                return view === metric;
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
                return subMetrics[currentSubMetric].getGraphs(currentTimePeriod);
            }
        };
    };

    TLRGRP.BADGER.Dashboard.Factory = (function () {
        return {
            build: function (config) {
                return new TLRGRP.BADGER.Dashboard.GraphSet(config);
            }
        };
    })();

    TLRGRP.BADGER.Dashboard.Overview = function () {
        return new TLRGRP.BADGER.Dashboard.GraphSet({
            metric: 'Overview',
            subMetrics: {
                'Summary': {
                    getGraphs: function(currentTimePeriod) {
                        var graphFactory = TLRGRP.BADGER.Dashboard.GraphFactory(currentTimePeriod);
                        return graphFactory.getGraphsFor({ id: 'TrafficByType', slots: 2 },
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
                    getGraphs: function(currentTimePeriod) {
                        var graphFactory = TLRGRP.BADGER.Dashboard.GraphFactory(currentTimePeriod);
                        return graphFactory.getGraphsFor('TrafficByPage',
                            { id: 'TrafficByType', slots: 2 },
                            { id: 'TrafficByChannel', slots: 2 });
                    }
                },
                'Errors': {
                    getGraphs: function(currentTimePeriod) {
                        var graphFactory = TLRGRP.BADGER.Dashboard.GraphFactory(currentTimePeriod);
                        return graphFactory.getGraphsFor({
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
                    getGraphs: function(currentTimePeriod) {
                        var graphFactory = TLRGRP.BADGER.Dashboard.GraphFactory(currentTimePeriod);
                        return graphFactory.getGraphsFor('IPGErrors', 'IPGResponseTime');
                    }
                }
            }
        });
    };
})();