using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels.OracleReTableDTOs
{
    public class IPQC_LCR_DTO
    {
        public string SN { get; set; }
        public string CUST_PN { get; set; }
        public string DATECODE { get; set; }
        public string VENDOR { get; set; }
        public string VENDORNO { get; set; }
        public string LOCATION { get; set; }
        public string QUANTITY { get; set; }
        public string REMAINQTY { get; set; }
        public string MATERIALTYPE { get; set; }
        public string DESCRIPTION { get; set; }
        public string LOWSPEC { get; set; }
        public string HIGHSPEC { get; set; }
        public string MEASUREVALUE { get; set; }
        public string STATUS { get; set; }
        public string DATETIME { get; set; }
        public string EMPLOYEE { get; set; }
        public string MARKING { get; set; }
        public string IDMATERIAL { get; set; }

    }

    public class LCRWorkShiftDTO
    {
        public LCRWorkShiftDTO()
        {
            ListWorkDay = new List<string>();
            ListPCS = new List<double>();
        }

        public List<string> ListWorkDay { get; set; }
        public List<double> ListPCS { get; set; }


    }
}