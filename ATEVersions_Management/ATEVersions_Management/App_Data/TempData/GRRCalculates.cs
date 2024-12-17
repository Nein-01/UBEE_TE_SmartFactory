using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _04.TestWindowPrint.Models.GRRModels
{
    public class OperSampleDTO
    {
        public int OperNo { get; set; }
        public List<double[]> OperSamples { get; set; }
    }
    public class GRRCalculateANOVA
    {
        public List<OperSampleDTO> ListOperSampleDTO { get; set; }
        public double OperNum
        {
            get
            {
                if (ListOperSampleDTO.Count > 0)
                {
                    return ListOperSampleDTO.Count;
                }
                return 0;

            }
            set { }
        }
        public double PartNum
        {
            get
            {
                if (ListOperSampleDTO.Count > 0)
                {
                    return ListOperSampleDTO[0].OperSamples.Count;
                }
                return 0;
            }
        }
        public double TrialNum
        {
            get
            {
                if (ListOperSampleDTO.Count > 0)
                {
                    return ListOperSampleDTO[0].OperSamples[0].Length;
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
                return SSoFunc(ListOperSampleDTO);
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
                return SSpFunc(ListOperSampleDTO);
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
                return SSeFunc(ListOperSampleDTO);
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
                return AVGTotalFunc(ListOperSampleDTO);
            }
        }
        public double SStotal
        {
            get
            {
                return SStotalFunc(ListOperSampleDTO);
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
        private double AVGTotalFunc(List<OperSampleDTO> listOperSampleDTO)
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
        private double SStotalFunc(List<OperSampleDTO> listOperSampleDTO)
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
        private double SSoFunc(List<OperSampleDTO> listOperSampleDTO)
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
        private List<double> AVGPartFunc(List<OperSampleDTO> listOperSampleDTO)
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
        private double SSpFunc(List<OperSampleDTO> listOperSampleDTO)
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
        private double SSeFunc(List<OperSampleDTO> listOperSampleDTO)
        {
            double SSe = 0;

            for (int i = 0; i < OperNum; i++)
            {
                SSe += AVGEquipFunc(ListOperSampleDTO[i].OperSamples);
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
                return Math.Round(result, 4);
            }
        }
        public double EV
        {
            get
            {
                return Vrepeatability;
            }
        }
        public double PV
        {
            get
            {
                return Vp;
            }
        }
        public double AV
        {
            get
            {
                double result = Vo + Vop;
                return Math.Round(result, 4);
            }
        }
        public double TV
        {
            get
            {
                double result = Vrepeatability + Vo + Vop + Vp;
                return Math.Round(result, 4);
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
}