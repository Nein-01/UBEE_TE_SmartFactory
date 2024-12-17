using ATEVersions_Management.Models.DAOModels.TestMonitorDAOs;
using ATEVersions_Management.Models.DTOModels.TestMonitorDTOs;
using ATEVersions_Management.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ATEVersions_Management.Controllers
{
    public class EnergySavingController : Controller
    {
        // === Controller Functions ===
        // GET: EnergySaving
        public ActionResult EnergySavingIndex()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            List<MachineInforDTO> listMachine = MachineInforDAO.GetTodayAllMachineInfor();
            ViewBag.listMachine = listMachine;
            return View();
        }
        // === AJAX Calling Functions ===
        // MQTT Sending Messages
        [HttpPost]
        public JsonResult POST_MQTTPublishMessage(string topic, string message)
        {
            try
            {
                string webServerErrorLogDirPath = Server.MapPath(@"~\App_Data\ErrorLogs\");
                return Json(MQTTConnector.MQTTPublishMessage(topic, message, webServerErrorLogDirPath), JsonRequestBehavior.AllowGet);
                //return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            
        }
        // Machine Overall Info
        public JsonResult GET_TodayAllMachineInfor()
        {            
            try
            {
                List<MachineInforDTO> listMachine = MachineInforDAO.GetTodayAllMachineInfor();
                listMachine = FilterListTodayMachine(listMachine);
                return Json(listMachine, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_MachineOwner(string hostname, string line, DateTime timeCheck)
        {
            try
            {
                string machineOwner =  FaceRecognizeDAO.GetOwnerForTestMachine(hostname, line, timeCheck);
                return Json(machineOwner, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("Error: " + ex, JsonRequestBehavior.AllowGet);
            }
        }
        // Record By Machine
        public JsonResult GET_IssueRecordByHostName(string hostname)
        {
            try
            {
                List<IssueRecordDTO> listIssue = IssueRecordDAO.GetIssueRecordByHostName(hostname);
                return Json(listIssue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_MachineChangeRecordByHostName(string hostname)
        {
            try
            {
                List<MachineChangeRecordDTO> listChange = MachineChangeRecordDAO.GetMachineChangeRecordByHostName(hostname);
                return Json(listChange, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex, JsonRequestBehavior.AllowGet);
            }
        }
        // Energy Record
        public JsonResult GET_TodayEnergyTotal()
        {
            try
            {
                EnergyRecordDateTotalDTO todayEnergyTotal = EnergyRecordDAO.GetTodayEnergyTotal();
                return Json(todayEnergyTotal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_TimeRangeEnergyTotal(string timeRange)
        {
            try
            {
                List<EnergyRecordDateTotalDTO> timeRangeEnergyTotal = EnergyRecordDAO.GetTimeRangeEnergyTotal(timeRange);
                return Json(timeRangeEnergyTotal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex, JsonRequestBehavior.AllowGet);
            }
        }
        // AIR Control Module
        public JsonResult GET_CurrentAirEnableMachine()
        {
            try
            {
                List<MachineAirDTO> listAIREnble = MachineInforDAO.GetCurrentAirEnableMachine();
                return Json(listAIREnble, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex, JsonRequestBehavior.AllowGet);
            }
        }

        #region Support Functions
        private List<MachineInforDTO> FilterListTodayMachine(List<MachineInforDTO> _listMachine)
        {
            int machineCount = _listMachine.Count;
            for(int i = 0; i < machineCount; i++)
            {
                int ipRepeat = _listMachine.Where(machine => machine.IP == _listMachine[i].IP).Count();
                if (ipRepeat > 1)
                {
                    _listMachine[i].IsIPDup = true;
                }
            }

            return _listMachine.Where(machine => !(machine.IsIPDup && machine.IdleTime > 0.5)).ToList();
        }
        #endregion
    }
}