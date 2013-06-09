using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LeanKit.ReleaseManager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Release",
                url: "Release/{action}/{id}",
                defaults: new { controller = "Release" },
                constraints:new { id = "[0-9]{1,9}" }
            );

            routes.MapRoute(
                name: "ReleaseDetail",
                url: "Release/{id}",
                defaults: new { controller = "Release", action = "Index" },
                constraints: new { id = "[0-9]{1,9}" }
            );

            routes.MapRoute(
                name: "NewRelease",
                url: "NewRelease",
                defaults: new { controller = "NewRelease", action = "Index" }
            );

            routes.MapRoute(
                name: "Releases",
                url: "Releases",
                defaults: new { controller = "Releases", action = "Index" }
            );

            routes.MapRoute(
                name: "UpcomingReleases",
                url: "UpcomingReleases",
                defaults: new { controller = "UpcomingReleases", action = "Index" }
            );

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}