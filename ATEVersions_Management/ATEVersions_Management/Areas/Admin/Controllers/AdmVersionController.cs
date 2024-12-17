using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.DAOModels;
using ATEVersions_Management.Models.HelperModels;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Syncfusion.CompoundFile.XlsIO.Native;

namespace ATEVersions_Management.Areas.Admin.Controllers
{
    public class AdmVersionController : DashboardController
    {
        readonly ATEVersionContext ateContext = new ATEVersionContext();
        // GET: Admin/AdmVersion
        public ActionResult VersionIndex()
        {
            if (!User.Identity.IsAuthenticated)
            {                
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            ViewBag.ProgramList = new SelectList(ATEVersionsDAO.GetAllProgram(), "ModelName", "ModelName");
            ViewBag.ModelKeeper = TempData["ModelKeeper"];

            return View();
        }
        // Partial ate version datatable action
        public ActionResult _AdmPartTableVersions(int? prgId, string searchStr, string model)
        {
            List<VersionDTO> listVersion = ATEVersionsDAO.GetVersionListByModel(model);

            if (!string.IsNullOrEmpty(searchStr))
            {
                listVersion = ATEVersionsDAO.GetVersionListWithSearchKey(searchStr);
            }
            if (prgId.HasValue)
            {
                listVersion = listVersion.Where(v => v.Status != 0 && v.ProgramID == prgId).ToList();                
            }
            // Check latest version 
            //Dictionary<string, string> dicLastestModelVer = TETestDataDAO.DictionaryLastestVersionOfModel();
            listVersion = ATEVersionsDAO.SetAdditionalVersionsData(listVersion);
            return PartialView(listVersion);
        }
        // Detail of version infor
        public ActionResult VersionDetail(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }            
            VERSION dtlVer = ateContext.VERSIONs.Find(id);
            return View(dtlVer);
        }

        // Create new version info
        public ActionResult VersionCreate()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }      
            
