using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using ATEVersions_Management.Models.ProductionModels;
using ATEVersions_Management.Models.DTOModels;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Data.Entity.Infrastructure;
using System.Web.Razor;
using System.Data.SqlClient;
using ATEVersions_Management.Models.ATEVersionModels;

namespace ATEVersions_Management.Models.DAOModels
{    
    public class TETestDataDAO
    {
        static readonly ProductionDBContext db = new ProductionDBContext();
        #region Test time to calculate equipment section
        // ====== ====================================== ======
        //       Test time to calculate equipment section
        // ====== ====================================== ======
        // ====== ====================================== ======
        //          Get data directly from database
        // ====== ====================================== ======

        static public List<TETestDataDTO> GetAllTETestData()
        {
            return (from te in db.TE_TEST_DATA
                    where te.STATUS == 1 &&
                          (te.START_TIME.HasValue &&                          
                          te.END_TIME.HasValue &&
                          te.START_TIME.Value.Year == te.END_TIME.Value.Year ||
                          te.START_TIME.Value.Year == DateTime.Now.Year)
                    orderby te.END_TIME descending
                    select new TETestDataDTO
                    {
                        ID = te.ID,
                        LINE = te.LINE,
                        MODEL = te.MODEL,
                        STATION = te.STATION,
                        WORK_DATE = te.WORK_DATE,
                        WORK_SECTION = te.WORK_SECTION,
                        START_TIME = te.START_TIME,
                        END_TIME = te.END_TIME,
                        ELAPSE_TIME = te.ELAPSE_TIME,
                    }).Take(10000).ToList();
        }
        static public List<TETestDataDTO> GetTETestDataByLineModelStation(string line, string model, string station, DateTime fromDate, DateTime toDate)
        {
            return (from te in db.TE_TEST_DATA
                    where te.STATUS == 1 &&
                          te.MODEL.ToLower() != "model" &&
                          !(string.IsNullOrEmpty(te.SN) &&
                          string.IsNullOrEmpty(te.BOARD_SN)) &&
                          (te.START_TIME.HasValue &&
                          te.END_TIME.HasValue &&
                          te.END_TIME.Value.Year >= 2000 &&
                          te.START_TIME.Value.Year >= 2000) &&
                          te.END_TIME.Value >= fromDate &&
                          te.END_TIME.Value <= toDate &&
                          (te.LINE.ToLower() == line.ToLower() &&
                          te.MODEL.ToLower() == model.ToLower() &&
                          te.STATION.ToLower() == station.ToLower())
                    orderby te.END_TIME descending
                    select new TETestDataDTO
                    {
                        ID = te.ID,
                        LINE = te.LINE,
                        MODEL = te.MODEL,
                        STATION = te.STATION,
                        WORK_DATE = te.WORK_DATE,
                        WORK_SECTION = te.WORK_SECTION,
                        START_TIME = te.START_TIME,
                        END_TIME = te.END_TIME,
                        ELAPSE_TIME = te.ELAPSE_TIME,
                    }).ToList();
        }
        static public List<string> GetListLines()
        {
            return (from te in db.TE_TEST_DATA
                    group te by te.LINE into teLines
                    select teLines.Key).ToList();
        }
        static public List<TETestDataEquipmentEstimate> GetModelStationByLine(DateTime fromDate, DateTime toDate)
        {
            return (from te in db.TE_TEST_DATA
                    where te.STATUS == 1 &&
                          te.MODEL.ToLower() != "model" &&
                          !(string.IsNullOrEmpty(te.SN) &&
                          string.IsNullOrEmpty(te.BOARD_SN)) &&
                          (te.START_TIME.HasValue &&
                          te.END_TIME.HasValue &&
                          te.END_TIME.Value.Year >= 2000 &&
                          te.START_TIME.Value.Year >= 2000) &&
                          te.END_TIME.Value >= fromDate &&
                          te.END_TIME.Value <= toDate
                    group te by new { te.LINE, te.MODEL, te.STATION } into teModelStation
                    orderby teModelStation.Key.LINE, teModelStation.Key.MODEL, teModelStation.Key.STATION
                    select new TETestDataEquipmentEstimate
                    {
                        Line = teModelStation.Key.LINE,
                        Model = teModelStation.Key.MODEL,
                        Station = teModelStation.Key.STATION,
                        FromDate = fromDate,
                        ToDate = toDate,
                        PcsTotal = teModelStation.Count(),
                        TotalTime = (double)(teModelStation.Sum(te => te.ELAPSE_TIME + 5.0)),
                    }).ToList();
        }
        #endregion

