using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ATEVersions_Management.Models.DAOModels;
using ATEVersions_Management.Models.DTOModels;
namespace ATEVersions_Management.Controllers
{
    public class TestTimeSystemController : Controller
    {
        #region ====== Equipment Estimate Section ======
        // ====== Controller Actions ======

        public ActionResult TestTimeIndex()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            
            ViewBag.ListLines = TETestDataDAO.GetListLines();
            return View();
        }

        // ====== AJAX Calling Functions ======
        [HttpGet]
        public ActionResult GET_TETestData(DateTime fromDate, DateTime toDate)
        {
            List<TETestDataEquipmentEstimate> listModelStation = TETestDataDAO.GetModelStationByLine(fromDate,toDate);
            listModelStation.ForEach(data => { data.DataID++; });

            return Json(AssignIDToList(listModelStation), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult POST_ReplaceTETestData(int dataId, string line, string model, string station,  double unitTestTime, double loadTime, double dailyTargetOutput, double equipExisted, double runHours, int pcsTotal)
        {
            TETestDataEquipmentEstimate replaceResult = new TETestDataEquipmentEstimate
            {
                DataID = dataId,
                Line = line,
                Model = model,
                Station = station,                
                UnitTestTime = unitTestTime,
                LoadTime = loadTime,
                DailyTargetOutput = dailyTargetOutput,
                EquipExisted = equipExisted,
                RunHours = runHours,
                PcsTotal = pcsTotal
            };
            return Json(replaceResult, JsonRequestBehavior.AllowGet);
        }
        
        // ====== Support Functions ======
        public List<TETestDataEquipmentEstimate> AssignIDToList(List<TETestDataEquipmentEstimate> dataList)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i].DataID = i;
            }
            return dataList;
        }

        #endregion

        #region ====== ATE Time Section ======
        // ====== Controller Actions ======
        public ActionResult ATETimeIndex()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            //Model option list
            List<string> listModel = TETestDataDAO.GetATETimeListModel(null,null);
            ViewBag.ListModel = listModel;
            ViewBag.SelectListModel = new SelectList(listModel);
            return View();
        }

        // ====== AJAX Calling Functions ======
        public JsonResult GET_ATETimeModels(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                List<string> listATEModel = TETestDataDAO.GetATETimeListModel(fromDate, toDate);
                return Json(listATEModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex,JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_ATETimeModelStations(string model, DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<string> listATEModelStation = TETestDataDAO.GetListStationByModel(model, fromDate, toDate);
                return Json(listATEModelStation, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_ATETimeModelStationData(string model, DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<TETestDataATETime> listATEModelStationData = TETestDataDAO.GetATETimeOfMachineByModel(model, fromDate, toDate);
                return Json(listATEModelStationData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ATETimeChartSection(string model, DateTime fromDate, DateTime toDate)
        {
            try
            {
                //List<TETestDataATETime> listATETime = TETestDataDAO.GetATETimeOfMachineByModel(model, fromDate, toDate);
                List<TETestDataATETime> listATETime = TETestDataDAO.Func_GET_ATETimeOfMachine(model, fromDate, toDate);
                List<string> listStation = listATETime.Select(t => t.Station).Distinct().ToList();
                ViewBag.ListATETime = listATETime;
                ViewBag.ListStation = listStation;
                ViewBag.Model = model;
                return PartialView();
            }
            catch (Exception ex) 
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            
        }        
        public ActionResult POST_ATETimeByWorkDate(string model, string station,string ateMachine, DateTime fromDate, DateTime toDate)
        {
            List<TETestDataATETime> listATETimeByDate = TETestDataDAO.GetATETimeOfMachineWithGroupDate(model, station, ateMachine, fromDate, toDate);

            return Json(listATETimeByDate, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GET_ATETimeOfMachine(string model, DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<TETestDataATETime> listATETimeOfMachine = TETestDataDAO.Func_GET_ATETimeOfMachine(model, fromDate, toDate);
                return Json(listATETimeOfMachine, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}