            if (!(User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer"))
            {
                return RedirectToAction("VersionIndex");
            }            

            ViewBag.ProgramList = new SelectList(ATEVersionsDAO.GetAllProgram(), "ProgramID", "ModelName");
            ViewBag.Error = TempData["Error"];
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> VersionCreate(VERSION vrs)
        {
            ViewBag.ProgramList = new SelectList(ATEVersionsDAO.GetAllProgram(), "ProgramID", "ModelName");
            PROGRAM prgTmp = ateContext.PROGRAMs.SingleOrDefault(p => p.ProgramID == vrs.ProgramID);
            string modelName = prgTmp.ModelName;
            try
            {
                if (!ExistVer(vrs.VersionID, vrs.ProgramID, vrs.VersionName))
                {
                    //Add data to model
                    vrs.Status = 1;
                    vrs.CreatedAt = DateTime.Now;
                    vrs.UpdatedAt = DateTime.Now;
                    vrs.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();
                    ateContext.VERSIONs.Add(vrs);
                    //Save to database
                    await ateContext.SaveChangesAsync();
                    //Finishing up
                    TempData["ModelKeeper"] = modelName;
                    Notification.setFlash1s("Create " + vrs.VersionName+" of " + modelName + " successfully!", "success");
                    //return RedirectToAction("VersionIndex");
                    return RedirectToAction("ATEListCreate","AdmATEList", new { versionId = vrs.VersionID } );
                }                                            
                
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
                Notification.setFlash1s("Error on creating " + vrs.VersionName + " of " + modelName + ": \n"+ ex.ToString(), "danger");
                Console.WriteLine(ex.ToString());
            }
            
            ViewBag.Error = "Version " + vrs.VersionName + " of " + modelName + " is existed!";
            return View();
        }
        
        // Update version info
        public ActionResult VersionEdit(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer")
                {
                    VERSION edtVer = ateContext.VERSIONs.Find(id);

                    return View(edtVer);
                }
                return RedirectToAction("VersionIndex");
            }
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        [HttpPost]
        public async Task<ActionResult> VersionEdit(VERSION vrs)
        {
            VERSION edtVrs = ateContext.VERSIONs.Find(vrs.VersionID);
            PROGRAM prgTmp = ateContext.PROGRAMs.SingleOrDefault(p => p.ProgramID == vrs.ProgramID);
            string prgName = prgTmp.ProgramName;
            try
            {
                if (!ExistVer(vrs.VersionID, vrs.ProgramID, vrs.VersionName))
                {
                    //Edit data of model                    
                    edtVrs.VersionName = vrs.VersionName;                                        
                    edtVrs.Engineer = vrs.Engineer;
                    edtVrs.BuildTime = vrs.BuildTime;
                    edtVrs.ReleaseTime = vrs.ReleaseTime;
                    edtVrs.ReleaseNote = vrs.ReleaseNote;
                    edtVrs.UpdatedAt = DateTime.Now;
                    edtVrs.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();
                    //Save to database
                    await ateContext.SaveChangesAsync();
                    //Finishing up
                    Notification.setFlash1s("Update " + vrs.VersionName + " of " + prgName + " successfully!", "success");
                    return Redirect("~/Admin/AdmVersion/VersionDetail/" + vrs.VersionID);
                }
               
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
                Console.WriteLine(ex.ToString());
                Notification.setFlash1s("Fail to update version " + vrs.VersionName + " of " + prgName + "!\n"+"Error: "+ ex.ToString(), "danger");
            }
            Notification.setFlash1s(vrs.VersionName + " of " + prgName + " is existed!", "danger");
            return View(edtVrs);
        }
        // Delete version info (change status to [0:disable] not delete record)
        [HttpPost]
        public async Task<ActionResult> VersionDelete(int id)
        {
            string modelName = "";
            if (User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer")
            {
                try
                {
                    VERSION dltVer = ateContext.VERSIONs.Find(id);
                    modelName = dltVer.PROGRAM.ModelName;
                    TempData["ModelKeeper"] = modelName;

                    ateContext.VERSIONs.Remove(dltVer);
                    await ateContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
            }

            return RedirectToAction("VersionIndex");
        }

        // View import version from csv files
        public ActionResult VersionImport()
        {

            if (User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer")
            {
                List<VersionDTO> versions = new List<VersionDTO>();
                ViewBag.Versions = versions;
                ViewBag.ProgramList = new SelectList(ATEVersionsDAO.GetAllProgram(),"ProgramID", "ModelName");
                ViewBag.Error = TempData["Error"];
                return View();
            }
            return RedirectToAction("VersionIndex");
        }
        // POST: Action import version from file
        [HttpPost]
        public async Task<ActionResult> VersionImport(VersionDTO verImp, HttpPostedFileBase uploadFile)
        {
            //Send data back to view if fail
            ViewBag.ProgramList = new SelectList(ATEVersionsDAO.GetAllProgram(), "ProgramID", "ModelName");
            //Prepare needed data
            List<VersionDTO> versionsImpList = new List<VersionDTO>();
            string tableName = "";
            ProgramDTO prg = (from p in ateContext.PROGRAMs
                                  where p.ProgramID == verImp.ProgramID
                                  select new ProgramDTO
                                  {
                                      ProgramID = p.ProgramID, 
                                      ProgramName = p.ProgramName,
                                      ModelName = p.ModelName,
                                  }
                                  ).FirstOrDefault();
            if (uploadFile == null)
            {
                Notification.setFlash("No file to upload!", "danger");
                ViewBag.Versions = versionsImpList;
                return View();
            }
            string fileName = uploadFile.FileName;
            //Check file is compatibale with program or not
            if (!IsFileCompatible(fileName, prg.ProgramName, prg.ModelName))
            {
                Notification.setFlash("File: " + fileName + " is not compatible with " + prg.ModelName+"!","danger");
                ViewBag.Versions = versionsImpList;
                return View();
            }
            
            //Process get data from file
            fileName = Path.Combine(Server.MapPath("~/App_Data/" + uploadFile.FileName));
            uploadFile.SaveAs(fileName);
            string errorMess = "";
            bool CheckImportVer = GetVersionFromCSV(fileName, ref tableName, ref versionsImpList, ref errorMess);
            //Check import data success or not
            if (!CheckImportVer)
            {
                ViewBag.Versions = versionsImpList;
                ViewBag.TableName = tableName;
                //Finishing up
                Notification.setFlash("Error when import file: " + uploadFile.FileName + "!\n"+"Detail: "+errorMess, "danger");
                return View();
            }
            //Filter to remove existed versions
            versionsImpList = FilterVerFile(versionsImpList, verImp.ProgramID);
            if(versionsImpList.Count == 0)
            {
                ViewBag.Versions = versionsImpList;
                ViewBag.TableName = tableName;
                //Finishing up
                Notification.setFlash("No different version from database in file: " + uploadFile.FileName + " is existed!", "danger");
                return View();
            }
            //Add Versions to database
            try
            {
                foreach (var item in versionsImpList)
                {
                    VERSION tmpVer = new VERSION
                    {
                        ProgramID = verImp.ProgramID,
                        VersionName = item.VersionName,
                        Engineer = item.Engineer,
                        BuildTime = item.BuildTime,
                        ReleaseTime = item.ReleaseTime,
                        ReleaseNote = item.ReleaseNote,
                        Status = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName()
                    };
                    ateContext.VERSIONs.Add(tmpVer);

                    await ateContext.SaveChangesAsync();
                }


                ViewBag.Versions = versionsImpList;
                ViewBag.TableName = tableName;
                //Finishing up
                Notification.setFlash("File: " + uploadFile.FileName + " is imported successfully!", "success");
                return View();
            }
            catch (Exception ex)
            {
                //Finishing up
                Notification.setFlash("Error on importing version from file: " + uploadFile.FileName + "!", "danger");
                Console.WriteLine(ex.Message);
            }
            ViewBag.Versions = versionsImpList;
            ViewBag.TableName = tableName;
            
            return View();
        }
        // Read data from uploaded file
        public bool GetVersionFromCSV(string fileName, ref string tableName, ref List<VersionDTO> lstVersion, ref string errorMess)
        {            
            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                //Initialize Application
                IApplication application = excelEngine.Excel;
                //Set the default application version as Excel 2016
                application.DefaultVersion = ExcelVersion.Excel2016;
                //Open a workbook with a worksheet
                //fileName = @"E:\02_Programing\08.C#_Projects\04.TestWindowPrint\App_Data\U10C100.30.csv";
                //fileName = @"E:\02_Programing\08.C#_Projects\04.TestWindowPrint\App_Data\U10C116.10.csv";
                IWorkbook workbook = application.Workbooks.Open(fileName);
                //Access first worksheet from the workbook instance
                IWorksheet worksheet = workbook.Worksheets[0];
                //Tranfer data to DataTable
                DataTable table = new DataTable();
                table = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
                
                lstVersion = new List<VersionDTO>();                
                tableName = table.TableName;
                try
                {
                    foreach (DataRow row in table.Rows)
                    {
                        int count = lstVersion.Count;
                        VersionDTO tmpVersion = new VersionDTO();
                        tmpVersion.ReleaseNote = row[table.Columns[4].ColumnName].ToString();
                        if (tmpVersion.ReleaseNote.ToLower().Contains("end") ||
                            string.IsNullOrEmpty(tmpVersion.ReleaseNote)) continue;
                        if (!tmpVersion.ReleaseNote.StartsWith("1.") && count > 0)
                        {
                            lstVersion[count - 1].ReleaseNote += "\n" + tmpVersion.ReleaseNote;
                            continue;
                        }
                        //cut data from VersionName column
                        string tmpVerName = row[table.Columns[0].ColumnName].ToString();
                        int startCut = tmpVerName.IndexOf('V');
                        int endCut = tmpVerName.Length - startCut;
                        tmpVersion.VersionName = tmpVerName.Substring(startCut,endCut);
                        tmpVersion.ModelName = tmpVerName.Substring(0, startCut-1);
                        //Remain column
                        tmpVersion.Engineer = row[table.Columns[1].ColumnName].ToString();
                        tmpVersion.BuildTime = DateTime.Parse(row[table.Columns[2].ColumnName].ToString());
                        tmpVersion.ReleaseTime = DateTime.Parse(row[table.Columns[3].ColumnName].ToString());

                        lstVersion.Add(tmpVersion);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    errorMess = ex.Message;
                    System.IO.File.Delete(fileName);
                    return false;
                }                
            }
            System.IO.File.Delete(fileName);
            lstVersion = lstVersion.OrderByDescending(v => v.ReleaseTime)
                .GroupBy(v => v.VersionName)
                .Select(g => g.First())
                .ToList();
            lstVersion = lstVersion.OrderBy(v => v.VersionName).ToList();
            return true;

            //return lstVersion;
        }        

        #region Hepler
        public bool ExistVer(int? verId, int prgId, string name)
        {
            VERSION tmp = ateContext.VERSIONs.FirstOrDefault(v => v.ProgramID == prgId && v.VersionName.ToLower() == name.ToLower());
            if(verId.HasValue)
            {
                tmp = ateContext.VERSIONs.FirstOrDefault(v => v.VersionID != verId && v.ProgramID == prgId && v.VersionName.ToLower() == name.ToLower());
            }
            return tmp != null;
        }
        public bool IsFileCompatible(string fileName, string prgName, string modelName)
        {
            //return fileName.ToLower().Contains(modelName);
            return prgName.ToLower().Contains(fileName.Substring(0,7).ToLower()) ||
                   modelName.ToLower().Contains(fileName.Substring(0, 7).ToLower());
        }
        public List<VersionDTO> FilterVerFile(List<VersionDTO> fileVers, int prgId)
        {
           return  (from v in fileVers
                        where !(from vImp in ateContext.VERSIONs
                                where vImp.ProgramID == prgId
                                select vImp.VersionName.ToLower()).Contains(v.VersionName.ToLower())
                        select v
                        ).ToList();
        }
        #endregion

    }
}