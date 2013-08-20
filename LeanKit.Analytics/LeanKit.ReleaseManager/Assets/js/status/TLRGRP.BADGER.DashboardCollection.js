(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.DashboardCollection = function (dashboards, defaultDashboard) {
        var x;
        var dashboardsLength = dashboards.length;
        var dashboardLookup = {};

        for (x = 0; x < dashboardsLength; x++) {
            dashboardLookup[dashboards[x].toString()] = dashboards[x];
        }

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
            getDashboardByView: function (view) {
                var matchedDashboard;

                for (x = 0; x < dashboardsLength; x++) {
                    if (dashboards[x].supportsView(view)) {
                        matchedDashboard = dashboards[x];
                        break;
                    }
                }

                return matchedDashboard;
            }
        };
    };
})();