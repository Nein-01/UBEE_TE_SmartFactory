using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels.TestMonitorDTOs
{
    public class EnergyRecordDTO
    {

    }

    public class EnergyRecordDateTotalDTO
    {
        public string WorkDate { get; set; }
        public double TotalMachine { get; set; }
        public double TotalActive { get; set; }
        public double TotalIdle { get; set; }
        public double PowerSave
        {
            get
            {
                return Math.Round(TotalIdle * 10 * (0.35 + 0.0116) * 0.8, 2);
            }
        }
        public double CostDown
        {
            get
            {
                return Math.Round(PowerSave * 0.64, 2);
            }
        }
    }
}