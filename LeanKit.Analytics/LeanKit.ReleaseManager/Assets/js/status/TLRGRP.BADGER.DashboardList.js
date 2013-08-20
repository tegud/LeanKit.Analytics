(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.DashboardList = function (request, currentSubmetric, views) {
        var currentMetric = request.dashboard();
        var metricsList = $('#metrics-list');
        var subMetricList = $('#metric-drilldown-list');

        for (var m in views) {
            if (!views.hasOwnProperty(m)) {
                continue;
            }

            var name = views[m].name || m;

            if (currentMetric === m) {
                metricsList.append('<li class="selected-metric">' + name + '</li>');
            } else {
                metricsList.append('<li><a href="/Status?metric=' + m + '">' + name + '</a></li>');
            }
        }

        if (views[currentMetric] && views[currentMetric].subMetrics) {
            for (var sm in views[currentMetric].subMetrics) {
                if (!views[currentMetric].subMetrics.hasOwnProperty(sm)) {
                    continue;
                }

                var name = views[currentMetric].subMetrics[sm].name || sm;

                if (currentSubmetric === sm) {
                    subMetricList.append('<li class="selected-metric">' + name + '</li>');
                } else {
                    subMetricList.append('<li><a href="/Status?metric=' + currentMetric + '&subMetric=' + sm + '">' + name + '</a></li>');
                }
            }
        }

        $('#metric-title').text(currentMetric);
    };
})();