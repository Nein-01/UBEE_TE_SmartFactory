/*
 * Required lib MathNet.Numerics to calculate CDF
 * to add lib see LibReferences.xml in TE Stored Server: 10.220.99.251 - 16/11/2023
 * Path: 0.QuangChau_TE/Quang_TE/02_Templates&References/02_MyLib/01_AddLib/LibReferences.xml
 */
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.CPKModels
{
    public class CPKCalculate
    {
        //
        public CPKCalculate()
        {
            ItemName = string.Empty;
            SpecL = 0; 
            SpecH = 0; 
            PcbSNList = new List<string>();
            DataSet = new List<double>();
        }
        // Attributes to store data after calculated
        public string ItemName { get; set; }
        public double? SpecL { get; set; }
        public double? SpecH { get; set; }
        public List<string> PcbSNList { get; set; }
        // Store dataset and its spec
        public List<double> DataSet { get; set; }
        public long DataSize 
        { 
            get { return this.DataSet.Count; } 
            set { } 
        }
        public double Average
        {
            get
            {
                try
                {
                    return Math.Round(this.DataSet.Average(), 5);
                }
                catch (Exception ex)
                {
                    return 0;
                }
                
            }
            set { }
        }
        public double Min
        {
            get 
            {
                try
                {
                    return this.DataSet.Min();
                }
                catch (Exception ex)
                {
                    return 0;
                }
                
            }
            set { }
        }
        public double Max
        {
            get
            {
                try
                {
                    return this.DataSet.Max();
                }
                catch (Exception ex)
                {
                    return 0;
                }

            }
            set { }
        }        
        // Store overall sigma
        public double OverallSigma
        {
            get { return OverallSigmaFunc(this.DataSet,this.Average, this.DataSize); }
            set { }
        }
        // Store within sigma and within data draw
        private List<double> MoveRange
        {
            get { return MoveRangeFunc(this.DataSet); }
        }
        public double WithinSigma
        {
            get { return WithinSigmaFunc(this.MoveRange); }
            set { }
        }
        public List<WithinPoint> WithinData
        {
            get { return DerivedData(this.Average, this.WithinSigma, 3, 3); }
            //intervals and pointsInInterval both default are 3
            set { }
        }
        // Store Cp, Cpl, Cpu, Cpk, Pp, Ppl, Ppu, Ppk
        // Potential (Within) Capability
        public double Cp
        {
            get 
            {
                if (this.SpecL.HasValue && this.SpecH.HasValue)
                {
                    return CPpFunc(this.SpecL.Value, this.SpecH.Value, this.WithinSigma);
                }
                return 0;
            }
        }
        public double Cpl
        {
            get
            {
                if (this.SpecL.HasValue)
                {
                    //return CPplFunc(this.SpecL.Value, this.Average, this.WithinSigma) + 1;
                    return CPplFunc(this.SpecL.Value, this.Average, this.WithinSigma);
                }
                return 0;
            }
        }
        public double Cpu
        {
            get
            {
                if (this.SpecH.HasValue)
                {
                    //return CPpuFunc(this.SpecH.Value, this.Average, this.WithinSigma) + 1;
                    return CPpuFunc(this.SpecH.Value, this.Average, this.WithinSigma);
                }
                return 0;
            }
        }
        private double _Cpk;
        public double Cpk
        {
            get
            {
                if(this.DataSet.Count < 1)
                {
                    //return 1060524;
                    _Cpk = 9999999;
                }
                if ((this.DataSet[0] == this.DataSet.Average()))
                {
                    //return 1060524;
                    _Cpk = 9999999;
                }
                if (this.Cpl == 0)
                {
                    _Cpk = this.Cpu;
                }
                if (this.Cpu == 0)
                {
                    _Cpk = this.Cpl;
                }
                _Cpk = CPpkFunc(this.Cpl,this.Cpu);
                return _Cpk;
            }
            set
            {
                _Cpk = value;
            }
        }
        // Overall Capability
        public double Pp
        {
            get
            {
                if (this.SpecL.HasValue && this.SpecH.HasValue)
                {
                    return CPpFunc(this.SpecL.Value, this.SpecH.Value, this.OverallSigma);
                }
                return 0;
            }
        }
        public double Ppl
        {
            get
            {
                if (this.SpecL.HasValue)
                {
                    return CPplFunc(this.SpecL.Value, this.Average, this.OverallSigma);
                }
                return 0;
            }
        }
        public double Ppu
        {
            get
            {
                if (this.SpecH.HasValue)
                {
                    return CPpuFunc(this.SpecH.Value, this.Average, this.OverallSigma);
                }
                return 0;
            }
        }
        public double Ppk
        {
            get
            {
                if (this.Ppl == 0)
                {
                    return this.Ppu;
                }
                if (this.Ppu == 0)
                {
                    return this.Ppl;
                }
                return CPpkFunc(this.Ppl, this.Ppu);
            }
        }
        // Store PPM_LSL, PPM_USL, PPM_Total
        // Within performance
        public double WtPpm_Lsl
        {
            get
            {
                if (this.SpecL.HasValue)
                {
                    return PPM_LSL(this.SpecL.Value, this.Average, this.WithinSigma);
                }
                return 0;
            }
        }
        public double WtPpm_Usl
        {
            get
            {
                if (this.SpecH.HasValue)
                {
                    return PPM_USL(this.SpecH.Value, this.Average, this.WithinSigma);
                }
                return 0;
            }
        }
        public double WtPpm_Total
        {
            get
            {                
                return PPM_Total(this.WtPpm_Lsl, this.WtPpm_Usl);
            }
        }
        // Overall performance
        public double OvPpm_Lsl
        {
            get
            {
                if (this.SpecL.HasValue)
                {
                    return PPM_LSL(this.SpecL.Value, this.Average, this.OverallSigma);
                }
                return 0;
            }
        }
        public double OvPpm_Usl
        {
            get
            {
                if (this.SpecH.HasValue)
                {
                    return PPM_USL(this.SpecH.Value, this.Average, this.OverallSigma);
                }
                return 0;
            }
        }
        public double OvPpm_Total
        {
            get
            {
                return PPM_Total(this.OvPpm_Lsl, this.OvPpm_Usl);
            }
        }
        // Store bin count and binwidth
        /*private long LargestP
        {
            get { return FindLargestP(this.DataSet); }
            set { }
        }
        private double BinRange
        {
            get { return BinRangeFunc(this.LargestP); }
            set { }
        }*/
        public double LowerBound
        {
            get { return LowerBoundFunc(this.DataSize); }
            set { }
        }
        public double UpperBound
        {
            get { return UpperBoundFunc(this.LowerBound); }
            set { }
        }
        public int BinCount
        {
            get { return BinCountFunc(this.Min, this.Max, 1, this.LowerBound, this.UpperBound); }
            set { }
        }
        public double BinWidth 
        {
            get { return BinWidthFunc(this.Min, this.Max, this.BinCount); }
            set { } 
        }
        //Common calculate functions
        //Overall Sigma Module
        public double OverallSigmaFunc(List<double> data, double avg, long size)
        {

            double sum = 0;
            long n = size - 1;
            foreach (double d in data)
            {
                sum += Math.Pow((d - avg), 2);
            }
            double sigma = Math.Sqrt(sum / (n));
            if(size <= 300)
            {
                return Math.Round(sigma / UnbiasConst(n), 6);
            }
            return Math.Round(sigma, 6);
        }
        public double UnbiasConst(long n)
        {
            double nume = (n + 1) / 2.0;
            double deno = n / 2.0;

            //nume = GammaFunc(nume);
            nume = SpecialFunctions.Gamma(nume);
            //deno = GammaFunc(deno);
            deno = SpecialFunctions.Gamma(deno);
            double result = Math.Sqrt(2.0 / n) * (nume / deno);

            return result;
        }

        public double GammaFunc(double x)
        {
            double result = 1;
            double gaussianConst = (Math.Sqrt(Math.PI) / 2);
            for (double i = x - 1; i > 0.5; i -= 1)
            {
                result *= i;
            }
            if (!IsNumberRounded(x))
            {
                result *= gaussianConst;
            }

            return result;
        }
        public bool IsNumberRounded(double n)
        {
            return ((n - Math.Truncate(n)) == 0.0);

        }
        // Within Sigma - Average Moving Range Method 
        public List<double> MoveRangeFunc(List<double> dataSet)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < dataSet.Count - 1; i++)
            {
                result.Add(Math.Round(dataSet[i + 1] - dataSet[i], 6));
            }

            return result;
        }
        public double WithinSigmaFunc(List<double> moveRange)
        {
            int dataSize = moveRange.Count;
            if (dataSize == 0)
            {
                return 0;
            }
            if(dataSize == 1)
            {
                return Math.Abs(moveRange[0]);
            }
            for (int i = 0; i < dataSize; i++)
            {
                moveRange[i] = Math.Abs(moveRange[i]);
            }
            double rsl = moveRange.Average() / 1.128;
            // 1.128 is unbiased const of d2(w) with w = 2 - default value of minitab - look up in d2 table

            return Math.Round(rsl, 6);
        }

        // Cp, Cpl, Cpu, Cpk, Pp, Ppl, Ppu, Ppk Module
        public double CPpFunc(double lsl, double usl, double sgm)
        {
            double rsl = (usl - lsl) / (6 * sgm);
            return Math.Round(rsl, 2);
        }
        public double CPplFunc(double lsl, double avg, double sgm)
        {
            double rsl = (avg - lsl) / (3 * sgm);
            return Math.Round(rsl, 2);
        }
        public double CPpuFunc(double usl, double avg, double sgm)
        {
            double rsl = (usl - avg) / (3 * sgm);
            return Math.Round(rsl, 2);
        }
        public double CPpkFunc(double cppl, double cppu)
        {
            double rsl = Math.Min(cppl, cppu);
            return Math.Round(rsl, 2);
        }
        // Cpm Module
        public double CpmFunc(double lsl, double usl, double sigmaTarget)
        {
            double result = (usl - lsl) / (6 * sigmaTarget);

            return result;
        }
        public double SigmaTarget(List<double> dataSet, int size, double target)
        {
            double sum = 0;
            foreach (double x in dataSet)
            {
                sum += ((x - target) * (x - target));
            }
            double result = sum / size;
            return result;
        }
        // Expected performance - PPM compare with LSL, USL
        // required lib MathNet.Numerics to calculate CDF
        public double PPM_LSL(double lsl, double avg, double sigma)
        {
            double result = 1000000 * Normal.CDF(avg, sigma, lsl);
            return Math.Round(result, 2);
        }
        public double PPM_USL(double usl, double avg, double sigma)
        {
            double result = 1000000 * (1 - Normal.CDF(avg, sigma, usl));
            return Math.Round(result, 2);
        }
        public double PPM_Total(double ppmLsl, double ppmUsl)
        {
            double result = ppmLsl + ppmUsl;
            return result;
        }
        // Calculate Dataset to draw within normal distribution graph
        // require a class to store {x;y} point.
        public double NormalDensity(double x, double avg, double sgm)
        {
            double translation = (x - avg);
            return Math.Exp(-(translation * translation) /
                (2 * sgm * sgm)) / (sgm * Math.Sqrt(2 * Math.PI));
        }
        public List<WithinPoint> DerivedData(double avg, double sgm, double intervals, double pointsInInterval)
        {
            //intervals and pointsInInterval both default are 3
            List<WithinPoint> result = new List<WithinPoint>();
            double stop = intervals * pointsInInterval * 2 + 1;
            double increment = sgm / pointsInInterval;
            double x = avg - intervals * sgm;
            for (int i = 0; i < stop; i++)
            {
                result.Add(new WithinPoint(x, NormalDensity(x, avg, sgm)));
                x += increment;
            }
            return result;
        }
        // Minitab's bins number and bin width formula to draw histogram graph
        // Calculate the bin number with formula provided by minitab        
        public double BinWidthFunc(double min, double max, int binCount)
        {
            double result = (max-min)/binCount;

            return Math.Round(result,3);
        }
        public int BinCountFunc(double min, double max, double binRange, double lowBound, double upBound)
        {
            if (min == max)
            {
                return (int)lowBound;
            }
                
            int result = 1 + (int)Math.Round((max - min) / binRange);
            while (result < lowBound)
            {

                for (double i = min; i < max; i += binRange)
                {
                    result++;
                }
            }

            return result;
        }

        public double LowerBoundFunc(double n)
        {
            return Math.Floor(Math.Pow((16 * n), 1.0 / 3.0) + 0.5);
        }

        public double UpperBoundFunc(double n)
        {
            return Math.Floor(n + (0.5 * n));
        }

        public double BinRangeFunc(long p)
        {
            return Math.Pow(10, p);
        }

        public long FindLargestP(List<double> dataSet)
        {
            long p = 0;
            int dataSize = dataSet.Count;
            for (int i=0; i<dataSize-1;++i)
            {
                if (dataSet[i] == dataSet[i + 1]) continue;
                long currP = GetP(dataSet[i]);
                if (currP > p) p = currP;
            }
            return p;
        }

        public long GetP(double val)
        {
            long p = 0;
            double tmp = val;
            while (tmp % 10 >= 0)
            {
                p++;
                tmp /= 10;
            }
            return p;
        }
        // Extra Module
        public long Factorial(long x)
        {
            long result = 1;
            for (long i = x; i > 1; i--)
            {
                result *= i;
            }

            return result;
        }

        public double RoundIntervals(double n)
        {
            double afterPoint = n - Math.Truncate(n);
            double after3Point = (afterPoint * 1000) - Math.Truncate(afterPoint * 1000);
            double after2Point = (afterPoint * 100) - Math.Truncate(afterPoint * 100);
            double result = Math.Round(n, 1, MidpointRounding.AwayFromZero);
            if (after3Point < 0.5)
                result = Math.Round(n, 3, MidpointRounding.AwayFromZero);
            if (after3Point > 0.5 && after2Point < 0.5)
                result = Math.Round(n, 2);
            if (after2Point > 0.5)
                result = Math.Round(n, 2, MidpointRounding.AwayFromZero);
            return result;
        }

        //end common calculating


        //...To be added
        //End common funcs
    }
    public class WithinPoint
    {
        public double x { get; set; }
        public double y { get; set; }

        public WithinPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}