        #region Test time by machine and date section

        // ========= Get ATE Time data from database =========

        static public List<TETestDataATETime> GetATETimeOfMachineByModel(string model, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (ProductionDBContext db = new ProductionDBContext())
                {
                    return (from ateTime in db.TE_TEST_DATA
                            where ateTime.STATUS == 1 &&
                                  ateTime.MODEL.ToLower() == model.ToLower() &&
                                  ateTime.MODEL.ToLower() != "model" &&
                                  !(string.IsNullOrEmpty(ateTime.SN) &&
                                  string.IsNullOrEmpty(ateTime.BOARD_SN)) &&
                                  (ateTime.START_TIME.HasValue &&
                                  ateTime.END_TIME.HasValue &&
                                  ateTime.END_TIME.Value.Year >= 2000 &&
                                  ateTime.START_TIME.Value.Year >= 2000) &&
                                  ateTime.END_TIME.Value >= fromDate &&
                                  ateTime.END_TIME.Value <= toDate
                            group ateTime by new { ateTime.MODEL, ateTime.STATION, ateTime.ATE } into grpATETime
                            orderby grpATETime.Key.MODEL, grpATETime.Key.STATION, grpATETime.Key.ATE
                            select new TETestDataATETime
                            {
                                Model = grpATETime.Key.MODEL,
                                Station = grpATETime.Key.STATION,
                                ATEMachine = grpATETime.Key.ATE,
                                PCS = grpATETime.Count(),
                                MeanTime = Math.Round(grpATETime.Average(ate => ate.ELAPSE_TIME.Value), 2),
                                MinTime = grpATETime.Min(ate => ate.ELAPSE_TIME.Value),
                                MaxTime = grpATETime.Max(ate => ate.ELAPSE_TIME.Value),
                            }
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        static public List<TETestDataATETime> GetATETimeOfMachineWithGroupDate(string model, string station, string ateMachine, DateTime fromDate, DateTime toDate)
        {
            //toDate = toDate.AddDays(-1);
            fromDate = toDate.AddDays(-6);

            return (from ateTime in db.TE_TEST_DATA
                    where ateTime.STATUS == 1 &&
                          ateTime.ATE.ToLower() == ateMachine.ToLower() &&
                          ateTime.STATION.ToLower() == station.ToLower() &&
                          ateTime.MODEL.ToLower() == model.ToLower() &&
                          ateTime.MODEL.ToLower() != "model" &&
                          !(string.IsNullOrEmpty(ateTime.SN) &&
                          string.IsNullOrEmpty(ateTime.BOARD_SN)) &&
                          (ateTime.START_TIME.HasValue &&
                          ateTime.END_TIME.HasValue &&
                          ateTime.END_TIME.Value.Year >= 2000 &&
                          ateTime.START_TIME.Value.Year >= 2000) &&
                          ateTime.END_TIME.Value >= fromDate &&
                          ateTime.END_TIME.Value <= toDate
                    group ateTime by new { ateTime.WORK_DATE, ateTime.MODEL, ateTime.STATION, ateTime.ATE } into grpATETime
                    orderby grpATETime.Key.WORK_DATE, grpATETime.Key.MODEL, grpATETime.Key.STATION, grpATETime.Key.ATE
                    select new TETestDataATETime
                    {
                        WorkDate = grpATETime.Key.WORK_DATE,
                        Model = grpATETime.Key.MODEL,
                        Station = grpATETime.Key.STATION,
                        ATEMachine = grpATETime.Key.ATE,
                        PCS = grpATETime.Count(),
                        MeanTime = Math.Round(grpATETime.Average(ate => ate.ELAPSE_TIME.Value), 2),
                        MinTime = grpATETime.Min(ate => ate.ELAPSE_TIME.Value),
                        MaxTime = grpATETime.Max(ate => ate.ELAPSE_TIME.Value),
                    }
                    ).ToList();
        }
        static public List<string> GetATETimeListModel(DateTime? fromDate, DateTime? toDate)
        {
            /*return (from ateTime in db.TE_TEST_DATA
            where ateTime.STATUS == 1 &&
                          ateTime.MODEL.ToLower() != "model"                          
                    group ateTime by  ateTime.MODEL into grpModel
                    orderby grpModel.Key
                    select grpModel.Key).ToList();*/

            try
            {
                using (ProductionDBContext db = new ProductionDBContext())
                {
                    string sqlCommand = "SELECT MODEL FROM TE_TEST_DATA WHERE STATUS = 1 AND MODEL NOT LIKE 'Model' GROUP BY MODEL";
                    if(fromDate.HasValue || toDate.HasValue)
                    {
                        sqlCommand = "SELECT MODEL FROM TE_TEST_DATA WHERE STATUS = 1 AND MODEL NOT LIKE 'Model' AND END_TIME >= '" + fromDate + "' AND END_TIME <= '" + toDate + "' GROUP BY MODEL";
                    }
                    List<string> listATEModel = db.Database.SqlQuery<string>(sqlCommand).ToList();
                    return listATEModel;
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }

        }
        static public List<string> GetListStationByModel(string model, DateTime fromDate, DateTime toDate)
        {            
            try
            {
                using (ProductionDBContext db = new ProductionDBContext())
                {
                    return (from ateTime in db.TE_TEST_DATA
                            where ateTime.STATUS == 1 &&
                                  ateTime.MODEL.ToLower() == model.ToLower() &&
                                  ateTime.MODEL.ToLower() != "model" &&
                                  (ateTime.START_TIME.HasValue &&
                                  ateTime.END_TIME.HasValue &&
                                  ateTime.END_TIME.Value.Year >= 2000 &&
                                  ateTime.START_TIME.Value.Year >= 2000) &&
                                  ateTime.END_TIME.Value >= fromDate &&
                                  ateTime.END_TIME.Value <= toDate
                            group ateTime by new { ateTime.MODEL, ateTime.STATION } into grpATETime
                            orderby grpATETime.Key.MODEL, grpATETime.Key.STATION
                            select grpATETime.Key.STATION).ToList();
                }
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }
        static public List<TETestDataDTO> GetATETimeRecordByModel(string model, DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (ProductionDBContext db = new ProductionDBContext())
                {

                    return (from ateTime in db.TE_TEST_DATA
                            where ateTime.STATUS == 1 &&
                                  ateTime.MODEL.ToUpper() == model.ToUpper() &&
                                  ateTime.MODEL.ToLower() != "model" &&
                                  !(string.IsNullOrEmpty(ateTime.SN) &&
                                  string.IsNullOrEmpty(ateTime.BOARD_SN)) &&
                                  (ateTime.START_TIME.HasValue &&
                                  ateTime.END_TIME.HasValue &&
                                  ateTime.END_TIME.Value.Year >= 2000 &&
                                  ateTime.START_TIME.Value.Year >= 2000) &&
                                  ateTime.END_TIME.Value >= fromDate &&
                                  ateTime.END_TIME.Value <= toDate
                            orderby ateTime.START_TIME ascending
                            select new TETestDataDTO
                            {
                                MODEL = ateTime.MODEL,
                                STATION = ateTime.STATION,
                                ATE = ateTime.ATE,
                                WORK_DATE = ateTime.WORK_DATE,
                                ELAPSE_TIME = ateTime.ELAPSE_TIME,
                                START_TIME = ateTime.START_TIME,
                                END_TIME = ateTime.END_TIME
                            }).ToList();

                    /*string sqlCommand = "SELECT * FROM TE_TEST_DATA WHERE STATUS = 1 AND MODEL LIKE '" + model + "' AND END_TIME >= '" + fromDate + "' AND END_TIME <= '" + toDate + "' ORDER BY START_TIME ASC";
                    return db.Database.SqlQuery<TETestDataDTO>(sqlCommand).ToList();*/

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ========= ATE Time data processing functions =========
        static public List<TETestDataATETime> Func_GET_ATETimeOfMachine(string model, DateTime fromDate, DateTime toDate)
        {
            List<TETestDataATETime> listATETimeOfMachine = new List<TETestDataATETime>();
            try
            {
                List<TETestDataDTO> listATETimeRecord = GetATETimeRecordByModel(model, fromDate, toDate);
                //List<string> listATEMachine = listATETimeRecord.GroupBy(atetime => atetime.ATE).Select(atetime => atetime.Key).ToList();
                
                listATETimeOfMachine = (from ateTime in listATETimeRecord                                        
                                        group ateTime by new { ateTime.MODEL, ateTime.STATION, ateTime.ATE } into grpATETime
                                        orderby grpATETime.Key.MODEL, grpATETime.Key.STATION, grpATETime.Key.ATE
                                        select new TETestDataATETime
                                        {
                                            Model = grpATETime.Key.MODEL,
                                            Station = grpATETime.Key.STATION,
                                            ATEMachine = grpATETime.Key.ATE,
                                            PCS = grpATETime.Count(),
                                            MeanTime = Math.Round(grpATETime.Average(ate => ate.ELAPSE_TIME.Value), 2),
                                            MinTime = grpATETime.Min(ate => ate.ELAPSE_TIME.Value),
                                            MaxTime = grpATETime.Max(ate => ate.ELAPSE_TIME.Value),
                                        }).ToList();

                //
                int ilistATETimeOfMachineSize = listATETimeOfMachine.Count;
                for (int i = 0; i < ilistATETimeOfMachineSize; i++)
                {
                    listATETimeOfMachine[i] = Func_GET_ATELoadTimeMachineFromRawList(listATETimeRecord, listATETimeOfMachine[i]);
                }
                return listATETimeOfMachine;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        static private TETestDataATETime Func_GET_ATELoadTimeMachineFromRawList(List<TETestDataDTO> listTETestRawData, TETestDataATETime ateMachineDetail)
        {
            
            // Data processing from raw list            
            int iRawListSize = listTETestRawData.Count;

            TETestDataDTO tempATETimeDTO = new TETestDataDTO();
            double dLoadTime = 0;
            int iLoadCount = 0;

            for (int i = 0; i < iRawListSize; i++)
            {
                
                if (listTETestRawData[i].MODEL.Equals(ateMachineDetail.Model) && 
                    listTETestRawData[i].STATION.Equals(ateMachineDetail.Station) &&
                    listTETestRawData[i].ATE.Equals(ateMachineDetail.ATEMachine))
                {
                    if(iLoadCount != 0)
                    {
                        double tmpLoadTime = (listTETestRawData[i].START_TIME.Value - tempATETimeDTO.END_TIME.Value).TotalSeconds;
                        if(tmpLoadTime < 0)
                        {
                            tmpLoadTime *= -1;
                        }
                        if (tmpLoadTime >= 300)
                        {
                            dLoadTime += 15;
                        }
                        else
                        {
                            dLoadTime += tmpLoadTime;
                        }
                        
                        tempATETimeDTO = listTETestRawData[i];
                        iLoadCount++;                                                
                    }
                    if(iLoadCount == 0)
                    {
                        tempATETimeDTO = listTETestRawData[i];
                        iLoadCount++;
                    }
                    
                }
            }

            ateMachineDetail.LoadTime = Math.Round(dLoadTime/iLoadCount, 2);

            return ateMachineDetail;
        }

        #endregion

        #region On line version program checking
        // ====== ====================================== ======
        //          Get data directly from database
        // ====== ====================================== ======
        static public string GetInUseVersion(string model)
        {
            DateTime fromDate = DateTime.Now.AddDays(-7);
            DateTime toDate = DateTime.Now;
            return (from ate in db.TE_TEST_DATA
                    where ate.STATUS == 1 &&
                          ate.MODEL.ToLower() == model.ToLower() &&
                          ate.MODEL.ToLower() != "model" &&
                          !(string.IsNullOrEmpty(ate.TEST_VERSION) &&
                          string.IsNullOrEmpty(ate.SN) &&
                          string.IsNullOrEmpty(ate.BOARD_SN)) &&
                          (ate.START_TIME.HasValue &&
                          ate.END_TIME.HasValue &&
                          ate.END_TIME.Value.Year >= 2000 &&
                          ate.START_TIME.Value.Year >= 2000) &&
                          ate.END_TIME.Value >= fromDate &&
                          ate.END_TIME.Value <= toDate
                    orderby ate.END_TIME descending
                    select ate.TEST_VERSION).Take(1).SingleOrDefault();
            

        }
        static public List<TETestDataDTO> GetListVersionOfModel()
        {
            return (from ate in db.TE_TEST_DATA
                    where ate.STATUS == 1 &&
                          ate.MODEL.ToLower() != "model" &&
                          !string.IsNullOrEmpty(ate.TEST_VERSION)                                              
                    group ate by new { ate.MODEL, ate.TEST_VERSION} into grpAte
                    orderby grpAte.Key.MODEL, grpAte.Key.TEST_VERSION ascending
                    select new TETestDataDTO
                    {
                        MODEL = grpAte.Key.MODEL,
                        TEST_VERSION = grpAte.Key.TEST_VERSION,
                    }).ToList();
        }
        static public List<VersionLatestDTO> GetListLatestVersionOnline()
        {
            try
            {
                string sqlCommand = "SELECT MODEL AS MODELNAME , TEST_VERSION AS VersionOnline FROM TE_TEST_DATA WHERE MODEL != 'Model' AND MODEL LIKE '%%' AND TEST_VERSION LIKE '%%' AND LEN(TEST_VERSION) > 0 GROUP BY MODEL, TEST_VERSION ORDER BY MODEL DESC, CONVERT(FLOAT, SUBSTRING(TEST_VERSION,4,LEN(TEST_VERSION))) DESC";
                using (ProductionDBContext db = new ProductionDBContext())
                {
                    return db.Database.SqlQuery<VersionLatestDTO>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // ====== ====================================== ======
        //              Process data from DTOs
        // ====== ====================================== ======
        static public Dictionary<string, string> DictionaryLastestVersionOfModel()
        {
            List<TETestDataDTO> listModelVersion = GetListVersionOfModel();
            Dictionary<string, string> listLatestVersionModel = new Dictionary<string, string>();

            foreach(var modelVer in listModelVersion)
            {
                if (listLatestVersionModel.ContainsKey(modelVer.MODEL))
                {
                    listLatestVersionModel[modelVer.MODEL] = modelVer.TEST_VERSION;
                    continue;
                }
                listLatestVersionModel.Add(modelVer.MODEL, modelVer.TEST_VERSION);                
            }

            return listLatestVersionModel;
        }
        static public Dictionary<string, string> DictionaryLastestVersionOnline()
        {
            List<VersionLatestDTO> listModelVersion = GetListLatestVersionOnline();
            Dictionary<string, string> dictLatestVersionOnline = new Dictionary<string, string>();

            foreach (var modelVer in listModelVersion)
            {
                if (dictLatestVersionOnline.ContainsKey(modelVer.ModelName))
                {                    
                    continue;
                }
                dictLatestVersionOnline.Add(modelVer.ModelName, modelVer.VersionOnline);
            }

            return dictLatestVersionOnline;
        }
        #endregion
    }
}