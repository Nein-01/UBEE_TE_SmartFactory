using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ATEVersions_Management.Models.TestMonitorModels;
using ATEVersions_Management.Models.DTOModels;

namespace ATEVersions_Management.Models.DAOModels
{    
    public class StationInforDAO
    {
        static readonly TestMonitorDBContext db = new TestMonitorDBContext();
        static async public Task<StationInforDTO> GetStationInfoByMachineModel(string machine, string model)
        {
            return await (from stationInfo in db.STATION_INFORMATION
                    where stationInfo.PRODUCT_NAME.ToLower() == model.ToLower() &&
                    stationInfo.MACHINE_NAME.ToLower() == machine.ToLower()
                    select new StationInforDTO
                    {
                        MACHINE_NAME = stationInfo.MACHINE_NAME,
                        PRODUCT_NAME = stationInfo.PRODUCT_NAME,
                        STATUS = stationInfo.STATUS,
                        ROOT_CAUSE = stationInfo.ROOT_CAUSE,
                        UNLOCK_BY = stationInfo.UNLOCK_BY,
                        
                    }).FirstOrDefaultAsync();
        }
        static public async Task<bool?> GetStationStatusByMachineModel(string machine, string model)
        {
            return await (from stationInfo in db.STATION_INFORMATION
                          where stationInfo.PRODUCT_NAME.ToLower() == model.ToLower() &&
                          stationInfo.MACHINE_NAME.ToLower() == machine.ToLower()
                          select stationInfo.STATUS).FirstOrDefaultAsync();
        } 
        static public bool ChangePCLockStatus(string userName, string model, string atePC, int lockStatus)
        {
            string rootCause = "Locked By Web " + userName;
            string actionCol = "UNLOCK_BY = NULL, ROOT_CAUSE";
            if (lockStatus == 0)
            {
                rootCause = "Web_" + userName;
                actionCol = "UNLOCK_BY";
            }
            string sqlCommand = "UPDATE STATION_INFORMATION SET STATUS = @lockStatus, "+ actionCol + " = @rootCause WHERE PRODUCT_NAME = @model AND MACHINE_NAME = @atePC";
            try
            {
                db.Database.ExecuteSqlCommand(sqlCommand,
                new SqlParameter("lockStatus", lockStatus),
                new SqlParameter("rootCause", rootCause),
                new SqlParameter("model", model),
                new SqlParameter("atePC", atePC));
                return true;
            }catch (Exception ex)
            {
                return false;
            }
            
        }
        static public void InsertNewPCLockRecord(string atePC, string model)
        {
            string sqlCommand = "INSERT INTO STATION_INFORMATION(PRODUCT_NAME,MACHINE_NAME,STATUS) VALUES(@model,@atePC,0)";
            try
            {
                db.Database.ExecuteSqlCommand(sqlCommand,                
                new SqlParameter("model", model),
                new SqlParameter("atePC", atePC)
                );
                
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}