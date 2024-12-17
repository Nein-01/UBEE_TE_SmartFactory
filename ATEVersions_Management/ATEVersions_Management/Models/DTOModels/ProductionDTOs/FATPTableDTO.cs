using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ATEVersions_Management.Models.DAOModels;
using System.Threading.Tasks;

namespace ATEVersions_Management.Models.DTOModels
{
    // ========= Data Tranfer For FATP Table =========
    public class FATPTableDTO
    {
        [DisplayName("FATP_ID")]
        public int ID { get; set; }

        [DisplayName("ATE_PC")]
        [StringLength(50)]
        public string ATE_PC { get; set; }

        [DisplayName("ATE_IP")]
        [StringLength(50)]
        public string ATE_IP { get; set; }

        [DisplayName("ATE_MAC")]
        [StringLength(50)]
        public string ATE_MAC { get; set; }

        [DisplayName("Line")]
        [StringLength(50)]
        public string LINE { get; set; }

        [DisplayName("Station")]
        [StringLength(50)]
        public string STATION { get; set; }

        [DisplayName("Model")]
        [StringLength(50)]
        public string MODEL { get; set; }

        [DisplayName("Updated at")]
        public DateTime? POST_DATE { get; set; }

        [DisplayName("PASS")]
        public int? PASS_NUM { get; set; }

        [DisplayName("FAIL")]
        public int? FAIL_NUM { get; set; }
        public int? FAIL_NUM_BUFFER { get; set; }

        [DisplayName("Error Code List")]
        public string ERROR_LIST { get; set; }

        [DisplayName("Counters")]
        public string COUNTERS { get; set; }

        [DisplayName("Equipments")]
        public string EQUIPMENTS { get; set; }

        [DisplayName("PC Date")]
        public DateTime? PC_DATE { get; set; }

        [DisplayName("CPK Results")]
        public string CPK_RESULTS { get; set; }

        [DisplayName("CPK Lowest Item")]
        public string CPK_LOWEST_ITEM { get; set; }

