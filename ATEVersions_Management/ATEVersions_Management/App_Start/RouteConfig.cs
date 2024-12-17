using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ATEVersions_Management
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // Enabling Attribute Routing
            //routes.MapMvcAttributeRoutes();

            //
            #region Custom Global Routes
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            /*routes.MapRoute(
                name: "Default",
                url: "{area}/{controller}/{action}/{id}",
                defaults: new { area = "Admin", controller = "Dashboard", action = "AdminIndex", id = UrlParameter.Optional }
            );*/

            routes.MapRoute(
                name: "ATETime",
                url: "ATETimeSystem/ATETimeAnalysis",
                defaults: new { controller = "ATETimeSystem", action = "ATETimeIndex" }
            );

            #endregion
            //
            #region Custom Admin Routes
            routes.MapRoute(
                "Progam",
                "Admin/Program/{action}/{id}",
                new { controller = "AdmProgram", action = "ProgramIndex", id = UrlParameter.Optional }
            );

            #endregion
        }
    }
}
