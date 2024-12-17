using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ATEVersions_Management.Models.DAOModels;
using System.Threading;

namespace ATEVersions_Management.Models.DTOModels
{
    // ====== ============================== ======
    //      DTO model to tranfer data from DB
    // ====== ============================== ======
    public class TETestDataDTO
    {
        public long ID { get; set; }

        [StringLength(254)]
        public string VIRTUAL_SN { get; set; }

        [StringLength(254)]
        public string WORK_DATE { get; set; }

        public int? WORK_SECTION { get; set; }

        [StringLength(254)]
        public string FULL_WORK_DATE { get; set; }

        [StringLength(254)]
        public string CFT { get; set; }

        [StringLength(254)]
        public string PROJECT { get; set; }

        [StringLength(254)]
        public string MODEL { get; set; }

        [StringLength(254)]
        public string LINE { get; set; }

        [StringLength(254)]
        public string STATION { get; set; }

        [StringLength(254)]
        public string ATE { get; set; }

        [StringLength(254)]
        public string FIXTURE_CODE { get; set; }

        [StringLength(254)]
        public string CARRIER_CODE { get; set; }

        public int? TEST_MODE { get; set; }

        [StringLength(254)]
        public string TEST_VERSION { get; set; }

        [StringLength(254)]
        public string MO { get; set; }

        [StringLength(254)]
        public string BOARD_SN { get; set; }

        [StringLength(254)]
        public string SN { get; set; }

        public DateTime? START_TIME { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? END_TIME { get; set; }

        public int? ELAPSE_TIME { get; set; }

        public int? STATUS { get; set; }

        [StringLength(254)]
        public string ERROR_CODE { get; set; }

        [StringLength(254)]
        public string TEST_ITEM { get; set; }

        [StringLength(254)]
        public string VALUE_TYPE { get; set; }

        [StringLength(254)]
        public string VALUE { get; set; }

        [StringLength(254)]
        public string LSL { get; set; }

        [StringLength(254)]
        public string USL { get; set; }
    }   

    #region Equipment estimate DTO
    public class TETestDataEquipmentEstimate
    {
        static int NextID;
        public int DataID { get;  set; }
        public string Line { get; set; }
        public string Model { get; set; }
        public string Station { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? PcsTotal { get; set; }
        public double TotalTime {
            get;
            set;
        }

        /*
            get 
            {
                if(FromDate.HasValue && ToDate.HasValue)
                {
                    List<TETestDataDTO> DataList = TETestDataDAO.GetTETestDataByLineModelStation(this.Line, this.Model, this.Station, this.FromDate.Value, this.ToDate.Value);
                    double result = 0;
                    foreach (var dto in DataList)
                    {
                        result += (dto.END_TIME.Value - dto.START_TIME.Value).TotalSeconds;
                    }
                    return result;
                }
                return 0;
            } 
            set { } 
        */

        private double _UnitTestTime;
        public double UnitTestTime
        {
            get
            {
                if(PcsTotal.HasValue && this.TotalTime != 0)
                {
                    return _UnitTestTime = Math.Round(this.TotalTime / this.PcsTotal.Value, 2);
                }
                return _UnitTestTime;
            }
            set { _UnitTestTime = value; }
        }
        public double LoadTime { get; set; }
        public double TotalUnitTime 
        { 
            get { return UnitTestTime + LoadTime; }
            set { }
        }
        public double DailyTargetOutput { get; set; }
        private double _RunHours = 18;
        public double RunHours 
        {
            get { return _RunHours; }
            set { _RunHours = value; } 
        }
        public double FPR 
        { 
            get { return 0.90; } 
            set { } 
        }
        public double Productivity 
        {
            get { return 0.90; }
            set { } 
        }
        public double EquipRequired
        {
            get 
            {
                if (DailyTargetOutput == 0 ) return 0;
                return Math.Ceiling(TotalUnitTime/(3600/(DailyTargetOutput/RunHours/FPR/Productivity))); 
            }
        }
        public double EquipExisted { get; set; }
        public double EquipGap { get { return  EquipExisted - EquipRequired; } }

        public double TheorenticalExistedOutput 
        { 
            get { return Math.Floor(3600/(TotalUnitTime/EquipExisted)*RunHours*FPR*Productivity); } 
        }
        /*public TETestDataModelStation()
        {
            DataID = Interlocked.Increment(ref NextID);
        }*/

    }
    #endregion
    #region Machine ATE Time DTO
    // === Model Object to handle machine test time ===
    public class TETestDataATETime
    {
        public string Line { get; set; }
        public string WorkDate { get; set; }
        public string Model { get; set; }
        public string Station { get; set; }
        public string ATEMachine { get; set; }
        public int PCS { get; set; }
        public double MeanTime { get; set; } // AKA UniTime
        public double LoadTime { get; set; }
        public double MinTime { get; set; }
        public double MaxTime { get; set; }
        public double RangeTime
        {
            get { return this.MaxTime - this.MinTime; } 
            set { }
        }
    }

    #endregion

}