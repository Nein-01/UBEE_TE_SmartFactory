using ATEVersions_Management.Models.DAOModels;
using ATEVersions_Management.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATEVersions_Management.Controllers
{
    public class GRRModuleController : Controller
    {
        // GET: GRRModule
        public ActionResult GRRModuleIndex()
        {

            if (User.Identity.IsAuthenticated)
            {
                List<GRRTableDTO> listGRR = ATEVersionsDAO.GetGRRList();
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
                return View(listGRR);
            }
            return RedirectToAction("Login", "Account");
        }
        //GET: GRR Report client view
        //
        public ActionResult GRRModuleDetail(int id)
        {
            GRRTableDTO detailGRR = ATEVersionsDAO.GetGRRByID(id);
            return View(detailGRR);
        }
    }
}