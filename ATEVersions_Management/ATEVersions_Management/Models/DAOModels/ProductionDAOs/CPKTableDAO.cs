using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using ATEVersions_Management.Models.CPKModels;
using ATEVersions_Management.Models.DTOModels;
using Microsoft.Ajax.Utilities;

namespace ATEVersions_Management.Models.DAOModels
{
    public class CPKTableDAO
    {
        static readonly CPKDbContext db = new CPKDbContext();
        // ====== ====================================== ======
        //          Get data directly from database
        // ====== ====================================== ======
        #region CPKSytem DB Data Queries
        static public List<CPKTableDTO> GetAllCPKData()
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    return (from cpk in db.CPK_TABLE
                            where cpk.STATUS == "PASS"
                            orderby cpk.DATE_TIME descending
                            select new CPKTableDTO
                            {
                                ID = cpk.ID,
                                MO_NUMBER = cpk.MO_NUMBER,
                                MODEL_NAME = cpk.MODEL_NAME,
                                STATION_NAME = cpk.STATION_NAME,
                                PCB_SN = cpk.PCB_SN,
                                SSN = cpk.SSN,
                                MAC = cpk.MAC,
                                DATE_TIME = cpk.DATE_TIME,
                                STATUS = cpk.STATUS,
                                CONTENT = cpk.CONTENT,
                            }).Take(120).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }        
        static public List<string> GetAllCPKModel()
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    return (from cpk in db.CPK_TABLE
                            group cpk by cpk.MODEL_NAME into cpkModel
                            orderby cpkModel.Key
                            select cpkModel.Key).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        static public List<string> GET_TodayCPKModel(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    string sqlCommand = "SELECT MODEL_NAME FROM CPK_TABLE WHERE DATE_TIME >= '"+ fromDate + "' AND DATE_TIME <= '" + toDate + "' GROUP BY MODEL_NAME ORDER BY MODEL_NAME";
                    return db.Database.SqlQuery<string>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<string> GetAllCPKStation(string model)
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    return (from cpk in db.CPK_TABLE
                            where cpk.MODEL_NAME.ToLower() == model.ToLower()
                            group cpk by cpk.STATION_NAME into cpkModel
                            orderby cpkModel.Key
                            select cpkModel.Key).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        static public List<CPKTableDTO> GetAllCPKDataByModelAndStation(string model, string station)
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    return (from cpk in db.CPK_TABLE
                            where cpk.STATUS == "PASS" &&
                                  cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                                  cpk.STATION_NAME.ToLower() == station.ToLower() &&
                                  !cpk.CONTENT.Contains("[(null)]")
                            orderby cpk.DATE_TIME descending
                            select new CPKTableDTO
                            {
                                ID = cpk.ID,
                                MO_NUMBER = cpk.MO_NUMBER,
                                MODEL_NAME = cpk.MODEL_NAME,
                                STATION_NAME = cpk.STATION_NAME,
                                PCB_SN = cpk.PCB_SN,
                                SSN = cpk.SSN,
                                MAC = cpk.MAC,
                                DATE_TIME = cpk.DATE_TIME,
                                STATUS = cpk.STATUS,
                                CONTENT = cpk.CONTENT,
                            }
                ).DistinctBy(cpk => cpk.PCB_SN).Take(1500).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }       
        static public List<string> GetListMOByModel(string model, DateTime fromDay, DateTime toDay)
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    return (from cpk in db.CPK_TABLE
                            where cpk.STATUS == "PASS" &&
                                  cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                                  !string.IsNullOrEmpty(cpk.PCB_SN) &&
                                  cpk.DATE_TIME >= fromDay &&
                                  cpk.DATE_TIME <= toDay
                            orderby cpk.DATE_TIME descending
                            group cpk by cpk.MO_NUMBER into cpkMO
                            select cpkMO.Key).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        static public async Task<List<string>> GetTaskListMOByModel(string model, DateTime fromDay, DateTime toDay)
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    return await Task.Run(async () => {
                        return await (from cpk in db.CPK_TABLE
                                      where cpk.STATUS == "PASS" &&
                                            cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                                            !string.IsNullOrEmpty(cpk.PCB_SN) &&
                                            cpk.DATE_TIME >= fromDay &&
                                            cpk.DATE_TIME <= toDay
                                      orderby cpk.DATE_TIME descending
                                      group cpk by cpk.MO_NUMBER into cpkMO
                                      select cpkMO.Key).ToListAsync();
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            
        }
        static public List<string> GetListPCByModelStation(string model, string station, DateTime fromDay, DateTime toDay)
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    return (from cpk in db.CPK_TABLE
                            where cpk.STATUS == "PASS" &&
                                  cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                                  cpk.STATION_NAME.ToLower() == station.ToLower() &&
                                  !string.IsNullOrEmpty(cpk.PCB_SN) &&
                                  !string.IsNullOrEmpty(cpk.ATE_PC) &&
                                  cpk.DATE_TIME >= fromDay &&
                                  cpk.DATE_TIME <= toDay
                            group cpk by cpk.ATE_PC into cpkATEPC
                            orderby cpkATEPC.Key ascending
                            select cpkATEPC.Key).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        static public async Task<List<string>> GetTaskListPCByModelStation(string model, string station, DateTime fromDay, DateTime toDay)
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    return await Task.Run(async () =>
                    {
                        return await (from cpk in db.CPK_TABLE
                                      where cpk.STATUS == "PASS" &&
                                            cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                                            cpk.STATION_NAME.ToLower() == station.ToLower() &&
                                            !string.IsNullOrEmpty(cpk.PCB_SN) &&
                                            !string.IsNullOrEmpty(cpk.ATE_PC) &&
                                            cpk.DATE_TIME >= fromDay &&
                                            cpk.DATE_TIME <= toDay
                                      group cpk by cpk.ATE_PC into cpkATEPC
                                      orderby cpkATEPC.Key ascending
                                      select cpkATEPC.Key).ToListAsync();
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            
        }
        static public List<CPKTableDTO> GetCPKByModelStationDate(string model, string station, int pcsNumber, string moNumber, string atePC, DateTime fromDay, DateTime toDay)
        {
            try
            {
                pcsNumber = (pcsNumber == 0) ? 100000 : pcsNumber;
                using (CPKDbContext db = new CPKDbContext())
                {                    
                    if (moNumber != "0" && atePC != "0")
                    {
                        return SetListCPKRecordContentObjects((from cpk in db.CPK_TABLE
                                where cpk.STATUS == "PASS" &&
                                      cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                                      cpk.STATION_NAME.ToLower() == station.ToLower() &&
                                      cpk.MO_NUMBER.ToLower() == moNumber.ToLower() &&
                                      cpk.ATE_PC.ToLower() == atePC.ToLower() &&
                                      !cpk.CONTENT.Contains("[(null)]") &&
                                      !string.IsNullOrEmpty(cpk.PCB_SN) &&
                                      cpk.DATE_TIME >= fromDay &&
                                      cpk.DATE_TIME <= toDay
                                orderby cpk.DATE_TIME descending
                                select new CPKTableDTO
                                {
                                    ID = cpk.ID,
                                    MO_NUMBER = cpk.MO_NUMBER,
                                    MODEL_NAME = cpk.MODEL_NAME,
                                    STATION_NAME = cpk.STATION_NAME,
                                    PCB_SN = cpk.PCB_SN,
                                    SSN = cpk.SSN,
                                    MAC = cpk.MAC,
                                    DATE_TIME = cpk.DATE_TIME,
                                    STATUS = cpk.STATUS,
                                    CONTENT = cpk.CONTENT,
                                }
                        ).DistinctBy(cpk => cpk.PCB_SN).Take(pcsNumber).ToList());
                    }
                    if (moNumber != "0")
                    {
                        return SetListCPKRecordContentObjects((from cpk in db.CPK_TABLE
                                where cpk.STATUS == "PASS" &&
                                      cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                                      cpk.STATION_NAME.ToLower() == station.ToLower() &&
                                      cpk.MO_NUMBER.ToLower() == moNumber.ToLower() &&
                                      !cpk.CONTENT.Contains("[(null)]") &&
                                      !string.IsNullOrEmpty(cpk.PCB_SN) &&
                                      cpk.DATE_TIME >= fromDay &&
                                      cpk.DATE_TIME <= toDay
                                orderby cpk.DATE_TIME descending
                                select new CPKTableDTO
                                {
                                    ID = cpk.ID,
                                    MO_NUMBER = cpk.MO_NUMBER,
                                    MODEL_NAME = cpk.MODEL_NAME,
                                    STATION_NAME = cpk.STATION_NAME,
                                    PCB_SN = cpk.PCB_SN,
                                    SSN = cpk.SSN,
                                    MAC = cpk.MAC,
                                    DATE_TIME = cpk.DATE_TIME,
                                    STATUS = cpk.STATUS,
                                    CONTENT = cpk.CONTENT,
                                }
                        ).DistinctBy(cpk => cpk.PCB_SN).Take(pcsNumber).ToList());
                    }
                    if (atePC != "0")
                    {
                        return SetListCPKRecordContentObjects((from cpk in db.CPK_TABLE
                                where cpk.STATUS == "PASS" &&
                                      cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                                      cpk.STATION_NAME.ToLower() == station.ToLower() &&
                                      cpk.ATE_PC.ToLower() == atePC.ToLower() &&
                                      !cpk.CONTENT.Contains("[(null)]") &&
                                      !string.IsNullOrEmpty(cpk.PCB_SN) &&
                                      cpk.DATE_TIME >= fromDay &&
                                      cpk.DATE_TIME <= toDay
                                orderby cpk.DATE_TIME descending
                                select new CPKTableDTO
                                {
                                    ID = cpk.ID,
                                    MO_NUMBER = cpk.MO_NUMBER,
                                    MODEL_NAME = cpk.MODEL_NAME,
                                    STATION_NAME = cpk.STATION_NAME,
                                    PCB_SN = cpk.PCB_SN,
                                    SSN = cpk.SSN,
                                    MAC = cpk.MAC,
                                    DATE_TIME = cpk.DATE_TIME,
                                    STATUS = cpk.STATUS,
                                    CONTENT = cpk.CONTENT,
                                }
                        ).DistinctBy(cpk => cpk.PCB_SN).Take(pcsNumber).ToList());
                    }
                    return SetListCPKRecordContentObjects((from cpk in db.CPK_TABLE
                            where cpk.STATUS == "PASS" &&
                                  cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                                  cpk.STATION_NAME.ToLower() == station.ToLower() &&
                                  !cpk.CONTENT.Contains("[(null)]") &&
                                  !string.IsNullOrEmpty(cpk.PCB_SN) &&
                                  cpk.DATE_TIME >= fromDay &&
                                  cpk.DATE_TIME <= toDay
                            orderby cpk.DATE_TIME descending
                            select new CPKTableDTO
                            {
                                ID = cpk.ID,
                                MO_NUMBER = cpk.MO_NUMBER,
                                MODEL_NAME = cpk.MODEL_NAME,
                                STATION_NAME = cpk.STATION_NAME,
                                PCB_SN = cpk.PCB_SN,
                                SSN = cpk.SSN,
                                MAC = cpk.MAC,
                                DATE_TIME = cpk.DATE_TIME,
                                STATUS = cpk.STATUS,
                                CONTENT = cpk.CONTENT,
                            }
                        ).DistinctBy(cpk => cpk.PCB_SN).Take(pcsNumber).ToList());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        static public List<CPKRawContent> GetCPKRawContentByModelAndStation(string model, string station)
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    return (from cpk in db.CPK_TABLE
                            where cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                                  cpk.STATION_NAME.ToLower() == station.ToLower() &&
                                  cpk.STATUS == "PASS"
                            orderby cpk.DATE_TIME descending
                            select new CPKTableDTO
                            {
                                ID = cpk.ID,
                                MO_NUMBER = cpk.MO_NUMBER,
                                MODEL_NAME = cpk.MODEL_NAME,
                                STATION_NAME = cpk.STATION_NAME,
                                PCB_SN = cpk.PCB_SN,
                                SSN = cpk.SSN,
                                MAC = cpk.MAC,
                                DATE_TIME = cpk.DATE_TIME,
                                STATUS = cpk.STATUS,
                                CONTENT = cpk.CONTENT,
                            }
                ).Select(cpk => cpk.RawContents).Take(120).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        static public CPKTableDTO GetOneCPKDataByModelAndStation(string model, string station)
        {
           /* return (from cpk in db.CPK_TABLE
                    where cpk.STATUS == "PASS" &&
                          !cpk.CONTENT.Contains("[(null)]") &&
                          !string.IsNullOrEmpty(cpk.PCB_SN) &&
                          cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                          cpk.STATION_NAME.ToLower() == station.ToLower()                          
                    orderby cpk.DATE_TIME descending
                    select new CPKTableDTO
                    {
                        ID = cpk.ID,
                        MO_NUMBER = cpk.MO_NUMBER,
                        MODEL_NAME = cpk.MODEL_NAME,
                        STATION_NAME = cpk.STATION_NAME,
                        PCB_SN = cpk.PCB_SN,
                        SSN = cpk.SSN,
                        MAC = cpk.MAC,
                        DATE_TIME = cpk.DATE_TIME,
                        STATUS = cpk.STATUS,
                        CONTENT = cpk.CONTENT,
                    }
                ).Take(1).SingleOrDefault();*/
            using(CPKDbContext db = new CPKDbContext())
            {
                try
                {
                    string sqlCommand = "SELECT TOP 1 * FROM CPK_TABLE WHERE STATUS LIKE 'PASS' AND CONTENT != '[(null)]' AND LEN(PCB_SN) > 0 AND MODEL_NAME LIKE '"+model+"' AND STATION_NAME LIKE '"+station+"' ORDER BY DATE_TIME DESC";
                    CPKTableDTO cpkDataResult = db.Database.SqlQuery<CPKTableDTO>(sqlCommand).SingleOrDefault();
                    cpkDataResult.CPKContentObjects = GetCPKRecordContentObjectList(cpkDataResult.RawContents);
                    return cpkDataResult;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        static public CPKTableDTO GetOneCPKDataBySN(string model, string station, string pcbSn)
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    return (from cpk in db.CPK_TABLE
                            where cpk.STATUS == "PASS" &&
                                  cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                                  cpk.STATION_NAME.ToLower() == station.ToLower() &&
                                  cpk.PCB_SN.ToLower() == pcbSn.ToLower()
                            orderby cpk.DATE_TIME descending
                            select new CPKTableDTO
                            {
                                ID = cpk.ID,
                                MO_NUMBER = cpk.MO_NUMBER,
                                MODEL_NAME = cpk.MODEL_NAME,
                                STATION_NAME = cpk.STATION_NAME,
                                PCB_SN = cpk.PCB_SN,
                                SSN = cpk.SSN,
                                MAC = cpk.MAC,
                                DATE_TIME = cpk.DATE_TIME,
                                STATUS = cpk.STATUS,
                                CONTENT = cpk.CONTENT,
                            }
                ).Take(1).SingleOrDefault();
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            
        }
        static public List<CPKModelStationContent> GetListCPKModelStation()
        {
            try
            {
                using (CPKDbContext db = new CPKDbContext())
                {
                    return (from cpk in db.CPK_TABLE                            
                            group cpk by new { cpk.MODEL_NAME, cpk.STATION_NAME } into cpkModelStation                            
                            select new CPKModelStationContent
                            {
                                ModelName = cpkModelStation.Key.MODEL_NAME,
                                Station = cpkModelStation.Key.STATION_NAME,
                            }
                   ).Distinct().OrderBy(cpk => cpk.ModelName).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        #endregion
        // ====== ====================================== ======
        // Process data from database to list of CPKDTO objects        
        // ====== ====================================== ======
        #region CPKSystem Process DB Data To DTO Model
        
        // ========= Get CPK value to model station =========
        static public CPKModelStationContent GetModelStationContent(CPKTableDTO oneCPKRecord)
        {
            CPKModelStationContent ModelStationContent = new CPKModelStationContent
            {
                ModelName = oneCPKRecord.MODEL_NAME,
                Station = oneCPKRecord.STATION_NAME,
                ListPCBSN = new List<string>(),
            };

            int contentSize = 0;
            if (oneCPKRecord.CPKContentObjects != null)
            {
                contentSize = oneCPKRecord.CPKContentObjects.Count;
            }
            List<CPKContentInvidual> listContentInvidual = new List<CPKContentInvidual>();
            for (int i = 0; i < contentSize; i++)
            {
                CPKContentInvidual tmpInvidual = new CPKContentInvidual
                {
                    ContentID = i,
                    ItemName = oneCPKRecord.CPKContentObjects[i].Name,
                    SpecL = oneCPKRecord.CPKContentObjects[i].SpecL,
                    SpecH = oneCPKRecord.CPKContentObjects[i].SpecH,
                    PcbSN = new List<string>(),
                    Value = new List<double>()
                };
                listContentInvidual.Add(tmpInvidual);
            }
            ModelStationContent.ContentGroup = listContentInvidual;
            return ModelStationContent;
        }
        static public CPKModelStationContent GetModelStationOneContentValue(CPKModelStationContent ModelStationContent, List<CPKTableDTO> rawContentList, int pos)
        {
            
            foreach (CPKTableDTO item in rawContentList)
            {                
                ModelStationContent.ListPCBSN.Add(item.PCB_SN);
                try
                {
                    ModelStationContent.ContentGroup[pos].Value.Add(item.CPKContentObjects[pos].Value);
                }catch(Exception ex)
                {
                    ModelStationContent.ContentGroup[pos].Value.Add(-1.12345);
                }
                    
            }
            ModelStationContent.ContentGroup[pos].PcbSN = ModelStationContent.ListPCBSN;
            return ModelStationContent;
        }
        static public CPKModelStationContent GetModelStationFullContentValue(CPKModelStationContent ModelStationContent, List<CPKTableDTO> rawCPKContentList)
        {
            int GetPCBSNOnce = 0;
            if(rawCPKContentList.Count > 0)
            {
                for (int i = 0; i < ModelStationContent.ContentGroup.Count; i++)
                {
                    foreach (CPKTableDTO item in rawCPKContentList)
                    {
                        if (GetPCBSNOnce < 1)
                        {
                            ModelStationContent.ListPCBSN.Add(item.PCB_SN);
                        }                        
                        try
                        {
                            ModelStationContent.ContentGroup[i].Value.Add(item.CPKContentObjects[i].Value);
                        }
                        catch (Exception ex)
                        {
                            ModelStationContent.ContentGroup[i].Value.Add(-1.12345);
                        }

                    }
                    ModelStationContent.ContentGroup[i].PcbSN = ModelStationContent.ListPCBSN;
                    GetPCBSNOnce++;
                }
            }            
            
            return ModelStationContent;
        }
        static public List<CPKTableDTO> SetListCPKRecordContentObjects(List<CPKTableDTO> rawContentList)
        {
            int listSize = rawContentList.Count;

            for (int i = 0; i < listSize; i++)
            {
                rawContentList[i].CPKContentObjects = GetCPKRecordContentObjectList(rawContentList[i].RawContents);
            }

            return rawContentList;
        }
        static public List<CPKContentObject> GetCPKRecordContentObjectList(CPKRawContent rawContent)
        {
            try
            {
                //rawContent = rawContent ?? new CPKRawContent();
                List<CPKContentObject> contentObjects = new List<CPKContentObject>();
                int rawContentSize = rawContent.name.Count;
                for (int i = 0; i < rawContentSize; i++)
                {
                    CPKContentObject contentObjectTmp = new CPKContentObject
                    {
                        Name = rawContent.name[i],
                        Value = rawContent.value[i],
                        SpecL = rawContent.specL[i],
                        SpecH = rawContent.specH[i]
                    };
                    if (contentObjects.Contains(contentObjectTmp))
                    {
                        int idx = contentObjects.IndexOf(contentObjectTmp);
                        contentObjects[idx] = contentObjectTmp;
                        continue;
                    }
                    contentObjects.Add(contentObjectTmp);
                }

                return contentObjects;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        
        // ========= API get data to send to Fii =========
        static public List<Ubee_CPKData> GET_UbeeCPKData(CPKModelStationContent cpkModelStation)
        {
            //
            List<Ubee_CPKData> listUbeeCPKData = new List<Ubee_CPKData>();
            int CPKItemCount = cpkModelStation.ContentGroup.Count;

            //
            for(int i = 0; i < CPKItemCount; i++)
            {
                Ubee_CPKData new_Ubee_CPKData = new Ubee_CPKData()
                {
                    itemTest = cpkModelStation.ContentGroup[i].ItemName,
                    usl = cpkModelStation.ContentGroup[i].SpecH,
                    lsl = cpkModelStation.ContentGroup[i].SpecL,
                    valueTest = cpkModelStation.ContentGroup[i].Value.ToArray(),
                };
                listUbeeCPKData.Add(new_Ubee_CPKData);
            }
            //
            return listUbeeCPKData;
        }
        #endregion

        // ====== ====================================== ======
        //          Get data directly from database
        // ====== ====================================== ======
        #region GRRSystem DB Data Queries
        static public List<string> GetListModel()
        {
            return (from GRR in db.CPK_TABLE
                    group GRR by GRR.MODEL_NAME into grpGRR
                    orderby grpGRR.Key
                    select grpGRR.Key).ToList();
        }
        static public List<string> GetListStationByModel(string model)
        {
            return (from GRR in db.CPK_TABLE
                    where GRR.MODEL_NAME.ToLower() == model.ToLower()
                    group GRR by GRR.STATION_NAME into grpGRR
                    orderby grpGRR.Key
                    select grpGRR.Key).ToList();
        }
        static public bool IsGRRPartExist()
        {
            return true;
        }
        static public List<CPKTableDTO> GetRandomOperSamples(string model, string station, int pcsNumber, DateTime fromDay, DateTime toDay)
        {            

            /*return (from cpk in db.CPK_TABLE
                    where cpk.STATUS == "PASS" &&
                          cpk.MODEL_NAME.ToLower() == model.ToLower() &&
                          cpk.STATION_NAME.ToLower() == station.ToLower() &&                          
                          !cpk.CONTENT.Contains("[(null)]") &&
                          !string.IsNullOrEmpty(cpk.PCB_SN) &&
                          cpk.DATE_TIME >= fromDay &&
                          cpk.DATE_TIME <= toDay
                    select new CPKTableDTO
                    {
                        ID = cpk.ID,
                        MO_NUMBER = cpk.MO_NUMBER,
                        MODEL_NAME = cpk.MODEL_NAME,
                        STATION_NAME = cpk.STATION_NAME,
                        PCB_SN = cpk.PCB_SN,
                        SSN = cpk.SSN,
                        MAC = cpk.MAC,
                        DATE_TIME = cpk.DATE_TIME,
                        STATUS = cpk.STATUS,
                        CONTENT = cpk.CONTENT,
                    }
                ).DistinctBy(cpk => cpk.PCB_SN)
                 .OrderByDescending(cpk => Guid.NewGuid())
                 .Take(pcsNumber).ToList();*/

            using (CPKDbContext db = new CPKDbContext())
            {
                try
                {
                    string strFromDay = fromDay.ToString("yyyy-MM-dd HH:mm:ss"),
                           strToDay = toDay.ToString("yyyy-MM-dd HH:mm:ss");

                    string sqlCommand = "SELECT TOP "+pcsNumber+" * FROM CPK_TABLE WHERE STATUS LIKE '%PASS%' AND CONTENT != '[(null)]' AND LEN(PCB_SN) > 0 AND MODEL_NAME LIKE '%"+model+"%' AND STATION_NAME LIKE '%"+station+"%' AND DATE_TIME >= '"+ strFromDay+"' AND DATE_TIME <= '"+ strToDay + "' ORDER BY NEWID()";
                    return SetListCPKRecordContentObjects(db.Database.SqlQuery<CPKTableDTO>(sqlCommand).ToList());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        #endregion
        // ====== ====================================== ======
        // Process data from database to list of GRR objects        
        // ====== ====================================== ======
        #region Process DB Data To DTO Model
        static public GRRModelStationContent GetGRRModelStationContent(CPKTableDTO oneCPKRecord)
        {
            GRRModelStationContent ModelStationContent = new GRRModelStationContent
            {
                ModelName = oneCPKRecord.MODEL_NAME,
                Station = oneCPKRecord.STATION_NAME,                
            };

            int contentSize = 0;
            if (oneCPKRecord.CPKContentObjects != null)
            {
                contentSize = oneCPKRecord.CPKContentObjects.Count;
            }
            List<GRRContentInvidual> listContentInvidual = new List<GRRContentInvidual>();
            for (int i = 0; i < contentSize; i++)
            {
                GRRContentInvidual tmpInvidual = new GRRContentInvidual
                {
                    ContentID = i,
                    ItemName = oneCPKRecord.CPKContentObjects[i].Name,
                    SpecL = oneCPKRecord.CPKContentObjects[i].SpecL,
                    SpecH = oneCPKRecord.CPKContentObjects[i].SpecH,                    
                    Value = new List<double>()
                };
                listContentInvidual.Add(tmpInvidual);
            }
            ModelStationContent.GRRContentGroup = listContentInvidual;
            return ModelStationContent;
        }
        static public double[] GetArrSamples(List<CPKTableDTO> rawContentList, int pos, int pcsNum)
        {            
            double[] arrSamples = new double[pcsNum];

            for (int i = 0; i < pcsNum; i++)
            {                
                try
                {
                    arrSamples[i] = rawContentList[i].CPKContentObjects[pos].Value;
                }
                catch (Exception ex)
                {
                    arrSamples[i] = -1.12345;
                }

            }

            return arrSamples;
        }
        static public List<double[]> DivideSamplesIntoList(double[] arrSamples, int pcsNum)
        {
            List<double[]> listArrSamples = new List<double[]>();
            int sampleSize = arrSamples.Length;
            double[] tmpDoubles = new double[pcsNum];
            int tmpCount = 0;
            for (int i = 0; i < sampleSize; i++)
            {
                tmpDoubles[tmpCount] = arrSamples[i];
                tmpCount++;
                if ((i + 1) % pcsNum == 0)
                {
                    //tmpDoubles = FlattenArrSamples(tmpDoubles,pcsNum);
                    listArrSamples.Add(tmpDoubles);
                    tmpDoubles = new double[pcsNum];
                    tmpCount = 0;
                }
            }

            return listArrSamples;
        }
        
        // === Modified Input Samples Section ===
        static public GRRModifiedOperSample GET_ModifiedOperSample(int operNo, string operName, List<int> listPosMark, double[] arrSamples, double? lsl, double? usl)
        {
            // Process Input Variables
            if (!lsl.HasValue || double.IsNaN(lsl.Value))
            {
                lsl = arrSamples.Min();
            }
            if (!usl.HasValue || double.IsNaN(usl.Value))
            {
                usl = arrSamples.Max();
            }
            listPosMark = listPosMark ?? new List<int>();
            int floatingPointCount = GetSampleFloatingPointNumber(arrSamples);
            // Initialize Neccessary Variables
            Random rand = new Random();
            List<double[]> listSample = new List<double[]>();           
            double boundRange = usl.Value - lsl.Value;
            double rangeLow = boundRange * 0.3,
                   rangeMid = boundRange * 0.5,
                   rangeHigh = boundRange * 0.75;           
            int lowCount = 0, midCount = 0, highCount = 0;
            int maxLow = 6, maxMid = 3, maxHigh = 1;
            // 
            if (listPosMark.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    int rangePicker = listPosMark[i];
                    if (rangePicker == 0)
                    {
                        if (lowCount == maxLow)
                        {
                            i--;
                            continue;
                        }
                        listSample.Add(new double[] { Math.Round((rand.NextDouble() * rangeLow) + lsl.Value, floatingPointCount), Math.Round((rand.NextDouble() * rangeLow) + lsl.Value, floatingPointCount), Math.Round((rand.NextDouble() * rangeLow) + lsl.Value, floatingPointCount) });
                        lowCount++;
                    }
                    if (rangePicker == 1)
                    {
                        if (midCount == maxMid)
                        {
                            i--;
                            continue;
                        }
                        listSample.Add(new double[] { Math.Round((rand.NextDouble() * rangeMid) + lsl.Value + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeMid) + lsl.Value + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeMid) + lsl.Value + rangeLow, floatingPointCount) });
                        midCount++;
                    }
                    if (rangePicker == 2)
                    {
                        if (highCount == maxHigh)
                        {
                            i--;
                            continue;
                        }
                        listSample.Add(new double[] { Math.Round((rand.NextDouble() * rangeHigh) + lsl.Value + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeHigh) + lsl.Value + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeHigh) + lsl.Value + rangeLow, floatingPointCount) });
                        highCount++;
                    }

                }
            }
            if (listPosMark.Count < 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    int rangePicker = rand.Next(0, 3);
                    if (rangePicker == 0)
                    {
                        if (lowCount == maxLow)
                        {
                            i--;
                            continue;
                        }
                        listSample.Add(new double[] { Math.Round((rand.NextDouble() * rangeLow) + lsl.Value, floatingPointCount), Math.Round((rand.NextDouble() * rangeLow) + lsl.Value, floatingPointCount), Math.Round((rand.NextDouble() * rangeLow) + lsl.Value, floatingPointCount) });
                        listPosMark.Add(0);
                        lowCount++;
                    }
                    if (rangePicker == 1)
                    {
                        if (midCount == maxMid)
                        {
                            i--;
                            continue;
                        }
                        listSample.Add(new double[] { Math.Round((rand.NextDouble() * rangeMid) + lsl.Value + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeMid) + lsl.Value + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeMid) + lsl.Value + rangeLow, floatingPointCount) });
                        listPosMark.Add(1);
                        midCount++;
                    }
                    if (rangePicker == 2)
                    {
                        if (highCount == maxHigh)
                        {
                            i--;
                            continue;
                        }
                        listSample.Add(new double[] { Math.Round((rand.NextDouble() * rangeHigh) + lsl.Value + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeHigh) + lsl.Value + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeHigh) + lsl.Value + rangeLow, floatingPointCount) });
                        listPosMark.Add(2);
                        highCount++;
                    }
                }
            }
            
            //
            OBJ_OperTestResult operSamplesResult = new OBJ_OperTestResult
            {
                ID = operNo,
                OperName = operName,
                OperSamples = listSample,
            };

            return new GRRModifiedOperSample()
            {
                OperSamplesResult = operSamplesResult,
                ListPosMark = listPosMark,
            };
        }
        static public List<double[]> GETGRRModifiedSamples(double[] arrSamples, double? lsl, double? usl)
        {

            if (!lsl.HasValue || double.IsNaN(lsl.Value))
            {
                lsl = arrSamples.Min();
            }
            if (!usl.HasValue || double.IsNaN(usl.Value))
            {
                usl = arrSamples.Max();
            }

            return CreateRandomSampleWithModifiedRange(GetSampleFloatingPointNumber(arrSamples), lsl.Value, usl.Value);
        }
        static private List<double[]> CreateRandomSampleWithModifiedRange(int floatingPointCount,double lsl, double usl)
        {
            Random rand = new Random();
            List<double[]> listSample = new List<double[]>();
            //
            double boundRange = usl - lsl;
            double rangeLow = boundRange * 0.25,
                   rangeMid = boundRange * 0.5,
                   rangeHigh = boundRange * 0.75;
            //
            int lowCount = 0, midCount = 0, highCount = 0;
            
                for (int i = 0; i < 10; i++)
                {
                    int rangePicker = rand.Next(0, 3);
                    if (rangePicker == 0)
                    {
                        if (lowCount == 7)
                        {
                            i--;
                            continue;
                        }
                        listSample.Add(new double[] { Math.Round((rand.NextDouble() * rangeLow) + lsl, floatingPointCount), Math.Round((rand.NextDouble() * rangeLow) + lsl, floatingPointCount), Math.Round((rand.NextDouble() * rangeLow) + lsl, floatingPointCount) });
                        
                        lowCount++;
                    }
                    if (rangePicker == 1)
                    {
                        if (midCount == 2)
                        {
                            i--;
                            continue;
                        }
                        listSample.Add(new double[] { Math.Round((rand.NextDouble() * rangeMid) + lsl + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeMid) + lsl + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeMid) + lsl + rangeLow, floatingPointCount) });
                        
                        midCount++;
                    }
                    if (rangePicker == 2)
                    {
                        if (highCount == 1)
                        {
                            i--;
                            continue;
                        }
                        listSample.Add(new double[] { Math.Round((rand.NextDouble() * rangeHigh) + lsl + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeHigh) + lsl + rangeLow, floatingPointCount), Math.Round((rand.NextDouble() * rangeHigh) + lsl + rangeLow, floatingPointCount) });
                        
                        highCount++;
                    }  
                }           
            

            return listSample;
        }
        static private int GetSampleFloatingPointNumber(double[] arrSamples)
        {
            int arrSize = arrSamples.Length;
            
            for(int i=0; i<arrSize; i++) 
            {
                if ((arrSamples[i] - Math.Truncate(arrSamples[i])) != 0)
                {
                    return 2;
                }
            }
            return 0;
        }
        #endregion

    }
}