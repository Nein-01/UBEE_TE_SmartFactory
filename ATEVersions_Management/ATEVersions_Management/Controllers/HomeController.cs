using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.HelperModels;
using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.DAOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Syncfusion.XlsIO;
using System.Data;

namespace ATEVersions_Management.Controllers
{    
    public class HomeController : Controller
    {
        // ====== Connect to database ======
        readonly ATEVersionContext db = new ATEVersionContext();
        //
        // ====== =================== ======

        // ====== Handling requests ======
        
        // Client Index view
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }            
            ViewBag.Message = TempData["Message"];
            //return RedirectToAction("CltVersionIndex", "ATEVersion");
            return View();
        }

        // About view
        public ActionResult About()
        {
            List<VersionDTO> listCurrentVersion = ATEVersionsDAO.GetVersionList();
            //List<TETestDataDTO> listModelVersion = TETestDataDAO.GetListVersionOfModel();
            Dictionary<string,string> dicLastestModelVer = TETestDataDAO.DictionaryLastestVersionOfModel();
            //ViewBag.ModelVersion = listModelVersion;
            ViewBag.LatestModelVer = dicLastestModelVer;
            List<VersionDTO> ListVersions = ATEVersionsDAO.SetAdditionalVersionsData(listCurrentVersion);
            ViewBag.LatestCurrentModelVer = ListVersions;

            return View();
        }
        // Contact view
        public ActionResult Contact()
        {

            ViewBag.Message = "Contact page.";            

            return View();
        }
        // Error foward view
        public ActionResult Error404Page()
        {
            return View();
        }
        public ActionResult TestLayout()
        {
            return View();
        }
    }
}