using ATEVersions_Management.Models.DAOModels.OracleReTableDAOs;
using ATEVersions_Management.Models.DTOModels.OracleReTableDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATEVersions_Management.Controllers
{
    public class IPQCLCRSystemController : Controller
    {
        // === Controller Functions ===
        // GET: OBASystem Index
        public ActionResult IPQCLCRIndex()
        {
            return View();
        }
        public PartialViewResult Partial_IPQCLCRDataTable(string fromDate, string toDate)
        {
            try
            {
                DataTable dtblLCRData = IPQC_LCR_DAO.GetLCRDataInTimeRange(fromDate, toDate);
                ViewBag.DtblLCRData = dtblLCRData;
                return PartialView();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return PartialView();
            }
        }

        // === AJAX Calling Functions ===
        public JsonResult GET_LCRShiftData(string fromDate, string toDate)
        {
            try
            {

                List<LCRWorkShiftDTO> listLCRShiftData = new List<LCRWorkShiftDTO>
                {
                    IPQC_LCR_DAO.GetLCRShiftData("",fromDate,toDate),
                    IPQC_LCR_DAO.GetLCRShiftData("night",fromDate,toDate),
                };
                return Json(listLCRShiftData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_LCRDataInTimeRange(string fromDate, string toDate)
        {
            try
            {
                DataTable dtblLCRData = IPQC_LCR_DAO.GetLCRDataInTimeRange(fromDate,toDate);
                return Json(dtblLCRData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_LCRDataByMaterialId(string materialId)
        {
            try
            {
                List<IPQC_LCR_DTO> LCRMaterialData = IPQC_LCR_DAO.GetLCRDataByMaterialId(materialId);
                return Json(LCRMaterialData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex, JsonRequestBehavior.AllowGet);
            }
        }
    }
}