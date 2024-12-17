using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.HelperModels
{
    public class WebInfoModel
    {
        static public string Protocol = "https://";
        static public string ServerIP = "10.220.99.252:6443";
        public string PageRoute { get; set; }

        static public string GetWebPageURL(string pageRoute)
        {
            return Protocol + ServerIP + pageRoute;
        }

    }
}