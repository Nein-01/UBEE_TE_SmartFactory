using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATEVersions_Management.Models.ProductionModels;
using ATEVersions_Management.Models.DTOModels;
using System.Data.SqlClient;
using static System.Net.WebRequestMethods;

namespace ATEVersions_Management.Models.DAOModels
{
    public class FATPTableDAO
    {
        #region Data DB Access
        static readonly ProductionDBContext db = new ProductionDBContext();
        static public FATPTableDTO GetFATPByID(int id)
        {
            try
            {
                using (ProductionDBContext db = new ProductionDBContext())
                {
                    return (from fatp in db.FATP_TABLE
                            where fatp.ID == id
                            orderby fatp.ATE_PC
                            select new FATPTableDTO
                            {
                                ID = fatp.ID,
                                ATE_IP = fatp.ATE_IP,
                                ATE_MAC = fatp.ATE_MAC,
                                ATE_PC = fatp.ATE_PC,
                                STATION = fatp.STATION,
                                MODEL = fatp.MODEL,
                                LINE = fatp.LINE,
                                POST_DATE = fatp.POST_DATE,
                                PC_DATE = fatp.PC_DATE,
                                PASS_NUM = fatp.PASS_NUM,
                                FAIL_NUM = fatp.FAIL_NUM,
                                ERROR_LIST = fatp.ERROR_LIST,
                                COUNTERS = fatp.COUNTERS,
                                EQUIPMENTS = fatp.EQUIPMENTS,
                                CPK_RESULTS = fatp.CPK_RESULTS,
                                CPK_LOWEST_ITEM = fatp.CPK_LOWEST_ITEM,
                                LOSS_MEASUREMENT = fatp.LOSS_MEASUREMENT,
                                OP_ID = fatp.OP_ID,
                                OP_RECORD = fatp.OP_RECORD,
                            }).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        static public List<FATPTableDTO> GetAllTodayFATP()
        {
            DateTime todayBegin = DateTime.Now.Date;                        

            return (from fatp in db.FATP_TABLE
                    where fatp.MODEL.ToLower() != "model" &&
                          !fatp.ATE_IP.ToLower().Contains("null") &&
                          !fatp.ATE_MAC.ToLower().Contains("null") &&
                          fatp.POST_DATE >= todayBegin                         
                    orderby fatp.ATE_PC
                    select new FATPTableDTO
                    {
                        ID = fatp.ID,
                        ATE_IP = fatp.ATE_IP,
                        ATE_MAC = fatp.ATE_MAC,
                        ATE_PC = fatp.ATE_PC,
                        STATION = fatp.STATION,
                        MODEL = fatp.MODEL,
                        LINE = fatp.LINE,
                        POST_DATE = fatp.POST_DATE,
                        PC_DATE = fatp.PC_DATE,
                        PASS_NUM = fatp.PASS_NUM,
                        FAIL_NUM = fatp.FAIL_NUM,
                        ERROR_LIST = fatp.ERROR_LIST,
                        COUNTERS = fatp.COUNTERS,
                        EQUIPMENTS = fatp.EQUIPMENTS,
                        CPK_RESULTS = fatp.CPK_RESULTS,
                        CPK_LOWEST_ITEM = fatp.CPK_LOWEST_ITEM,
                        LOSS_MEASUREMENT = fatp.LOSS_MEASUREMENT,
                    }).ToList();
        }
        static public List<string> GetAllFATPLine()
        {
            return (from fatp in db.FATP_TABLE
                    where fatp.MODEL.ToLower() != "model"
                    group fatp by fatp.LINE into fatpLine
                    orderby fatpLine.Key
                    select fatpLine.Key).ToList();
        }
        static public List<TabFATPLine> GetAllTabFATPLine()
        {
            return (from fatp in db.FATP_TABLE
                    where fatp.MODEL.ToLower() != "model"
                    group fatp by fatp.LINE into fatpLine
                    orderby fatpLine.Key
                    select new TabFATPLine
                    {
                        Line = fatpLine.Key,                        
                    }).ToList();
        }
        static public List<TabFATPLineStation> GetAllTabFATPLineStation()
        {
            return (from fatp in db.FATP_TABLE
                    where fatp.MODEL.ToLower() != "model"
                    group fatp by fatp.LINE into fatpLine
                    orderby fatpLine.Key
                    select new TabFATPLineStation
                    {
                        Line = fatpLine.Key,
                    }).ToList();
        }
        static public List<TabFATPStation> GetAllTabFATPStationByLine(string line)
        {            
            DateTime currentDate = DateTime.Now.Date;
            DateTime momentRange = DateTime.Now.AddMinutes(-15);

            return (from fatp in db.FATP_TABLE
                    where fatp.LINE.ToLower() == line.ToLower() &&                          
                          fatp.POST_DATE >= momentRange
                    group fatp by fatp.STATION into fatpStation
                    orderby fatpStation.Key
                    select new TabFATPStation
                    {
                        Station = fatpStation.Key,
                        ActivePCNum = fatpStation.Count()
                    }).ToList();

            
        }
       
        static public List<string> GetAllFATPStationByLine(string line)
        {
            DateTime todayBegin = DateTime.Now.Date;

            return (from fatp in db.FATP_TABLE
                    where fatp.MODEL.ToLower() != "model" &&                          
                          fatp.LINE.ToLower() == line.ToLower() &&
                          fatp.POST_DATE >= todayBegin
                    group fatp by fatp.STATION into fatpStation
                    orderby fatpStation.Key
                    select fatpStation.Key).ToList();
        }
        static public List<string> GetAllFATPModelByLine(string line)
        {
            DateTime todayBegin = DateTime.Now.Date;

            return (from fatp in db.FATP_TABLE
                    where fatp.MODEL.ToLower() != "model" &&
                          !fatp.ATE_IP.ToLower().Contains("null") &&
                          !fatp.ATE_MAC.ToLower().Contains("null") &&
                          fatp.LINE.ToLower() == line.ToLower() &&
                          fatp.POST_DATE >= todayBegin
                    group fatp by fatp.MODEL into fatpModel
                    orderby fatpModel.Key
                    select fatpModel.Key).ToList();
        }
        static public List<FATPTableDTO> GetListFATPByLineStation(string line, string station)
        {
            DateTime todayBegin = DateTime.Now.Date;

            return (from fatp in db.FATP_TABLE
                    where fatp.MODEL.ToLower() != "model" &&
                          !fatp.ATE_IP.ToLower().Contains("null") &&
                          //!fatp.ATE_MAC.ToLower().Contains("null") &&
                          fatp.LINE.ToLower() == line.ToLower() &&
                          fatp.STATION.ToLower() == station.ToLower() &&
                          fatp.POST_DATE >= todayBegin
                    orderby fatp.ATE_PC
                    select new FATPTableDTO
                    {
                        ID = fatp.ID,
                        ATE_IP = fatp.ATE_IP,
                        ATE_MAC = fatp.ATE_MAC,
                        ATE_PC = fatp.ATE_PC,
                        STATION = fatp.STATION,
                        MODEL = fatp.MODEL,
                        LINE = fatp.LINE,
                        POST_DATE = fatp.POST_DATE,
                        PC_DATE = fatp.PC_DATE,
                        PASS_NUM = fatp.PASS_NUM,
                        FAIL_NUM = fatp.FAIL_NUM,
                        FAIL_NUM_BUFFER = fatp.FAIL_NUM_BUFFER,
                        ERROR_LIST = fatp.ERROR_LIST,
                        COUNTERS = fatp.COUNTERS,
                        EQUIPMENTS = fatp.EQUIPMENTS,
                        CPK_RESULTS = fatp.CPK_RESULTS,
                        CPK_LOWEST_ITEM = fatp.CPK_LOWEST_ITEM,
                        LOSS_MEASUREMENT = fatp.LOSS_MEASUREMENT,
                    }).ToList();
        }

        static public bool SET_ResetFAILNUMbyFAILBUFFER(string atePC, string ateIP, string model)
        {
            try
            {
                using (ProductionDBContext db = new ProductionDBContext())
                {
                    string sqlCommand = "UPDATE FATP_TABLE SET FAIL_NUM_BUFFER = 0 WHERE ATE_PC LIKE '" + atePC + "' AND ATE_IP LIKE '" + ateIP + "' AND MODEL LIKE '" + model + "'";
                    db.Database.ExecuteSqlCommand(sqlCommand);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Data Process
        static public List<FATPTableDTO> GetFATPIdentifiedAbnormal(List<FATPTableDTO> ListFATP)
        {
            

            int listSize = ListFATP.Count;

            for(int i = 0; i < listSize; i++)
            {
                int PC_Count = ListFATP.Where(fatp => fatp.ATE_PC == ListFATP[i].ATE_PC).Count();
                if( PC_Count > 1)
                {
                    ListFATP[i].AbnormalPC = true;
                }
                int IP_Count = ListFATP.Where(fatp => fatp.ATE_IP == ListFATP[i].ATE_IP).Count();
                if (IP_Count > 1)
                {
                    ListFATP[i].AbnormalIP = true;
                }
                int MAC_Count = ListFATP.Where(fatp => fatp.ATE_MAC == ListFATP[i].ATE_MAC).Count();
                if (MAC_Count > 1)
                {
                    ListFATP[i].AbnormalMAC = true;
                }
            }

            /*if(ListFATP.Where(fatp => fatp.AbnormalIP || fatp.AbnormalMAC || fatp.AbnormalPC).Count() > 0)
            {*/
                ListFATP = ListFATP.Where(fatp => fatp.TimeSpanDataSent < 300 && !(fatp.AbnormalPC && fatp.AbnormalIP && fatp.AbnormalMAC)).ToList();
                listSize = ListFATP.Count;
                for (int i = 0; i < listSize; i++)
                {
                    if (ListFATP[i].AbnormalPC || ListFATP[i].AbnormalMAC || ListFATP[i].AbnormalIP)
                    {
                        ListFATP[i].AbnormalPC = false;
                        ListFATP[i].AbnormalIP = false;
                        ListFATP[i].AbnormalMAC = false;
                    }
                }
            //}

            return ListFATP;
        }
        static public List<FATPTableDTO> GetFATPLockedStatus(List<FATPTableDTO> ListFATP)
        {
            int listSize = ListFATP.Count;
            /*for (int i = 0; i < listSize; i++)
            {
                ListFATP
            }*/
            return ListFATP;
        }
        #endregion
    }
}