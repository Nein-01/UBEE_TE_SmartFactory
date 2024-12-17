using ATEVersions_Management.Models.DAOModels;
using ATEVersions_Management.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATEVersions_Management.Controllers
{
    public class TestPlanController : Controller
    {
        #region =========== View Controller Functions ===========
        // GET: TestPlan
        public ActionResult CltTestPlanIndex()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            List<TestPlanDTO> listTestPlan = ATEVersionsDAO.GetTestPlanList();
            return View(listTestPlan);
        }
        #endregion

        #region =========== AJAX Calling Functions ===========
        public ActionResult _PartialTestPlanTable(string model)
        {
            try
            {
                List<TestPlanDTO> listTestPlan = ATEVersionsDAO.GetTestPlanByModel(model);
                return PartialView(listTestPlan);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            
        }
        public JsonResult GET_ListModelOfTestPlan()
        {
            try
            {
                List<string> listModelTestPlan = ATEVersionsDAO.GetListModelOfTestPlan();
                return Json(listModelTestPlan,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_ListTestPlanPreview()
        {
            try
            {
                List<TestPlanPreviewDTO> listTestPlanPreview = ATEVersionsDAO.GetListTestPlanPreview();
                return Json(listTestPlanPreview, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_ListTestPlanPreviewByProjectType(string projectType)
        {
            try
            {
                List<TestPlanPreviewDTO> listTestPlanPreview = ATEVersionsDAO.GET_ListTestPlanPreviewByProjectType(projectType);
                return Json(listTestPlanPreview, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}