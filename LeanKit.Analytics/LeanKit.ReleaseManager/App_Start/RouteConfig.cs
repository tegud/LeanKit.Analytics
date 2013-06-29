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
                name: "ControllerWithId",
                url: "{controller}/{id}",
                defaults: new { controller = "Home", action = "Index" },
                constraints: new { id = "[0-9]{1,9}" });

            routes.MapRoute(
                name: "ControllerActionWithId",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { id = "[0-9]{1,9}" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" });
        }
    }
}