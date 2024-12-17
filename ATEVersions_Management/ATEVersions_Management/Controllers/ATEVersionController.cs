using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.DAOModels;
using ATEVersions_Management.Models.HelperModels;
using Syncfusion.XlsIO;

namespace ATEVersions_Management.Controllers
{
    public class ATEVersionController : Controller
    {
        // ====== Connect to database ======
        readonly ATEVersionContext db = new ATEVersionContext();
        //
        // ====== =================== ======

        #region ====== Controller Requests ======
        // GET: ATEVersion Client side
        public ActionResult CltVersionIndex()
        {
            if (User.Identity.IsAuthenticated)
            {

                ViewBag.ProgramList = new SelectList(ATEVersionsDAO.GetAllProgram(),"ProgramID", "ModelName");
                ViewBag.Message = TempData["Message"];                
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        // Partial ate version datatable action
        public PartialViewResult VrsDataTable(int? prgId, string searchStr, string model)
        {
            List<VersionDTO> listVersion = ATEVersionsDAO.GetVersionListByModel(model);

            if (!string.IsNullOrEmpty(searchStr))
            {
                listVersion = ATEVersionsDAO.GetVersionListWithSearchKey(searchStr);
            }
            // Check latest version 
            //Dictionary<string, string> dicLastestModelVer = TETestDataDAO.DictionaryLastestVersionOfModel();
            listVersion = ATEVersionsDAO.SetAdditionalVersionsData(listVersion);
            int NoATEListCount = listVersion.Where(ate => ate.IsATEListAvailable == 0).Count();
            int UnchekedCount = listVersion.Where(ate => ate.IsATEListChecked == 0).Count();
            TempData["NoATEListCount"] = NoATEListCount;
            TempData["UnchekedCount"] = UnchekedCount;
            return PartialView(listVersion);
        }
        // Client ATE Checklist view
        public ActionResult CltATEList(int ateVerID)
        {
            if (User.Identity.IsAuthenticated)
            {
                VERSION vrs = db.VERSIONs.Find(ateVerID);
                ATE_CHECKLIST cltAte = db.ATE_CHECKLIST.SingleOrDefault(ate => ate.VersionID == ateVerID);
                ViewBag.ChecklistItems = db.CHECKLIST_ITEM.ToList();
                if (cltAte != null)
                {
                    return View(cltAte);
                }

                string msg = "Version " + vrs.PROGRAM.ModelName+ "_" + vrs.VersionName + " has not had checklist yet.";
                Notification.setFlash(msg, "danger");
                return RedirectToAction("CltVersionIndex");
            }

            return RedirectToAction("Login", "Account");
        }
        // Partial ATE Checklist
        public ActionResult PartialVersionATEList(int ateVerID)
        {
            try
            {
                VERSION vrs = db.VERSIONs.Find(ateVerID);
                ATE_CHECKLIST cltAte = db.ATE_CHECKLIST.SingleOrDefault(ate => ate.VersionID == ateVerID);
                ViewBag.ChecklistItems = db.CHECKLIST_ITEM.ToList();
                
                if (cltAte == null)
                {
                    cltAte = new ATE_CHECKLIST();
                }
                ViewBag.versionATEList = cltAte;
                return PartialView();
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ":\n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region ====== AJAX Calling Functions ======

        public JsonResult GET_ListLatestVersion()
        {
            try
            {
                return Json(ATEVersionsDAO.GET_ListLatesATEtVersionComparisionMethod(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GET_ListLatestVersionOnline()
        {
            try
            {
                return Json(ATEVersionsDAO.GetListLatestVersionsFullInfo(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_ListLatestVersionOnlineByProjectType(string projectType)
        {
            try
            {
                return Json(ATEVersionsDAO.GET_ListLatestVersionsFullInfoByProjectType(projectType), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + "\nDetail:\n" + ex.InnerException, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ====== Support Functions ======
        // Export versions to excel
        public void VersionToExcel(int? prgId)
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Initialize Application
                IApplication application = excelEngine.Excel;
                //Set the default application version as Excel 2016
                application.DefaultVersion = ExcelVersion.Excel2016;
                //Create a workbook with a worksheet
                IWorkbook workbook = application.Workbooks.Create(1);
                //Access first worksheet from the workbook instance
                IWorksheet worksheet = workbook.Worksheets[0];
                //Define columns of DataTable 
                DataTable table = new DataTable();                               
                table.Columns.Add("Version", typeof(string));
                table.Columns.Add("Engineer", typeof(string));
                table.Columns.Add("Build time", typeof(DateTime));
                table.Columns.Add("Release time", typeof(DateTime));
                table.Columns.Add("Release note", typeof(string));

                //Fill data row to datatable
                int no = 0;
                string nameFormat = "All";
                List<VERSION> versions = db.VERSIONs.ToList();
                if (prgId.HasValue)
                {
                    ProgramDTO tmpProgram = (from p in db.PROGRAMs
                                            where p.ProgramID == prgId
                                            select new ProgramDTO{
                                                ModelName = p.ModelName
                                            }).SingleOrDefault();
                    nameFormat = tmpProgram.ModelName;
                    versions = db.VERSIONs.OrderBy(v => v.VersionName).Where(v => v.ProgramID == prgId).ToList();
                }

                foreach (var vrs in versions)
                {
                    string tmpVerName = vrs.PROGRAM.ModelName + "_" + vrs.VersionName;
                    table.Rows.Add(tmpVerName, vrs.Engineer, vrs.BuildTime, vrs.ReleaseTime, vrs.ReleaseNote);
                }

                //import datatable to worksheet
                worksheet.ImportDataTable(table, true, 1, 1);
                worksheet.UsedRange.AutofitColumns();
                //Save the workbook to disk in xlsx format                
                workbook.SaveAs(nameFormat + "_ReleaseNotes.xlsx", ExcelSaveType.SaveAsXLS, HttpContext.ApplicationInstance.Response, ExcelDownloadType.Open);
            }
            
        }
        // Cut product name format
        public string ProductCode(string prgName)
        {
            string regexUC = @"^U\d{2}C\d{3}$";
            string regexUG = @"^U\d{2}G\d{3}$";
            string regexUH = @"^U\d{2}H\d{3}$";
            string regexUL = @"^U\d{2}L\d{3}$";
            string regexUBN = @"^UBN\d{4}$";
            string verCutStart = prgName.Substring(0, 7).Trim();
            string verCutEnd = prgName.Substring(prgName.Length - 7, 7).Trim();
            string result = verCutEnd;
            if (Regex.IsMatch(verCutStart, regexUC) ||
               Regex.IsMatch(verCutStart, regexUG) ||
               Regex.IsMatch(verCutStart, regexUH) ||
               Regex.IsMatch(verCutStart, regexUL) ||
               Regex.IsMatch(verCutStart, regexUBN) )
            {
                result = verCutStart;
            }
            
            return result;
        }
        #endregion
    }
}