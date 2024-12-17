using _04.TestWindowPrint.Models.GRRModels;
using ATEVersions_Management.Models.CPKModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels
{
    public class GRRTableDTO
    {
        [Display(Name = "GRR_ID")]
        public int GRR_ID { get; set; }

        [Display(Name = "UserID")]
        public int UserID { get; set; }

        [Display(Name = "Dept")]
        public string Dept { get; set; }

        [Display(Name = "Gage Model")]
        [StringLength(50)]
        [Required(ErrorMessage = "Can't leave blank!")]
        public string GageModel { get; set; }

        [Display(Name = "Gage Name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Can't leave blank!")]
        public string GageName { get; set; }

        [Display(Name = "Gage No")]
        [StringLength(50)]
        public string GageNo { get; set; }

        [Display(Name = "Part Name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Can't leave blank!")]
        public string PartName { get; set; }

        [Display(Name = "Specification")]
        [StringLength(250)]
        public string Specification { get; set; }

        [Display(Name = "Characteristic")]
        [StringLength(250)]
        public string Characteristic { get; set; }

        [Display(Name = "JSON_OperTestResult")]        
        public string JSON_OperTestResult { get; set; }
       
        [Display(Name = "Prepared By")]
        [StringLength(50)]
        public string PreparedBy { get; set; }

        [Display(Name = "Prepared at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? PreparedAt { get; set; }

        [Display(Name = "Preparer note")]
        [StringLength(250)]
        public string PreparedNote { get; set; }                

        [Display(Name = "Approved by")]
        [StringLength(50)]
        public string ApprovedBy { get; set; }

        [Display(Name = "Approved at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "Approver note")]
        [StringLength(250)]
        public string ApproverNote { get; set; }

        [Display(Name = "Status")]
        public int? Status { get; set; }

        [Display(Name = "Created at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Updated at")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Updated by")]
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        //
        [Display(Name = "GR&R")]
        public GRRCalculateANOVA GRR_Result // GRRCalculateANOVA | GRRCalculateXbarRange
        {
            get
            {
                try
                {
                    List<OBJ_OperTestResult> listOperSamples = JsonConvert.DeserializeObject<List<OBJ_OperTestResult>>(this.JSON_OperTestResult);
                    /*GRRCalculateXbarRange GRRResult = new GRRCalculateXbarRange()
                    {
                        ListOperSamples = listOperSamples
                    };*/
                    GRRCalculateANOVA GRRResult = new GRRCalculateANOVA()
                    {
                        ListOperSamples = listOperSamples
                    };
                    return GRRResult;
                } catch (Exception ex)
                {
                    return null;
                }
                
            }
        }
    }

    #region GRR Data Model Process
    public class GRRModifiedOperSample
    {
        public OBJ_OperTestResult OperSamplesResult { get; set; }
        public List<int> ListPosMark { get; set; }
    }

    public class OBJ_OperTestResult
    {
        public int ID { get; set; }
        public string OperName { get; set; }
        public List<double[]> OperSamples { get; set; }
        public double[] OperSampleRanges 
        {
            get
            {
                int sizeSamples = this.OperSamples.Count;
                if (sizeSamples != 0)
                {
                    double[] result = new double[sizeSamples];
                    for(int i = 0; i < sizeSamples; i++)
                    {
                        result[i] = Math.Round(this.OperSamples[i].Max() - this.OperSamples[i].Min(),2);
                    }
                    return result;
                }
                return null;
            }
            set { }
        }
        public double[] OperSampleAVG
        {
            get
            {
                int sizeSamples = this.OperSamples.Count;
                if (sizeSamples != 0)
                {
                    double[] result = new double[sizeSamples];
                    for (int i = 0; i < sizeSamples; i++)
                    {
                        result[i] = Math.Round(this.OperSamples[i].Average(), 2);
                    }
                    return result;
                }
                return null;
            }
            set { }
        }
    }
    
    public class GRRModelStationContent
    {
        [Display(Name = "Model")]
        public string ModelName { get; set; }

        [Display(Name = "Station")]
        public string Station { get; set; }       
        public List<GRRContentInvidual> GRRContentGroup { get; set; }

        public override int GetHashCode()
        {
            return this.ModelName.GetHashCode() ^ this.Station.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is GRRModelStationContent other)
            {
                if (this.ModelName != other.ModelName && this.Station != other.Station)
                    return false;
            }
            return true;
        }
    }

    public class GRRContentInvidual
    {
        public int ContentID { get; set; }
        public string ItemName { get; set; }
        public double? SpecL { get; set; }
        public double? SpecH { get; set; }       
        public List<double> Value { get; set; }
        
        public override int GetHashCode()
        {
            return this.ItemName.GetHashCode() ^ this.SpecL.GetHashCode() ^ this.SpecH.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is GRRContentInvidual other)
            {
                if (this.ItemName != other.ItemName && this.SpecL != other.SpecL && this.SpecH != other.SpecH)
                    return false;
            }
            return true;
        }
    }

    
    #endregion

    #region GRR Calculating Fomulas
    public class GRRCalculateXbarRange
    {
        public List<OBJ_OperTestResult> ListOperSamples { get; set; }
        public List<double> XbarP 
        {
            get
            {
                return CalXbarP(this.ListOperSamples);
            }
            set { }
        }
        public List<double> XbarABC
        {
            get { return CalXbarABC(this.ListOperSamples); }
            set { }
        }
        public List<double> RbarABC
        {
            get { return CalRbarABC(this.ListOperSamples); }
            set { }
        }
        public double Xdiff
        {
            get { return Math.Round(this.XbarABC.Max() - this.XbarABC.Min(), 4); }
            set { } 
        }
        public double R2bar
        {
            get { return Math.Round(this.RbarABC.Average(), 4); }
            set { }
        }
        public double Rp
        {
            get { return Math.Round(this.XbarP.Max() - this.XbarP.Min(), 4); }
            set { }
        }
        public double EV
        {
            get 
            {
                double EV = Math.Round(R2bar * 0.5908,4);
                return EV;
            }
            set { }
        }
        public double AV
        {
            get
            {
                double Xdiff_K2 = (Xdiff * 0.5231) * (Xdiff * 0.5231);
                double EV_nr = (EV * EV)/(10*3);
                double AV = (EV_nr > Xdiff_K2) ? 0 : Math.Round(Math.Sqrt(Xdiff_K2 - EV_nr),4);
                return AV;
            }
            set { }
        }
        public double PV
        {
            get
            {
                double PV = Math.Round(Rp * 0.7071,4);
                return PV;
            }
            set { }
        }
        public double GRR
        {
            get 
            {
                double EV_2 = (EV * EV);
                double AV_2 = (AV * AV);
                double RR = Math.Round(Math.Sqrt(EV_2 + AV_2), 4);
                return RR;
            }
            set { }
        }        
        public double NDC
        {
            get
            {
                double PV_RR = PV / GRR;
                double ndc = Math.Round(1.41 * PV_RR, 3);
                return ndc;
            }
            set { }
        }
        public double TV
        {
            get
            {
                double RR_2 = (GRR * GRR);
                double PV_2 = (PV * PV);
                double TV = Math.Round(Math.Sqrt(PV_2 + RR_2), 4);
                return TV;
            }
            set { }
        }
        public double PercentEV
        {
            get 
            {
                double percent_EV = 100 * (EV / TV);
                return Math.Round(percent_EV,2);
            }
        }
        public double PercentAV
        {
            get
            {
                double percent_EV = 100 * (AV / TV);
                return Math.Round(percent_EV, 2);
            }
        }
        public double PercentGRR
        {
            get
            {
                double percent_EV = 100 * (GRR / TV);
                return Math.Round(percent_EV, 2);
            }
        }
        public double PercentPV
        {
            get
            {
                double percent_EV = 100 * (PV / TV);
                return Math.Round(percent_EV, 2);
            }
        }
        private List<double> CalXbarP(List<OBJ_OperTestResult> _listData)
        {
            List<double> listResult = new List<double>();
            int _dataSize = _listData.Count;
            int _objListSamplesSize = _listData[0].OperSamples.Count;

            for(int i = 0; i < _objListSamplesSize; i++)
            {
                double nume = 0;
                double deno = 0;
                for(int j = 0; j < _dataSize; j++)
                {
                    nume += _listData[j].OperSamples[i].Sum();
                    deno += _listData[j].OperSamples[i].Length;
                }
                double avgSample = Math.Round(nume / deno, 3);
                listResult.Add(avgSample);
            }


            return listResult;
        }
        private List<double> CalXbarABC(List<OBJ_OperTestResult> _listData)
        {
            List<double> listResult = new List<double>();
            int _dataSize = _listData.Count;            
           
            for (int i = 0; i < _dataSize; i++)
            {
                int _sampleSize = _listData[i].OperSamples.Count;
                double nume = 0;
                double deno = 0;
                for(int j = 0; j < _sampleSize; j++)
                {
                    nume += _listData[i].OperSamples[j].Sum();
                    deno += _listData[i].OperSamples[j].Length;
                }
                double avgResult = Math.Round(nume / deno, 4);
                listResult.Add(avgResult);
            }                

            return listResult;
        }
        private List<double> CalRbarABC(List<OBJ_OperTestResult> _listData)
        {
            List<double> listResult = new List<double>();
            int _dataSize = _listData.Count;

            for (int i = 0; i < _dataSize; i++)
            {                
                double avgResult = Math.Round(_listData[i].OperSampleRanges.Average(), 3);
                listResult.Add(avgResult);
            }

            return listResult;
        }
    }

    public class GRRCalculateANOVA
    {
        public List<OBJ_OperTestResult> ListOperSamples { get; set; }
        public double OperNum
        {
            get
            {
                if (ListOperSamples.Count > 0)
                {
                    return ListOperSamples.Count;
                }
                return 0;

            }
            set { }
        }
        public double PartNum
        {
            get
            {
                if (ListOperSamples.Count > 0)
                {
                    return ListOperSamples[0].OperSamples.Count;
                }
                return 0;
            }
        }
        public double TrialNum
        {
            get
            {
                if (ListOperSamples.Count > 0)
                {
                    return ListOperSamples[0].OperSamples[0].Length;
                }
                return 0;
            }
        }
        //=== ANOVA table value calculate ===
        //SSo - DFo
        public double SSo
        {
            get
            {
                return SSoFunc(ListOperSamples);
            }
        }
        public double DFo
        {
            get
            {

                return OperNum - 1;

            }
            set { }
        }
        public double MSo
        {
            get
            {
                double result = SSo / DFo;
                return Math.Round(result, 4);
            }
        }
        public double Fo
        {
            get
            {
                double result = MSo / MSop;
                return Math.Round(result, 4);
            }
        }
        //SSp - DFp
        public double SSp
        {
            get
            {
                return SSpFunc(ListOperSamples);
            }
        }
        public double DFp
        {
            get
            {
                return PartNum - 1;
            }
        }
        public double MSp
        {
            get
            {
                double result = SSp / DFp;
                return Math.Round(result, 4);
            }
        }
        public double Fp
        {
            get
            {
                double result = MSp / MSop;
                return Math.Round(result, 4);
            }
        }
        //SSe - DFe
        public double SSe
        {
            get
            {
                return SSeFunc(ListOperSamples);
            }
        }
        public double DFe
        {
            get
            {
                return OperNum * PartNum * (TrialNum - 1);
            }
        }
        public double MSe
        {
            get
            {
                double result = SSe / DFe;
                return Math.Round(result, 4);
            }
        }
        //SSt - DFt
        public double AVGTotal
        {
            get
            {
                return AVGTotalFunc(ListOperSamples);
            }
        }
        public double SStotal
        {
            get
            {
                return SStotalFunc(ListOperSamples);
            }
        }
        public double DFtotal
        {
            get
            {
                return OperNum * PartNum * TrialNum - 1;
            }
        }
        //SSop - DFop
        public double SSop
        {
            get
            {
                double result = SStotal - (SSo + SSp + SSe);
                return Math.Round(result, 4);
            }
        }
        public double DFop
        {
            get
            {
                return DFo * DFp;
            }
        }
        public double MSop
        {
            get
            {
                double result = SSop / DFop;
                return Math.Round(result, 4);
            }
        }
        public double Fop
        {
            get
            {
                double result = MSop / MSe;
                return Math.Round(result, 4);
            }
        }

        //Calculate functions
        //SSt - Total sum of squares calculate functions
        private double AVGTotalFunc(List<OBJ_OperTestResult> listOperSampleDTO)
        {
            double nume = 0;
            double deno = OperNum * PartNum * TrialNum;

            for (int i = 0; i < OperNum; i++)
            {
                for (int j = 0; j < PartNum; j++)
                {
                    nume += listOperSampleDTO[i].OperSamples[j].Sum();
                }
            }

            double avgTotal = Math.Round(nume / deno, 4);
            return avgTotal;
        }
        private double SStotalFunc(List<OBJ_OperTestResult> listOperSampleDTO)
        {
            double SStotal = 0;
            for (int i = 0; i < OperNum; i++)
            {
                for (int j = 0; j < PartNum; j++)
                {
                    for (int k = 0; k < TrialNum; k++)
                    {
                        double partResult = listOperSampleDTO[i].OperSamples[j][k] - AVGTotal;
                        SStotal += (partResult * partResult);
                    }
                }
            }
            return Math.Round(SStotal, 3);
        }

        //SSo - Operator sum of squares calculate functions
        private double AVGOperFunc(List<double[]> operSamples)
        {
            double nume = 0;
            double deno = PartNum * TrialNum;
            for (int i = 0; i < PartNum; i++)
            {
                for (int j = 0; j < TrialNum; j++)
                {
                    nume += operSamples[i][j];
                }
            }
            double AVGOper = Math.Round(nume / deno, 4);
            return AVGOper;
        }
        private double SSoFunc(List<OBJ_OperTestResult> listOperSampleDTO)
        {
            double SSo = 0;

            for (int i = 0; i < OperNum; i++)
            {
                double AVGOper = AVGOperFunc(listOperSampleDTO[i].OperSamples);
                double partResult = AVGOper - AVGTotal;
                SSo += (partResult * partResult);
            }

            SSo = PartNum * TrialNum * SSo;

            return Math.Round(SSo, 4);

        }

        //SSp - Parts sum of squares calculate functions
        private List<double> AVGPartFunc(List<OBJ_OperTestResult> listOperSampleDTO)
        {
            List<double> listAVGPart = new List<double>();

            for (int i = 0; i < PartNum; i++)
            {
                double nume = 0;
                double deno = 0;
                for (int j = 0; j < OperNum; j++)
                {
                    nume += listOperSampleDTO[j].OperSamples[i].Sum();
                    deno += listOperSampleDTO[j].OperSamples[i].Length;
                }
                double avgSample = Math.Round(nume / deno, 4);
                listAVGPart.Add(avgSample);
            }

            return listAVGPart;

        }
        private double SSpFunc(List<OBJ_OperTestResult> listOperSampleDTO)
        {
            List<double> listAVGPart = AVGPartFunc(listOperSampleDTO);
            int AVGPartSize = listAVGPart.Count;
            double SSp = 0;
            for (int i = 0; i < AVGPartSize; i++)
            {
                double partResult = listAVGPart[i] - AVGTotal;
                SSp += (partResult * partResult);
            }
            SSp = OperNum * TrialNum * SSp;
            return Math.Round(SSp, 4);
        }

        //SSe - Equipment sum of squares calculate functions
        private double AVGEquipFunc(List<double[]> operSamples)
        {
            double AVGEquip = 0;

            for (int i = 0; i < PartNum; i++)
            {
                double RowAVG = operSamples[i].Average();
                for (int j = 0; j < TrialNum; j++)
                {
                    double partResult = operSamples[i][j] - RowAVG;
                    AVGEquip += partResult * partResult;
                }
            }

            return Math.Round(AVGEquip, 4);
        }
        private double SSeFunc(List<OBJ_OperTestResult> listOperSampleDTO)
        {
            double SSe = 0;

            for (int i = 0; i < OperNum; i++)
            {
                SSe += AVGEquipFunc(ListOperSamples[i].OperSamples);
            }

            return Math.Round(SSe, 4);
        }

        //=== Evaluate repeatability, operator, part, iteraction variance ===
        public double Vrepeatability
        {
            get
            {
                return MSe;
            }
        }
        public double Vop
        {
            get
            {
                double result = (MSop - Vrepeatability) / TrialNum;
                return (result < 0) ? 0 : Math.Round(result, 4);
            }
        }
        public double Vp
        {
            get
            {
                double result = (MSp - MSop) / (OperNum * TrialNum);
                return (result < 0) ? 0 : Math.Round(result, 4);
            }
        }
        public double Vo
        {
            get
            {
                double result = (MSo - MSop) / (PartNum * TrialNum);
                return (result < 0) ? 0 : Math.Round(result, 4);
            }
        }
        //=== GRR results ===
        public double GRR
        {
            get
            {
                double result = Vrepeatability + Vo;
                return Math.Round(result, 2);
            }
        }
        public double EV
        {
            get
            {
                return Math.Round(Vrepeatability, 2);
            }
        }
        public double PV
        {
            get
            {
                return Math.Round(Vp, 2);
            }
        }
        public double AV
        {
            get
            {
                double result = Vo + Vop;
                return Math.Round(result, 2);
            }
        }
        public double TV
        {
            get
            {
                double result = Vrepeatability + Vo + Vop + Vp;
                return Math.Round(result, 2);
            }
        }
        //=== %GRR ===
        public double PercentGRR
        {
            get
            {
                double result = (GRR / TV) * 100;
                return Math.Round(result, 2);
            }
        }
        public double PercentEV
        {
            get
            {
                double result = (EV / TV) * 100;
                return Math.Round(result, 2);
            }
        }
        public double PercentPV
        {
            get
            {
                double result = (PV / TV) * 100;
                return Math.Round(result, 2);
            }
        }
        public double PercentAV
        {
            get
            {
                double result = (AV / TV) * 100;
                return Math.Round(result, 2);
            }
        }
    }
    #endregion
}