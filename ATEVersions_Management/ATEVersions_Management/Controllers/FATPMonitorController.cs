using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ATEVersions_Management.Models.DAOModels;
using ATEVersions_Management.Models.DTOModels;
namespace ATEVersions_Management.Controllers
{
    public class FATPMonitorController : Controller
    {
        // GET: FATPMonitor
        public ActionResult FATPMonitorIndex()
        {   
            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.FATPLine = FATPTableDAO.GetAllFATPLine();
            return View();
        }
        // GET: Partial FATP table data
        public PartialViewResult _PartFATPTableData(string line, string station)
        {
            List<FATPTableDTO> listFATP = FATPTableDAO.GetListFATPByLineStation(line, station);
            listFATP = FATPTableDAO.GetFATPIdentifiedAbnormal(listFATP);
            List<string> listModelInLineStation = listFATP.OrderBy(model => model.MODEL).Select(model => model.MODEL).Distinct().ToList();
            TempData["listModelInLineStation"] = listModelInLineStation;
            ViewBag.ListModelInProducing = listModelInLineStation;
            return PartialView(listFATP);
        }
        // GET: Design detail view
        public ActionResult FATPMonitorDetail(int FATP_ID)
        {
            try
            {
                FATPTableDTO detailFATP = FATPTableDAO.GetFATPByID(FATP_ID);
                return View(detailFATP);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }            
        }
        // GET: Partial view detail of FATP
        public ActionResult _PartFATPDetail(int  FATP_ID)
        {
            try
            {
                FATPTableDTO detailFATP = FATPTableDAO.GetFATPByID(FATP_ID);
                return PartialView(detailFATP);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }

            
        }

        #region AJAX Calling functions
        public JsonResult GET_AllFATPLine()
        {
            
            try
            {
                List<string> fatpLine = FATPTableDAO.GetAllFATPLine();
                return Json(fatpLine, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_AllTabFATPLine()
        {
            
            try
            {
                List<TabFATPLine> tabFATPLine = FATPTableDAO.GetAllTabFATPLine();
                return Json(tabFATPLine, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_AllTabFATPLineStation()
        {
            
            try
            {
                List<TabFATPLineStation> listLineStation = FATPTableDAO.GetAllTabFATPLineStation();
                return Json(listLineStation, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_AllFATPLineModel(string line)
        {
            
            try
            {
                List<string> fatpLineModel = FATPTableDAO.GetAllFATPModelByLine(line);
                return Json(fatpLineModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_AllFATPLineStation(string line)
        {
            
            try
            {
                List<string> fatpLineStation = FATPTableDAO.GetAllFATPStationByLine(line);
                return Json(fatpLineStation, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_ChangeFATPLockStatus(string userName, string model, string atePC, int lockStatus)
        {            
            
            try
            {
                return Json(StationInforDAO.ChangePCLockStatus(userName, model, atePC, lockStatus), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(ex,JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SET_ResetFAILNUMbyFAILBUFFER(string atePC, string ateIP, string model)
        {
            try
            {
                bool isResetOk = FATPTableDAO.SET_ResetFAILNUMbyFAILBUFFER(atePC, ateIP, model);
                return Json(isResetOk, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}