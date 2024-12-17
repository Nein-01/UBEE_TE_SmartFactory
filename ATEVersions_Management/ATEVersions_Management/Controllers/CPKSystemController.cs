using ATEVersions_Management.Models.CPKModels;
using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.DAOModels;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using ATEVersions_Management.Models.ATEVersionModels;

namespace ATEVersions_Management.Controllers
{
    public class CPKSystemController : Controller
    {
        // ====== =================== ======
        //     CPK From Database Section
        // ====== =================== ======
        #region ====== CPK From Database Section ======
        // ====== Handling requests ======
        // GET: CPKSystem Index View
        public ActionResult CPKDBIndex()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            List<CPKModelStationContent> modelStationList = CPKTableDAO.GetListCPKModelStation();

            return View(modelStationList);
            
        }

        // GET: CPKSystem model and station item list
        public ActionResult CPKItemList(string model, string station)
        {
            CPKTableDTO cpkRecord = CPKTableDAO.GetOneCPKDataByModelAndStation(model, station);
            CPKModelStationContent cpkModelStation = CPKTableDAO.GetModelStationContent(cpkRecord);
            TempData["CPKData"] = cpkModelStation;
            return View(cpkModelStation);
        }
        // GET: Partial view table CPK model and station item list  
        public ActionResult PartialTableCPKModelStationItem(string model, string station, int pcsNum, string keyMO, string keyPC, DateTime fromDay, DateTime toDay)
        {
            try
            {
                /* DateTime toDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 30, 0),
                    fromDay = toDay.AddDays(-1);*/
                CPKTableDTO cpkRecord = CPKTableDAO.GetOneCPKDataByModelAndStation(model, station);
                List<CPKTableDTO> rawCPKContentList = CPKTableDAO.GetCPKByModelStationDate(model, station, pcsNum, keyMO, keyPC, fromDay, toDay);
                //cpkRecord.CPKContentObjects = CPKTableDAO.GetCPKRecordContentObjectList(cpkRecord.RawContents);
                CPKModelStationContent cpkModelStation = CPKTableDAO.GetModelStationFullContentValue(CPKTableDAO.GetModelStationContent(cpkRecord), rawCPKContentList);
                // ======== Manipulate data section ========
                
                //
                TempData["cpkModelStation"] = cpkModelStation;
                return PartialView(cpkModelStation);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ":\n"+ex.InnerException,JsonRequestBehavior.AllowGet);
            }
          
        }
        // GET: Draw CPK chart
        public ActionResult CPKDrawing(string model, string station, int? pos, string itemName)
        {
            /*TempData.Keep("CPKData");
            CPKModelStationContent cpkModelStation = (CPKModelStationContent)TempData["CPKData"];*/
            
            ViewBag.ItemName = itemName;
            ViewBag.ModelName = model;
            ViewBag.StationName = station;                        
            ViewBag.Pos = pos;
            return View();
        }
        // GET: Partial view chart drawing section
        public PartialViewResult CPKChartSection(string model, string station, int? pos, int pcsNumber, string moNumber, string atePC, DateTime fromDay, DateTime toDay)
        {

            CPKTableDTO cpkRecord = CPKTableDAO.GetOneCPKDataByModelAndStation(model, station);
            //cpkRecord.CPKContentObjects = CPKTableDAO.GetCPKRecordContentObjectList(cpkRecord.RawContents);
            CPKModelStationContent cpkModelStation = CPKTableDAO.GetModelStationContent(cpkRecord);
            CPKCalculate cpkDrawData = new CPKCalculate();
            if (pos.HasValue)
            {
                //cpkDrawData = GetDataFromDb(cpkModelStation, pos.Value);
                cpkDrawData = GetCPKWithDate(cpkModelStation, pos.Value, pcsNumber, moNumber, atePC, fromDay, toDay);
            }

            return PartialView(cpkDrawData);
        }
        // GET: View data of specific SN
        public ActionResult CPKPcbSNCheck(string model, string station, string pcbSN)
        {

            CPKTableDTO cpkRecord = CPKTableDAO.GetOneCPKDataBySN(model, station, pcbSN);                        

            return View(cpkRecord);
        }
        // GET: View data of model and station
        public ActionResult CPKModelStationData()
        {

            return View();
        }

        // GET: View data of specific item
        public ActionResult CPKItemData(string model, string station, int? pos)
        {

            CPKTableDTO cpkRecord = CPKTableDAO.GetOneCPKDataByModelAndStation(model, station);
            CPKModelStationContent cpkModelStation = CPKTableDAO.GetModelStationContent(cpkRecord);
            CPKCalculate cpkDrawData = new CPKCalculate();
            if (pos.HasValue)
            {
                cpkDrawData = GetDataFromDb(cpkModelStation, pos.Value);
            }
            ViewBag.ModelName = model;
            ViewBag.StationName = station;

            return View(cpkDrawData);
        }

        // Export cpk data to excel
        public void POST_ExportCPKToExcel()
        {
            CPKModelStationContent cpkModelStationContent = TempData["cpkModelStation"] as CPKModelStationContent;
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
                //Create title columns
                table.Columns.Add("PCB_SN", typeof(string));

                foreach (CPKContentInvidual cpkContent in cpkModelStationContent.ContentGroup)
                {
                    table.Columns.Add(cpkContent.ItemName + " (" + cpkContent.SpecL + "," + cpkContent.SpecH + ")", typeof(string));
                }

                DataColumnCollection cpkDataColumns = table.Columns;

                //Fill data rows to datatable
                int rowNum = cpkModelStationContent.ListPCBSN.Count;
                for (int iRow = 0; iRow <= rowNum; iRow++)
                {
                    DataRow cpkDataRow = table.NewRow();

                    for(int iCol = 0; iCol < cpkDataColumns.Count; iCol++)
                    {
                        if(iRow == 0)
                        {
                            if(iCol == 0)
                            {
                                cpkDataRow[cpkDataColumns[iCol]] = "CPK";
                            }
                            if (iCol > 0)
                            {
                                cpkDataRow[cpkDataColumns[iCol]] = cpkModelStationContent.ContentGroup[iCol - 1].CPKResult.Cpk.ToString();
                            }
                        }
                        else
                        {
                            if (iCol == 0)
                            {
                                cpkDataRow[cpkDataColumns[iCol]] = cpkModelStationContent.ListPCBSN[iRow - 1];
                            }
                            else
                            {
                                cpkDataRow[cpkDataColumns[iCol]] = cpkModelStationContent.ContentGroup[iCol - 1].Value[iRow - 1];
                            }
                        }                        
                    }

                    table.Rows.Add(cpkDataRow);

                }
                
                //import datatable to worksheet
                worksheet.ImportDataTable(table, true, 1, 1);
                worksheet.UsedRange.AutofitColumns();
                //Save the workbook to disk in xlsx format
                string TestName = cpkModelStationContent.ModelName + "_" + cpkModelStationContent.Station + "_CPKData_Exported";
                workbook.SaveAs(TestName + ".xlsx", ExcelSaveType.SaveAsXLS, HttpContext.ApplicationInstance.Response, ExcelDownloadType.Open);
            }

        }

        #region ====== AJAX Calling Functions ======
        [HttpGet]
        public JsonResult GET_CPKOnOffModelList(DateTime fromDate, DateTime toDate)
        {
            try
            {                
                List<string> _cpkOnlineModels = CPKTableDAO.GET_TodayCPKModel(fromDate, toDate);
                List<string> _cpkOfflineModels = CPKTableDAO.GetAllCPKModel().Except( _cpkOnlineModels ).ToList();
                return Json(new {cpkOnlineModels = _cpkOnlineModels, cpkOfflineModels = _cpkOfflineModels }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GET_ListCPKModelStation()
        {
            try
            {
                List<CPKModelStationContent> listCPKModelStation = CPKTableDAO.GetListCPKModelStation();
                return Json(listCPKModelStation, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        public JsonResult GET_CPKModelList()
        {
            try
            {
                List<string> cpkModels = CPKTableDAO.GetAllCPKModel();
                return Json(cpkModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            
        }
        
        [HttpGet]
        public JsonResult GET_CPKStationList(string model)
        {
            try
            {
                List<string> cpkStations = CPKTableDAO.GetAllCPKStation(model);
                return Json(cpkStations, JsonRequestBehavior.AllowGet);
            }
            catch  (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            
        }
        
        [HttpGet]
        public async Task<JsonResult> ListMoByModel(string model, DateTime fromDay, DateTime toDay)
        {
            try
            {
                List<string> listMO = await CPKTableDAO.GetTaskListMOByModel(model, fromDay, toDay);

                return Json(listMO, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            
        }
        
        [HttpGet]
        public async Task<JsonResult> GET_PCList(string model, string station, DateTime fromDay, DateTime toDay)
        {
            try
            {
                List<string> listPC = await CPKTableDAO.GetTaskListPCByModelStation(model, station, fromDay, toDay);

                return Json(listPC, JsonRequestBehavior.AllowGet);
            }
            catch  (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            
        }
        #endregion

        #endregion
        // ====== =================== ======
        //      CPK From Excel Section
        // ====== =================== ======
        #region ====== CPK From Excel Section ======
        // ====== Handling requests ======
        public ActionResult CPKExcelIndex()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }                
            return RedirectToAction("Login", "Account");
        }
        // POST: Upload data from excel files to data tranfer object
        #region ====== AJAX Calling Functions ======
        [HttpPost]
        public JsonResult CPKExcelFileUpload()
        {
            List<CPKExcelDTO> lstCPKData = new List<CPKExcelDTO>();
            HttpPostedFileBase fileUpload = Request.Files[0];
            if (fileUpload.ContentLength != 0)
            {
                string fileName = fileUpload.FileName;
                fileName = Path.Combine(Server.MapPath("~/App_Data/" + fileName));
                fileUpload.SaveAs(fileName);
                lstCPKData = GetDataFromExcel(fileName);
            }
            
            return Json(lstCPKData, JsonRequestBehavior.AllowGet);
        }
        // POST: CPK Chart partial view from Excel data
        [HttpPost]
        public ActionResult ExcelCPKChart(string ItemName, double? SpecL, double? SpecH, List<double> Value)
        {
            CPKCalculate cpkResult = GetCPKFromExcelData(ItemName, SpecL, SpecH, Value);
            return PartialView("CPKChartSection", cpkResult);
            //return Json(ItemName, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        // ====== =================== ======
        //         Support Functions
        // ====== =================== ======
        #region ====== Support Functions ======
        // Data process functions
        public CPKCalculate GetDataFromDb(CPKModelStationContent cpkModelStation, int pos)
        {
            //List<CPKRawContent> rawContentList = CPKDataAccess.GetAllCPKDataByModelAndStation(cpkModelStation.ModelName, cpkModelStation.Station).Select(cpk => cpk.ContentList).ToList();
            List<CPKTableDTO> rawContentList = CPKTableDAO.GetAllCPKDataByModelAndStation(cpkModelStation.ModelName, cpkModelStation.Station);
            //rawContentList = CPKTableDAO.SetListCPKRecordContentObjects(rawContentList);
            cpkModelStation = CPKTableDAO.GetModelStationOneContentValue(cpkModelStation, rawContentList, pos);
            CPKCalculate cpkResult = new CPKCalculate
            {
                ItemName = cpkModelStation.ContentGroup[pos].ItemName,
                SpecL = cpkModelStation.ContentGroup[pos].SpecL,
                SpecH = cpkModelStation.ContentGroup[pos].SpecH,
                PcbSNList = cpkModelStation.ContentGroup[pos].PcbSN,
                DataSet = cpkModelStation.ContentGroup[pos].Value,
            };
            
            return cpkResult;
        }
        public CPKCalculate GetCPKWithDate(CPKModelStationContent cpkModelStation, int pos, int pcsNumber, string moNumber, string atePC, DateTime fromDay, DateTime toDay)
        {
            //List<CPKRawContent> rawContentList = CPKDataAccess.GetAllCPKDataByModelAndStation(cpkModelStation.ModelName, cpkModelStation.Station).Select(cpk => cpk.ContentList).ToList();
            List<CPKTableDTO> rawContentList = CPKTableDAO.GetCPKByModelStationDate(cpkModelStation.ModelName, cpkModelStation.Station, pcsNumber, moNumber, atePC, fromDay, toDay);
            //rawContentList = CPKTableDAO.SetListCPKRecordContentObjects(rawContentList);
            cpkModelStation = CPKTableDAO.GetModelStationOneContentValue(cpkModelStation, rawContentList, pos);
            /*CPKCalculate cpkResult = new CPKCalculate
            {
                ItemName = cpkModelStation.ContentGroup[pos].ItemName,
                SpecL = cpkModelStation.ContentGroup[pos].SpecL,
                SpecH = cpkModelStation.ContentGroup[pos].SpecH,
                PcbSNList = cpkModelStation.ContentGroup[pos].PcbSN,
                DataSet = cpkModelStation.ContentGroup[pos].Value,
            };*/

            CPKCalculate cpkResult = cpkModelStation.ContentGroup[pos].CPKResult;

            return cpkResult;
        }
        public CPKCalculate GetDataTestFromTxtFile()
        {
            CPKCalculate cpkData = new CPKCalculate();
            string file = @"E:\04_UBEE_TE_Projects\ATEVersions_Management\ATEVersions_Management\App_Data\testValueCPK.txt";
            List<double> dataSet = new List<double>();

            if (System.IO.File.Exists(file))
            {
                int i = 0;
                foreach (string itm in System.IO.File.ReadAllLines(file))
                {
                    if (i == 0)
                    {
                        cpkData.ItemName = itm;
                        i++;
                        continue;
                    }
                    if (i == 1)
                    {
                        cpkData.SpecL = null;
                        if (!string.IsNullOrEmpty(itm))
                        {
                            cpkData.SpecL = double.Parse(itm);
                        }                        
                        i++;
                        continue;
                    }
                    if (i == 2)
                    {
                        cpkData.SpecH = null;
                        if (!string.IsNullOrEmpty(itm))
                        {
                            cpkData.SpecH = double.Parse(itm);
                        }
                        i++;
                        continue;
                    }
                    if (string.IsNullOrEmpty(itm)) continue;
                    double val = double.Parse(itm);
                    dataSet.Add(val);

                }
            }
            cpkData.DataSet = dataSet;

            return cpkData;
        }
        public List<CPKExcelDTO> GetDataFromExcel(string fileName)
        {
            List<CPKExcelDTO> lstCPKData = new List<CPKExcelDTO>();
            using (ExcelEngine excelEngine =  new ExcelEngine())
            {
                //Initialize Application
                IApplication application = excelEngine.Excel;
                //Set the default application version as Excel 2016
                application.DefaultVersion = ExcelVersion.Excel2016;
                //Open a workbook with a worksheet                
                IWorkbook workbook = application.Workbooks.Open(fileName);
                //Access first worksheet from the workbook instance
                IWorksheet worksheet = workbook.Worksheets[0];
                //Tranfer data to DataTable
                DataTable table = new DataTable();
                table = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
                //Get data to CPK Data object
                int pos = 0;
                foreach (DataColumn dataCol in table.Columns)
                {
                    CPKExcelDTO contentTmp = new CPKExcelDTO
                    {
                        ID = pos,
                        ColName = dataCol.ColumnName.ToString(),
                        RowValues = new List<string>(),
                    };
                    foreach (DataRow dataRow in table.Rows)
                    {
                        contentTmp.RowValues.Add(dataRow[dataCol.ColumnName].ToString());
                    }
                    lstCPKData.Add(contentTmp);
                    pos++;
                }
            }
            System.IO.File.Delete(fileName);
            return lstCPKData;
        }
        public CPKCalculate GetCPKFromExcelData(string ItemName, double? SpecL, double? SpecH, List<double> Value)
        {
            CPKCalculate cpkResult = new CPKCalculate
            {
                ItemName = ItemName,
                SpecL = SpecL,
                SpecH = SpecH,
                PcbSNList = new List<string>(),
                DataSet = Value
            };

            return cpkResult;
        }
        #endregion
    }
}