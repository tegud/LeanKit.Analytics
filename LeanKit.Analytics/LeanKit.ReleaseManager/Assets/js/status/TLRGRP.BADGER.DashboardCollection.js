(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.DashboardCollection = function (dashboards, defaultView) {
        var x;
        var dashboardsLength = dashboards.length;
        var dashboardLookup = {};
        var currentDashboard;
        var currentTimePeriod;
        

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

        setView(defaultView);

        return {
            getDashboardByView: getDashboardByView,
            setCurrent: function(request) {
                if (request.dashboard()) {
                    setView(request.dashboard(), request.subMetric());
                }
                
                if (request.timePeriod()) {
                    setTimePeriod(request.timePeriod());
                }
            },
            setUpUi: function () {
                var viewModel = buildViewModel();
                var graphs = currentDashboard.getGraphs();
                
                new TLRGRP.BADGER.DashboardList(viewModel);

                new TLRGRP.BADGER.TimeSelect($('#query-time-select'));
                
                new TLRGRP.BADGER.StopStart($('#stop-start'));

                $('#metric-title').text(viewModel.pageName);
                
                new TLRGRP.dashboards.Builder($('#graphs'), graphs);
            }
        };
    };
})();