using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.DAOModels;
using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.HelperModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ATEVersions_Management.Areas.Admin.Controllers
{
    public class AdmGRRSystemController : Controller
    {
        readonly ATEVersionContext db = new ATEVersionContext();
        #region Requests Handling Controllers
        // GET: Admin/AdmGRRSytem
        public ActionResult GRRIndex()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }            
            List<GRRTableDTO> listGRR = ATEVersionsDAO.GetGRRList();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(listGRR);
        }

        // GET: Rejected GRR Reports view
        public ActionResult GRRRejectedView()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }            
            List<GRRTableDTO> listRejectedGRR = ATEVersionsDAO.GetGRRRejectedList();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(listRejectedGRR);
        }

        // GET: View for creating new GRR record
        public ActionResult GRRCreate()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            ViewBag.ListGRRModel = new SelectList(CPKTableDAO.GetListModel());
            return View();
        }
        // POST: Action to create new GRR record
        [HttpPost]
        public async Task<ActionResult> GRRCreate(GRRTableDTO createGRRTableDTO)
        {
            ViewBag.ListGRRModel = new SelectList(CPKTableDAO.GetListModel());
            try
            {
                // Get data to entity model
                DateTime currentTime = DateTime.Now;
                GRR_TABLE createGRR = new GRR_TABLE
                {
                    UserID = User.Identity.GetUserId(),
                    Dept = "TE",
                    GageModel = createGRRTableDTO.GageModel,
                    GageName = createGRRTableDTO.GageName,
                    GageNo = createGRRTableDTO.GageNo,
                    PartName = createGRRTableDTO.PartName,
                    Specification = createGRRTableDTO.Specification,
                    Characteristic = createGRRTableDTO.Characteristic,
                    JSON_OperTestResult = createGRRTableDTO.JSON_OperTestResult,
                    PreparedBy = User.Identity.GetName(),
                    PreparedAt = currentTime,
                    PreparedNote = createGRRTableDTO.PreparedNote,
                    Status = 1,
                    CreatedAt = currentTime,
                    UpdatedAt = currentTime,
                    UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName(),

                };
                // Save data to database
                db.GRR_TABLE.Add(createGRR);
                await db.SaveChangesAsync();
                // Finishing up
                Notification.setFlash1s("Create new GR&R report successfully!", "success");
                return RedirectToAction("GRRIndex");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error on creating new GR&R report: \n" + ex.Message;
            }

            return View(createGRRTableDTO);
        }
        // GET: View to display detail GRR record
        public ActionResult GRRDetail(int id)
        {
            GRRTableDTO detailGRR = ATEVersionsDAO.GetGRRByID(id);
            return View(detailGRR);
        }
        
        // POST: Action to delete GRR record
        [HttpPost]
        public async Task<ActionResult> GRRDelete(int id)
        {
            GRR_TABLE deleteGRR = db.GRR_TABLE.Find(id);
            if (User.Identity.GetUserId() == deleteGRR.UserID)
            {                
                try
                {
                    //Update database
                    db.GRR_TABLE.Remove(deleteGRR);
                    await db.SaveChangesAsync();
                    //Finishing up
                    Notification.setFlash1s("Successfully deleted!","success");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    Console.WriteLine("Error: " + ex);
                }
            }
            return RedirectToAction("GRRIndex");
        }
        // POST: Approving GRR record        
        [HttpPost]
        public async Task<ActionResult> GRRApproving(int GRR_ID, int Status, string ApproverNote)
        {
            try
            {
                // Get data to enetity model
                DateTime currentTime = DateTime.Now;
                string userName = User.Identity.GetName();
                GRR_TABLE grrRecord = db.GRR_TABLE.Find(GRR_ID);
                grrRecord.Status = Status;
                grrRecord.ApproverNote = ApproverNote;
                grrRecord.ApprovedAt = currentTime;
                grrRecord.ApprovedBy = userName;
                grrRecord.UpdatedAt = currentTime;
                grrRecord.UpdatedBy = User.Identity.GetUserName() + " | " + userName;
                // Finishing up
                await db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            
            return RedirectToAction("GRRDetail", new { id = GRR_ID });
        }
        #endregion

        #region AJAX Calling Functions
        public JsonResult GETListStationByModel(string model) 
        { 
            List<string> ListStationByModel = CPKTableDAO.GetListStationByModel(model);
            return Json(ListStationByModel,JsonRequestBehavior.AllowGet);
        }
        public JsonResult GETListItemInStation(string model, string station)
        {
            try
            {
                CPKTableDTO cpkSingleRecord = CPKTableDAO.GetOneCPKDataByModelAndStation(model, station);
                //cpkSingleRecord.CPKContentObjects = CPKTableDAO.GetCPKRecordContentObjectList(cpkSingleRecord.RawContents);
                GRRModelStationContent grrContent = CPKTableDAO.GetGRRModelStationContent(cpkSingleRecord);
                Session["cpkContent"] = grrContent;
                return Json(grrContent, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public JsonResult GETOperGRRSampleFromCPKTable(int operNo, string operName, string model, string station, int pos, double? lsl, double? usl, List<int> listPosMark, DateTime fromDay, DateTime toDay)
        {
            try
            {
                int pcsNum = 3, tryNo = 10;

                List<CPKTableDTO> listRandSamples = CPKTableDAO.GetRandomOperSamples(model, station, tryNo * pcsNum, fromDay, toDay);
                double[] arrSamples = CPKTableDAO.GetArrSamples(listRandSamples, pos, tryNo * pcsNum);

                //List<double[]> listArrSamples = CPKTableDAO.DivideSamplesIntoList(arrSamples, pcsNum);                
                
                /*List<double[]> listArrSamples = CPKTableDAO.GETGRRModifiedSamples(arrSamples, lsl, usl);

                OBJ_OperTestResult operSamplesResult = new OBJ_OperTestResult
                {
                    ID = operNo,
                    OperName = operName,
                    OperSamples = listArrSamples,
                };*/

                GRRModifiedOperSample GRROperSample = CPKTableDAO.GET_ModifiedOperSample(operNo, operName, listPosMark, arrSamples, lsl, usl);

                return Json(GRROperSample, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }            
        }

        [HttpPost]
        public JsonResult POSTGrrCalculateResult(List<OBJ_OperTestResult> dataObject)
        {
            try
            {
                /*GRRCalculateXbarRange grrCalResult = new GRRCalculateXbarRange
                   {
                       ListOperSamples = dataObject,
                   };*/

                GRRCalculateANOVA grrCalResult = new GRRCalculateANOVA
                {
                    ListOperSamples = dataObject,
                };

                return Json(grrCalResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
           
        }
        #endregion

        #region Support Functions

        #endregion
    }
}