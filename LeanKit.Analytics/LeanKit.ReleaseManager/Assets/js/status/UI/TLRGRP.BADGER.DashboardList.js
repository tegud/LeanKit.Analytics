(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.DashboardList = function (viewModel) {
        var dashboardViewsLength = viewModel.dashboardViews.length;
        var subMetricsLength = viewModel.subMetrics.length;
        var x;
        var metricsList = $('#metrics-list');
        var subMetricList = $('#metric-drilldown-list');

        for (x = 0; x < dashboardViewsLength; x++) {
            var name = viewModel.dashboardViews[x].name;
            var metric = viewModel.dashboardViews[x].metric;

            if (viewModel.dashboardViews[x].isSelected) {
                metricsList.append('<li class="selected-metric">' + name + '</li>');
            } else {
                metricsList.append('<li><a href="/Status?metric=' + metric + '">' + name + '</a></li>');
            }
        }
        
        for (x = 0; x < subMetricsLength; x++) {
            var name = viewModel.subMetrics[x].name;
            var metric = viewModel.subMetrics[x].metric;
            var subMetric = viewModel.subMetrics[x].subMetric;

            if (viewModel.subMetrics[x].isSelected) {
                subMetricList.append('<li class="selected-metric">' + name + '</li>');
            } else {
                subMetricList.append('<li><a href="/Status?metric=' + metric + '&subMetric=' + subMetric + '">' + name + '</a></li>');
            }
        }
    };
})();