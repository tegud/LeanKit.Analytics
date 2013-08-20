(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.DashboardCollection = function (dashboards, defaultView) {
        var x;
        var dashboardsLength = dashboards.length;
        var dashboardLookup = {};
        var currentDashboard;

        function setView(view, subMetric) {
            currentDashboard = getDashboardByView(view);
            currentDashboard.setView(view, subMetric);
        }
        
        function setTimePeriod(step, limit) {
            currentDashboard.setTimePeriod(step, limit);
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
            getDashboard: function (name) {
                return dashboardLookup[name];
            },
            allViews: function () {
                var allViews = {};
                
                for (x = 0; x < dashboardsLength; x++) {
                    dashboards[x].appendViews(allViews);
                }

                return allViews;
            },
            getDashboardByView: getDashboardByView,
            currentView: function () {
                return currentDashboard;
            },
            setCurrent: function(request) {
                if (request.dashboard()) {
                    setView(request.dashboard(), request.subMetric());
                }
                
                if (request.step() && request.limit()) {
                    setTimePeriod(request.step(), request.limit());
                }
            }
        };
    };
})();