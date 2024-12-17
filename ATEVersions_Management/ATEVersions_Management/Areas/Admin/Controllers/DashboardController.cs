using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.HelperModels;

namespace ATEVersions_Management.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // Contructor to detect authentication
        public DashboardController() {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("~/Account/Login");
            }            
            if (System.Web.HttpContext.Current.User.Identity.GetRoleCode() == 2)
            {
                System.Web.HttpContext.Current.Response.Redirect("~/Home/Index");
            }           
        }

        readonly ATEVersionContext db = new ATEVersionContext();
        //readonly SmartFactoryContext db = new SmartFactoryContext();

        // GET: Admin/Dashboard
        public ActionResult AdminIndex()
        {
            return View();
        }        
        // Back to client index page
        public ActionResult BackToHome()
        {
            return Redirect("~/Home/Index");
        }
        // Logout from admin site
        public ActionResult Logout() 
        { 
            FormsAuthentication.SignOut();
            return Redirect("~/Account/Login");
        }        
    }
}