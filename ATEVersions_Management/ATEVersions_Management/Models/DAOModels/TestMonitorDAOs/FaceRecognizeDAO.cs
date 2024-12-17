using ATEVersions_Management.Models.DTOModels.TestMonitorDTOs;
using ATEVersions_Management.Models.TestMonitorModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace ATEVersions_Management.Models.DAOModels.TestMonitorDAOs
{
    public class FaceRecognizeDAO
    {
        static private readonly TestMonitorDBContext db = new TestMonitorDBContext();        
        static public string GetOwnerForTestMachine(string hostname, string line, DateTime timeCheck)
        {
            string onwerResult = "";
            try
            {
                FaceRecognizeDTO faceRecognizeDTO = (from emp in db.FACE_RECOGNITION_DATA
                                                     where emp.MACHINE_NAME.Trim().Contains(hostname.Trim())/* &&
                                                      (from empID in db.EMPLOYEES select empID.CARD_ID).Contains(emp.CARD_ID)*/
                                                     orderby emp.TIME descending
                                                     select new FaceRecognizeDTO
                                                     {
                                                         MACHINE_NAME = emp.MACHINE_NAME,
                                                         CARD_ID = emp.CARD_ID,
                                                         NAME = emp.NAME,
                                                         TIME = emp.TIME
                                                     }).Take(1).FirstOrDefault();

                if (faceRecognizeDTO != null)
                {
                    return faceRecognizeDTO.CARD_ID + " - " + faceRecognizeDTO.NAME;
                }
                string sqlCommand = "SELECT TOP 1 CARD_ID, NAME, COUNT(TIME) AS ACTION_TIME FROM FACE_RECOGNITION_DATA WHERE MACHINE_NAME LIKE '%" + line + "%' AND (TIME >= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME),' 19:30:00')) AND TIME <= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME),' 23:59:00'))) OR (TIME >= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME + 1),' 00:00:00')) AND TIME <= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME +1),' 06:30:00'))) GROUP BY CARD_ID, NAME ORDER BY COUNT(TIME) DESC";                
                if (timeCheck.TimeOfDay >= new TimeSpan(7, 30, 0))
                {
                    sqlCommand = "SELECT TOP 1 CARD_ID, NAME, COUNT(TIME) AS ACTION_TIME FROM FACE_RECOGNITION_DATA WHERE MACHINE_NAME LIKE '%" + line + "%' AND TIME >= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME),' 07:30:00')) AND TIME <= CONVERT(DATETIME,CONCAT(CONVERT(DATE, TIME),' 18:30:00')) GROUP BY CARD_ID, NAME ORDER BY COUNT(TIME) DESC";                    

                }
                FaceRecognizeOnwerDTO onwer = db.Database.SqlQuery<FaceRecognizeOnwerDTO>(sqlCommand).SingleOrDefault();
                if(onwer != null)
                {
                    onwerResult = onwer.CARD_ID + " - " + onwer.NAME;
                }                

                return onwerResult;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}