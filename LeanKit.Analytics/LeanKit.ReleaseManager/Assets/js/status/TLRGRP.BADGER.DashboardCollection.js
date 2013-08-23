(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.DashboardCollection = function (dashboards, defaultView) {
        var x;
        var dashboardsLength = dashboards.length;
        var dashboardLookup = {};
        var currentDashboard;
        var currentTimePeriod;
        var request = new TLRGRP.BADGER.MetricsRequest();

        function buildViewModel() {
            var viewModel = {
                dashboardViews: [],
                subMetrics: []
            };
                
            for (x = 0; x < dashboardsLength; x++) {
                dashboards[x].appendViewModel(viewModel);
            }

            return viewModel;
        }

        function setView(view, subMetric) {
            currentDashboard = getDashboardByView(view);
            currentDashboard.setView(view, subMetric);
            for (x = 0; x < dashboardsLength; x++) {
                if (currentDashboard.toString() !== dashboards[x].toString()) {
                    dashboards[x].clearView('', '');   
                }
            }
        }
        
        function setTimePeriod(timePeriod) {
            currentTimePeriod = timePeriod;
            currentDashboard.setTimePeriod(timePeriod);
        }
        
        function getDashboardByView(view) {
            var matchedDashboard;

            for (x = 0; x < dashboardsLength; x++) {
                if (dashboards[x].supportsView(view)) {
                    matchedDashboard = dashboards[x];
                    break;
                }
            }

            return matchedDashboard;
        }

        for (x = 0; x < dashboardsLength; x++) {
            dashboardLookup[dashboards[x].toString()] = dashboards[x];
        }

        
        if (request.dashboard()) {
            setView(request.dashboard(), request.subMetric());
        }
        else {
            setView(defaultView);
        }
                
        if (request.timePeriod()) {
            setTimePeriod(request.timePeriod());
        }
        
        return {
            getDashboardByView: getDashboardByView,
            setUpUi: function () {
                var viewModel = buildViewModel();
                var graphs = currentDashboard.getGraphs();
                
                new TLRGRP.BADGER.DashboardList(viewModel);

                new TLRGRP.BADGER.TimeSelect($('#query-time-select'), viewModel.timePeriod);
                
                new TLRGRP.BADGER.StopStart($('#stop-start'));

                $('#metric-title').text(viewModel.pageName);
                
                new TLRGRP.dashboards.Builder($('#graphs'), graphs);
            }
        };
    };
})();