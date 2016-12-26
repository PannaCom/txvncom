﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ThueXeVn
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "view detail news",
                "tin/{name}-{id}",
                new { controller = "news", action = "GetDetails", name = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "view news",
                "tin/page={page}",
                new { controller = "news", action = "List", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ViewXetaxinoibai",
                "taxi-noi-bai-gia-re-taxi-san-bay-gia-re",
                new { controller = "Home", action = "XeTaxiNoiBai" }
            );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
