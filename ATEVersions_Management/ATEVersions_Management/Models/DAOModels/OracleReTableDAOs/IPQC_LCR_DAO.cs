using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ATEVersions_Management.Models.DTOModels.OracleReTableDTOs;
using ATEVersions_Management.Models.HelperModels;
namespace ATEVersions_Management.Models.DAOModels.OracleReTableDAOs
{
    public class IPQC_LCR_DAO
    {
        static private OracleConnector oraConn = new OracleConnector();
        static public LCRWorkShiftDTO GetLCRShiftData(string shift, string fromDate, string toDate)
        {
            string sqlCommand = "SELECT TO_CHAR(DATETIME,'yyyy-mm-dd') AS WORKDAY, COUNT(SN) AS PCS FROM IPQC_LCR WHERE (TO_CHAR(DATETIME,'yyyy-mm-dd') >= '"+fromDate+"' AND TO_CHAR(DATETIME,'yyyy-mm-dd') <= '"+toDate+ "')  AND (TO_CHAR(DATETIME,'yyyy-mm-dd hh24:mi:ss') >= TO_CHAR(TO_TIMESTAMP(TO_CHAR(DATETIME,'yyyy-mm-dd')||'07:30:00','yyyy-mm-dd hh24:mi:ss'),'yyyy-mm-dd hh24:mi:ss') AND      TO_CHAR(DATETIME,'yyyy-mm-dd hh24:mi:ss') <= TO_CHAR(TO_TIMESTAMP(TO_CHAR(DATETIME,'yyyy-mm-dd')||'19:30:00','yyyy-mm-dd hh24:mi:ss'),'yyyy-mm-dd hh24:mi:ss')) GROUP BY TO_CHAR(DATETIME,'yyyy-mm-dd') ORDER BY TO_CHAR(DATETIME,'yyyy-mm-dd') ASC";
            //
            if (shift == "night") 
            {
                //sqlCommand = "SELECT TO_CHAR(DATETIME,'yyyy-mm-dd') AS WORKDAY, COUNT(SN) AS PCS FROM IPQC_LCR WHERE (TO_CHAR(DATETIME,'yyyy-mm-dd') >= '"+fromDate+"' AND TO_CHAR(DATETIME,'yyyy-mm-dd') <= '"+toDate+ "')  AND ((TO_CHAR(DATETIME,'yyyy-mm-dd hh24:mi:ss') >= TO_CHAR(TO_TIMESTAMP(TO_CHAR(DATETIME,'yyyy-mm-dd')||'19:30:00','yyyy-mm-dd hh24:mi:ss'),'yyyy-mm-dd hh24:mi:ss') AND      TO_CHAR(DATETIME,'yyyy-mm-dd hh24:mi:ss') <= TO_CHAR(TO_TIMESTAMP(TO_CHAR(DATETIME,'yyyy-mm-dd')||'23:59:00','yyyy-mm-dd hh24:mi:ss'),'yyyy-mm-dd hh24:mi:ss')) OR  (TO_CHAR(DATETIME+1,'yyyy-mm-dd hh24:mi:ss') >= TO_CHAR(TO_TIMESTAMP(TO_CHAR(DATETIME+1,'yyyy-mm-dd')||'00:00:00','yyyy-mm-dd hh24:mi:ss'),'yyyy-mm-dd hh24:mi:ss') AND      TO_CHAR(DATETIME+1,'yyyy-mm-dd hh24:mi:ss') <= TO_CHAR(TO_TIMESTAMP(TO_CHAR(DATETIME+1,'yyyy-mm-dd')||'07:30:00','yyyy-mm-dd hh24:mi:ss'),'yyyy-mm-dd hh24:mi:ss')))      GROUP BY TO_CHAR(DATETIME,'yyyy-mm-dd') ORDER BY TO_CHAR(DATETIME,'yyyy-mm-dd') ASC";                
                sqlCommand = "SELECT WORKDATE AS WORKDAY, PCS FROM TABLE(funcGetNightShiftData(DATE '"+ fromDate + "', DATE '"+ toDate + "'))";           
            }
            try
            {
                DataTable dtLCRShiftData = oraConn.ExecSqlQuery(sqlCommand);
                List<string> listWorkDay = new List<string>();
                List<double> listPCS = new List<double>();
                foreach (DataRow dr in dtLCRShiftData.Rows)
                {
                    listWorkDay.Add(dr["WORKDAY"].ToString());
                    listPCS.Add(double.Parse(dr["PCS"].ToString()));
                }
                return new LCRWorkShiftDTO
                {
                    ListWorkDay = listWorkDay,
                    ListPCS = listPCS
                };
            }
            catch(Exception ex)
            {
                throw ex;
                //return new OBAWorkShiftDTO();
            }            
            
        }
        static public DataTable GetLCRDataInTimeRange(string fromDate, string toDate)
        {
            string sqlCommand = "SELECT SN,CUST_PN,DATECODE,VENDOR,VENDORNO,LOCATION,QUANTY,REMAINQTY,MATERIALTYPE,DESCRIPTION,MARKING,LOWSPEC,HIGHSPEC,MEASUREVALUE, STATUS,DATETIME,EMPLOYEE,IDMERTERIAL FROM IPQC_LCR WHERE (TO_CHAR(DATETIME,'yyyy-mm-dd') >= '" + fromDate + "' AND TO_CHAR(DATETIME,'yyyy-mm-dd') <= '" + toDate + "')  ORDER BY DATETIME DESC";

            try
            {
                DataTable dtblLCRData = oraConn.ExecSqlQuery(sqlCommand);
                /*
                 * AND STATUS LIKE '%PASS%'
                 * List<IPQC_LCR_DTO> listLCRData = new List<IPQC_LCR_DTO>();
                foreach (DataRow dr in dtblLCRData.Rows)
                {
                    listLCRData.Add(new IPQC_LCR_DTO
                    {
                        SN = dr["SN"].ToString(),
                        CUST_PN = dr["CUST_PN"].ToString(),
                        DATECODE = dr["DATECODE"].ToString(),
                        VENDOR = dr["VENDOR"].ToString(),
                        VENDORNO = dr["VENDORNO"].ToString(),
                        LOCATION = dr["LOCATION"].ToString(),
                        QUANTITY = dr["QUANTY"].ToString(),
                        REMAINQTY = dr["REMAINQTY"].ToString(),
                        MATERIALTYPE = dr["MATERIALTYPE"].ToString(),
                        DESCRIPTION = dr["DESCRIPTION"].ToString(),
                        LOWSPEC = dr["LOWSPEC"].ToString(),
                        HIGHSPEC = dr["HIGHSPEC"].ToString(),
                        MEASUREVALUE = dr["MEASUREVALUE"].ToString(),
                        STATUS = dr["STATUS"].ToString(),
                        DATETIME = dr["DATETIME"].ToString(),
                        EMPLOYEE = dr["EMPLOYEE"].ToString(),
                        MARKING = dr["MARKING"].ToString(),
                        IDMATERIAL = dr["IDMERTERIAL"].ToString(),
                    });
                }*/
                return dtblLCRData;
            }
            catch (Exception ex)
            {
                throw ex;                
            }
        }
        static public List<IPQC_LCR_DTO> GetLCRDataByMaterialId(string materialId)
        {
            string sqlCommand = "SELECT SN,CUST_PN,DATECODE,VENDOR,VENDORNO,LOCATION,QUANTY,REMAINQTY,MATERIALTYPE,DESCRIPTION,MARKING,LOWSPEC,HIGHSPEC,MEASUREVALUE, STATUS,DATETIME,EMPLOYEE,IDMERTERIAL FROM IPQC_LCR WHERE IDMERTERIAL LIKE '%" + materialId + "%' AND STATUS LIKE '%PASS%'";

            try
            {
                DataTable LCRMaterialData = oraConn.ExecSqlQuery(sqlCommand);
                List<IPQC_LCR_DTO> listLCRData = new List<IPQC_LCR_DTO>();
                foreach (DataRow dr in LCRMaterialData.Rows)
                {
                    listLCRData.Add(new IPQC_LCR_DTO
                    {
                        SN = dr["SN"].ToString(),
                        CUST_PN = dr["CUST_PN"].ToString(),
                        DATECODE = dr["DATECODE"].ToString(),
                        VENDOR = dr["VENDOR"].ToString(),
                        VENDORNO = dr["VENDORNO"].ToString(),
                        LOCATION = dr["LOCATION"].ToString(),
                        QUANTITY = dr["QUANTY"].ToString(),
                        REMAINQTY = dr["REMAINQTY"].ToString(),
                        MATERIALTYPE = dr["MATERIALTYPE"].ToString(),
                        DESCRIPTION = dr["DESCRIPTION"].ToString(),
                        LOWSPEC = dr["LOWSPEC"].ToString(),
                        HIGHSPEC = dr["HIGHSPEC"].ToString(),
                        MEASUREVALUE = dr["MEASUREVALUE"].ToString(),
                        STATUS = dr["STATUS"].ToString(),
                        DATETIME = dr["DATETIME"].ToString(),
                        EMPLOYEE = dr["EMPLOYEE"].ToString(),
                        MARKING = dr["MARKING"].ToString(),
                        IDMATERIAL = dr["IDMERTERIAL"].ToString(),
                    });
                }
                return listLCRData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}