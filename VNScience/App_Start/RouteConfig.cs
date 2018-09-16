using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VNScience
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DetailPost",
                url: "tin-tuc/{metatitle}-{id}",
                defaults: new { controller = "Post", action = "Detail" },
                namespaces: new[] { "VNScience.Controllers" }
            );

            routes.MapRoute(
                name: "PostCategory",
                url: "danh-muc-tin/{metatitle}-{categoryId}",
                defaults: new { controller = "Post", action = "Index"},
                namespaces: new[] { "VNScience.Controllers" }
            );

            routes.MapRoute(
                name: "SearchPosts",
                url: "tim-kiem/",
                defaults: new { controller = "Post", action = "Search" },
                namespaces: new[] { "VNScience.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "VNScience.Controllers" }
            );
        }
    }
}
