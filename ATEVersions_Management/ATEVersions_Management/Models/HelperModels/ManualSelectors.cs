using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.HelperModels
{
    public class ManualSelectors
    {
        public struct StatusSelector
        {
            public int StatusCode { get; set; }
            public string StatusValue { get; set; }

            public StatusSelector(int statusCode, string statusValue)
            {
                StatusCode = statusCode;
                StatusValue = statusValue;
            }
        }

        public static List<StatusSelector> GetStatusSelectors()
        {
            List<StatusSelector> statusSelectors = new List<StatusSelector>
            {
                new StatusSelector(0, "Disable"),
                new StatusSelector(1, "Enable")
            };
            return statusSelectors;
        }
    }
}