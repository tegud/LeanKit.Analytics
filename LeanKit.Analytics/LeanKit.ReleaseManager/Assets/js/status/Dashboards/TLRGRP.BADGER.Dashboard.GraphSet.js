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
            getComponents: function () {
                var subMetricConfiguration = subMetrics[currentSubMetric];
                var graphFactory = TLRGRP.BADGER.Dashboard.GraphFactory(currentTimePeriod);

                return graphFactory.getGraphsFor.apply(this, subMetricConfiguration);
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
})();