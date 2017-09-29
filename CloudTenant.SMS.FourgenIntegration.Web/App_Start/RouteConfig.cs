using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

 //           RouteTable.Routes.MapHttpRoute(
 //     name: "DefaultApi",
 //     routeTemplate: "{controller}/{id}",
 //     defaults: new { id = System.Web.Http.RouteParameter.Optional }
 //);
        }
    }
}
