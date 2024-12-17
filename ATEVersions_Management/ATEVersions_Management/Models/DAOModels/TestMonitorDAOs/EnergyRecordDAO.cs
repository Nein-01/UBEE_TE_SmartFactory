using ATEVersions_Management.Models.DTOModels.TestMonitorDTOs;
using ATEVersions_Management.Models.TestMonitorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DAOModels.TestMonitorDAOs
{
    public class EnergyRecordDAO
    {
        static private readonly TestMonitorDBContext db = new TestMonitorDBContext();
        static public EnergyRecordDateTotalDTO GetTodayEnergyTotal()
        {
            try
            {
                string sqlCommand = "SELECT CONVERT(NVARCHAR,DATE) AS WorkDate, CONVERT(FLOAT,COUNT(HOST_NAME)) AS TotalMachine, SUM(ACTIVE_TIME) AS TotalActive, SUM(STANDBY_TIME) AS TotalIdle FROM ENERGY_RECORD WHERE DATE >= CONVERT(DATE, GETDATE()) AND STANDBY_TIME > 0 GROUP BY DATE";

                EnergyRecordDateTotalDTO todayEnergyTotal = db.Database.SqlQuery<EnergyRecordDateTotalDTO>(sqlCommand).SingleOrDefault();

                return todayEnergyTotal;
            }
            catch (Exception ex)
            {

                return new EnergyRecordDateTotalDTO();
            }
            
        }
        static public List<EnergyRecordDateTotalDTO> GetTimeRangeEnergyTotal(string timeRange)
        {
            try
            {
                string[] timePart = timeRange.Split('-'); 

                string sqlCommand = "SELECT CONVERT(NVARCHAR,DATE) AS WorkDate, CONVERT(FLOAT,COUNT(HOST_NAME)) AS TotalMachine, SUM(ACTIVE_TIME) AS TotalActive, SUM(STANDBY_TIME) AS TotalIdle FROM ENERGY_RECORD WHERE DATE >= '" + timePart[0].Trim() + "' AND DATE <= '" + timePart[1].Trim() + "' AND STANDBY_TIME > 0 GROUP BY DATE ORDER BY DATE ASC";

                List<EnergyRecordDateTotalDTO> timeRangeEnergyTotal = db.Database.SqlQuery<EnergyRecordDateTotalDTO>(sqlCommand).ToList();

                return timeRangeEnergyTotal;
            }
            catch (Exception ex)
            {

                return new List<EnergyRecordDateTotalDTO>();
            }
        }
    }
}