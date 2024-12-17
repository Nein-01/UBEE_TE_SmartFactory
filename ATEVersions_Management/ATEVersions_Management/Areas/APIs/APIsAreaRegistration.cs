using System.Web.Mvc;

namespace ATEVersions_Management.Areas.CPKModule
{
    public class APIsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "APIs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "APIs",
                "APIs/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}