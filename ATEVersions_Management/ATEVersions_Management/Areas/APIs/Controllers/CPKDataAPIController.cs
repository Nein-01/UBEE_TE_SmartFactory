using ATEVersions_Management.Models.DAOModels;
using ATEVersions_Management.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ATEVersions_Management.Areas.APIs.Controllers
{
    public class CPKDataAPIController : ApiController
    {
        // GET: api/CPKDataAPI
        public List<Ubee_CPKData> GET_UbeeCPKData(string nameProject, string groupName, DateTime? startTime, DateTime? endTime)
        {
            try
            {
                //List<Ubee_CPKData> listUbeeCPKData = new List<Ubee_CPKData>();
                //
                CPKTableDTO cpkRecord = CPKTableDAO.GetOneCPKDataByModelAndStation(nameProject, groupName);
                List<CPKTableDTO> rawCPKContentList = CPKTableDAO.GetCPKByModelStationDate(nameProject, groupName, 0, "0", "0", startTime.Value, endTime.Value);                
                CPKModelStationContent cpkModelStation = CPKTableDAO.GetModelStationFullContentValue(CPKTableDAO.GetModelStationContent(cpkRecord), rawCPKContentList);
                //
                return CPKTableDAO.GET_UbeeCPKData(cpkModelStation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        // GET: api/CPKDataAPI
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // GET: api/CPKDataAPI
        public IEnumerable<string> Get(int id, int num)
        {
            return new string[] { "I", "Am", "Atomic" };
        }
        // GET: api/CPKDataAPI/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CPKDataAPI
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/CPKDataAPI/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CPKDataAPI/5
        public void Delete(int id)
        {
        }
    }
}
