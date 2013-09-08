using System.Web.Optimization;

namespace LeanKit.ReleaseManager.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var dashboardsBundle = BuildDashboardsBundle();

            bundles.Add(dashboardsBundle);
        }

        private static ScriptBundle BuildDashboardsBundle()
        {
            var dashboardsBundle = new ScriptBundle("~/assets/js/bundles/dashboards");

            dashboardsBundle.Include("~/assets/js/lib/jquery-2.0.0.min.js",
                                     "~/assets/js/lib/jquery-ui.js",
                                     "~/assets/js/lib/lodash.js",
                                     "~/assets/js/lib/moment.min.js",
                                     "~/assets/js/lib/TLRGRP.core.js",
                                     "~/assets/js/lib/moment.min.js",
                                     "~/assets/js/lib/nanomachine.js",
                                     "~/assets/js/lib/TLRGRP.BADGER.utilities.js");

            dashboardsBundle.Include("~/assets/js/status-charts/TLRGRP.dashboards.Builder.js");

            dashboardsBundle.Include("~/assets/js/status/UI/TLRGRP.BADGER.ColorPalette.js",
                                     "~/assets/js/status/UI/TLRGRP.BADGER.DashboardList.js",
                                     "~/assets/js/status/UI/TLRGRP.BADGER.TimeSelect.js");

            dashboardsBundle.Include("~/assets/js/status/Sources/TLRGRP.BADGER.Cube.js");

            dashboardsBundle.Include("~/assets/js/status/TLRGRP.BADGER.Machines.js",
                                     "~/assets/js/status/TLRGRP.BADGER.Pages.js",
                                     "~/assets/js/status/LateRooms/TLRGRP.BADGER.LateRooms.Machines.js",
                                     "~/assets/js/status/LateRooms/TLRGRP.BADGER.LateRooms.Pages.js");

            dashboardsBundle.Include("~/assets/js/status/Metrics/TLRGRP.BADGER.IIS.js",
                                     "~/assets/js/status/Metrics/TLRGRP.BADGER.WMI.js",
                                     "~/assets/js/status/Metrics/TLRGRP.BADGER.Errors.js");

            dashboardsBundle.Include("~/assets/js/status/TLRGRP.BADGER.Dashboard.GraphFactory.js",
                                     "~/assets/js/status/Graphs/TLRGRP.BADGER.Dashboard.Graphs.js",
                                     "~/assets/js/status/Graphs/TLRGRP.BADGER.Dashboard.Graphs.IIS.js",
                                     "~/assets/js/status/Graphs/TLRGRP.BADGER.Dashboard.Graphs.Errors.js");

            dashboardsBundle.Include("~/assets/js/status/TLRGRP.BADGER.DashboardCollection.js");

            dashboardsBundle.Include("~/assets/js/status/Dashboards/TLRGRP.BADGER.Dashboard.WebHosts.js",
                                     "~/assets/js/status/Dashboards/TLRGRP.BADGER.Dashboard.ByHost.js",
                                     "~/assets/js/status/Dashboards/TLRGRP.BADGER.Dashboard.Overview.js",
                                     "~/assets/js/status/Dashboards/TLRGRP.BADGER.Dashboard.Mobile.js",
                                     "~/assets/js/status/Dashboards/TLRGRP.BADGER.Dashboard.ByPage.js");

            dashboardsBundle.Include("~/assets/js/status/TLRGRP.BADGER.StopStart.js",
                                     "~/assets/js/status/TLRGRP.BADGER.MetricRequest.js");
            return dashboardsBundle;
        }
    }
}