        [DisplayName("Loss Measurement")]
        public string LOSS_MEASUREMENT { get; set; }
        public string OP_ID { get; set; }
        public string OP_RECORD { get; set; }
        public List<DEJSON_COUNTERS> DE_COUNTERS { 
            get 
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<DEJSON_COUNTERS>>(COUNTERS);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return new List<DEJSON_COUNTERS>();
                }                
            } 
            set { } 
        }
        public List<DEJSON_EQUIPMENTS> DE_EQUIPMENTS
        {
            get
            {
                try
                {                    
                    if (EQUIPMENTS == "NULL")
                    {                                               
                        return new List<DEJSON_EQUIPMENTS>();
                    }
                    return JsonConvert.DeserializeObject<List<DEJSON_EQUIPMENTS>>(EQUIPMENTS);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return new List<DEJSON_EQUIPMENTS>();
                }
                
            }
            set { }
        }
        // ====== FATP Additional Data ======
        [DisplayName("Usage")]
        public double TimeSpanDataSent
        {
            get
            {
                DateTime timeNow = DateTime.Now;
                TimeSpan timeSpanDataSent = timeNow - POST_DATE.Value;                
                return Math.Round(timeSpanDataSent.TotalMinutes,2);
            }
        }
        public double TimeSpanNotSync
        {
            get
            {
                if (PC_DATE.HasValue)
                {
                    TimeSpan timeSpanNotSync = POST_DATE.Value - PC_DATE.Value;
                    return Math.Round(timeSpanNotSync.TotalMinutes, 2);
                }
                return 0;
            }
        }
        public bool AbnormalPC { get; set; }
        public bool AbnormalIP { get; set; }
        public bool AbnormalMAC { get; set; }
        public double TotalTestCount
        {
            get
            {
                return PASS_NUM.Value + FAIL_NUM.Value;
            }
        }
        public double FailRate
        {
            get
            {
                return (TotalTestCount == 0) ? 0 : Math.Round((FAIL_NUM.Value / TotalTestCount) * 100, 2);
            }
        }
        public double PassRate
        {
            get
            {
                return (TotalTestCount == 0) ? 0 : Math.Round((PASS_NUM.Value / TotalTestCount) * 100, 2);
            }
        }
        public string StatusMessage
        {
            get
            {
                return GetAbnormalMessage();
            }
        }
        public string LastestResult 
        {
            get
            {
                return ErrorListLastResult();
            }
        }
        /*public bool? LockedStatus
        {
            get
            {
                // Task<bool?> tskGETStatusFunc = TestMonitorDBDAO.GetStationStatusByMachineModel(ATE_PC, MODEL);
                Task.Run(async () => {
                    return await TestMonitorDBDAO.GetStationStatusByMachineModel(ATE_PC, MODEL); 
                });
                return false;
            }
        }*/
        public StationInforDTO LockInfo
        {
            get
            {
                try
                {
                    StationInforDTO _lockInfo = Task.Run(async () => {
                        return await StationInforDAO.GetStationInfoByMachineModel(ATE_PC, MODEL);
                    }).Result;
                    if (_lockInfo == null)
                    {
                        _lockInfo = new StationInforDTO();
                        StationInforDAO.InsertNewPCLockRecord(ATE_PC, MODEL);
                    }
                    return _lockInfo;
                }
                catch (Exception ex)
                {
                    return new StationInforDTO();
                }                
            }
        }
        public Dictionary<string, int> ErrorStatistic
        {
            get
            {
                return (from error in ErrorStatisticFunc(ERROR_LIST)
                       orderby error.Value descending
                       select error).Take(3).ToDictionary(error => error.Key, error => error.Value);
            }
        }
        public string[] CpkLowestItem
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(this.CPK_LOWEST_ITEM))
                    {
                        return null;
                    }
                    return this.CPK_LOWEST_ITEM.Split(',');
                }catch (Exception ex)
                {
                    return null;
                }
                
            }
        }
        public Dictionary<string,string> CpkResults
        { 
            get 
            {
                if (string.IsNullOrEmpty(this.CPK_RESULTS))
                {
                    return new Dictionary<string, string>();
                }
                return CpkResultFunc(this.CPK_RESULTS);
            }
            set { } 
        }
        // FATP Additional Functions
        private string ErrorListLastResult()
        {
            string[] SplittedErrorList = ERROR_LIST.Split('\n');
            string ErrorListLastLine = SplittedErrorList[SplittedErrorList.Length-1];
            string[] SplittedLastLine = ErrorListLastLine.Split(' ');
            string LastStatus = SplittedLastLine[SplittedLastLine.Length-1];
            return LastStatus;
        }
        private Dictionary<string, int> ErrorStatisticFunc(string ERROR_LIST)
        {
            Dictionary<string, int> DicErrorStatistic = new Dictionary<string, int>();
            try
            {
                string[] SplittedErrorList = ERROR_LIST.Split('\n');
                int errorListNum = SplittedErrorList.Length;
                for (int i = 0; i < errorListNum; i++)
                {
                    string[] errorLine = SplittedErrorList[i].Split(' ');
                    string errorCode = errorLine[errorLine.Length - 1].Trim();
                    if (errorCode.ToLower() == "pass")
                    {
                        continue;
                    }
                    int errorRepeat = 1;
                    if (DicErrorStatistic.ContainsKey(errorCode))
                    {
                        errorRepeat = DicErrorStatistic[errorCode] + 1;
                        DicErrorStatistic[errorCode] = errorRepeat;
                        continue;
                    }
                    DicErrorStatistic.Add(errorCode, errorRepeat);

                }

                return DicErrorStatistic;
            }
            catch(Exception ex) 
            { 
                return DicErrorStatistic; 
            }
            
        }
        private Dictionary<string, string> CpkResultFunc(string CPK_RESULTS)
        {
            Dictionary<string,string> dicCPKResults = new Dictionary<string,string>();

            string[] listLineCPKResult = CPK_RESULTS.Trim().Split('\n');
            int cpkResultNum = listLineCPKResult.Length;
            for(int i = 2; i < cpkResultNum; i++)
            {
                string[] splittedLineCPK = listLineCPKResult[i].Trim().Split(',');
                dicCPKResults.Add(splittedLineCPK[0], splittedLineCPK[1]);
            }

            return dicCPKResults;
        }
        private string GetAbnormalMessage()
        {
            string result = "";

            if(AbnormalPC || AbnormalIP || AbnormalMAC)
            {
                result += "- PC, IP or MAC are duplicated.\n";
            }
            if(FailRate >= 20)
            {
                result += "- Fail rate is over 20%.\n";
            }
            if(DE_COUNTERS.Where(counter => counter.IsAbnormal == true).Count() > 0)
            {
                result += "- Counters usage are over limit.\n";
            }
            if(TimeSpanDataSent >= 120)
            {
                result += "- No data updated after 2 hours.\n";
            }
            if(TimeSpanNotSync >= 5 || TimeSpanNotSync < -5)
            {
                result += "- Time on server and on line is not matched.\n";
            }
            return result;
        }
        
    }
    public class DEJSON_COUNTERS
    {
        public int CounterNo { get; set; }
        public string CounterName { get; set; }
        public int CounterNum { get; set; }
        public bool IsAbnormal 
        { 
            get { return CheckAbnormal(); } 
        }
        private bool CheckAbnormal()
        {
            switch (CounterName.Trim())
            {
                case "PON Cable":
                case "Fiber Count":
                    return (CounterNum >= 50);
                case "RF Cable":
                case "RF Probe Count":                    
                case "ANT Cable":                    
                case "POWER Cable":                    
                case "Power Count":                    
                case "USB Cable":                    
                case "USB Count":                    
                case "Ethernet Cable":                    
                case "RJ45 Count":                    
                case "Voice Cable":                    
                case "Console Cable":                    
                case "Console Count":                    
                case "PIN Count":
                    return (CounterNum >= 3000);
            }
            return false;
        }
    }
    public class DEJSON_EQUIPMENTS
    {
        public int EquipNo { get; set; }
        public string EquipName { get; set; }
        public string EquipSN { get; set; }
    }

    // ========= Data Tranfer For Web View =========
    public class TabFATPLine
    {
        public string Line { get; set; }
        public int StationNum 
        { 
            get { return FATPTableDAO.GetAllFATPStationByLine(Line).Count(); }
            set { } 
        }
    }

    public class TabFATPLineStation
    {
        public string Line { get; set; }
        public List<TabFATPStation> ListStation { 
            get
            {
                return FATPTableDAO.GetAllTabFATPStationByLine(this.Line);
            }
            set { } 
        }
        public int IsLineActive
        {
            get
            {
                if (this.ListStation.Sum(station => station.ActivePCNum) == 0) 
                {
                    return 1;
                }
                return 0;
            }
        }
    }

    public class TabFATPStation
    {
        public string Station { get; set; }
        public int ActivePCNum { get; set; }
    }
}

