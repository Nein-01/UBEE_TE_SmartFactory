using ATEVersions_Management.Models.CPKModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels
{
    // ====== ============================== ======
    //      DTO model to tranfer data from DB
    // ====== ============================== ======
    public class CPKTableDTO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(25)]
        public string MO_NUMBER { get; set; }

        [Required]
        [StringLength(25)]
        public string MODEL_NAME { get; set; }

        [Required]
        [StringLength(25)]
        public string STATION_NAME { get; set; }

        [Required]
        [StringLength(25)]
        public string PCB_SN { get; set; }

        [StringLength(25)]
        public string SSN { get; set; }

        [StringLength(25)]
        public string MAC { get; set; }

        public DateTime? DATE_TIME { get; set; }

        [StringLength(10)]
        public string STATUS { get; set; }

        public string CONTENT { get; set; }

        public CPKRawContent RawContents { 
            get
            {
                try
                {
                    CPKRawContent output = JsonConvert.DeserializeObject<CPKRawContent>(this.CONTENT);
                    return output;
                }
                catch (Exception ex)
                {
                    return new CPKRawContent();
                }                
            }
            set { } 
        }
        public List<CPKContentObject> CPKContentObjects
        {
            get;
            set;
            /*get 
            {
                return  (this.RawContents != null) ? 
                        GetContentObjectList(this.RawContents):
                        null; 
            }*/
        }

        public List<CPKContentObject> GetContentObjectList(CPKRawContent rawContent)
        {
            List<CPKContentObject> contentObjects = new List<CPKContentObject>();
            int rawContentSize = rawContent.name.Count;
            for (int i = 0; i < rawContentSize; i++)
            {
                CPKContentObject contentObjectTmp = new CPKContentObject
                {
                    Name = rawContent.name[i],
                    Value = rawContent.value[i],
                    SpecL = rawContent.specL[i],
                    SpecH = rawContent.specH[i]
                };
                if (contentObjects.Contains(contentObjectTmp))
                {
                    int idx = contentObjects.IndexOf(contentObjectTmp);
                    contentObjects[idx] = contentObjectTmp;
                    continue;
                }
                contentObjects.Add(contentObjectTmp);
            }

            return contentObjects;
        }
    }
    // ====== ============================== ======
    //  Realization objects to handle CONTENT field
    // ====== ============================== ======
    public class CPKRawContent
    {
        public List<string> name { get; set; }
        public List<double> value { get; set; }
        public List<double?> specL { get; set; }
        public List<double?> specH { get; set; }

        public override string ToString()
        {
            return "name: " + name.Count + " | value: " + value.Count + " | specL: " + specL.Count + " | specH: " + specH.Count;
        }
    }
    
    public class CPKContentObject
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public double? SpecL { get; set; }
        public double? SpecH { get; set; }        

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() ^ this.SpecL.GetHashCode() ^ this.SpecH.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is CPKContentObject other)
            {
                if (this.Name == other.Name && this.SpecL == other.SpecL && this.SpecH == other.SpecH)
                    return true;
            }
            return false;
        }
    }

    public class CPKModelStationContent
    {       

        [Display(Name = "Model")]
        public string ModelName { get; set; }

        [Display(Name = "Station")]
        public string Station { get; set; }
        public List<string> ListPCBSN { get; set; }
        public List<CPKContentInvidual> ContentGroup { get; set; }

        public override int GetHashCode()
        {
            return this.ModelName.GetHashCode() ^ this.Station.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is CPKModelStationContent other)
            {
                if(this.ModelName != other.ModelName && this.Station != other.Station)
                    return false;
            }
            return true;
        }

    }
    public class CPKContentInvidual
    {
        public int ContentID { get; set; }
        public string ItemName { get; set; }
        public double? SpecL { get; set; }
        public double? SpecH { get; set; }
        public List<string> PcbSN { get; set; }
        public List<double> Value { get; set; }
        public CPKCalculate CPKResult
        {
            get
            {
                if (Value.Count > 0)
                {
                    return new CPKCalculate
                    {
                        ItemName = ItemName,
                        SpecL = SpecL,
                        SpecH = SpecH,
                        PcbSNList = PcbSN,
                        DataSet = Value,
                    };
                }
                return new CPKCalculate();
            }
        }
        public override int GetHashCode()
        {
            return this.ItemName.GetHashCode() ^ this.SpecL.GetHashCode() ^ this.SpecH.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is CPKContentInvidual other)
            {
                if(this.ItemName != other.ItemName && this.SpecL != other.SpecL && this.SpecH != other.SpecH)
                    return false;
            }
            return true;
        }
    }

    // ====== ============================== ======
    //  DTO model to handle data from excel files
    // ====== ============================== ======

    public class CPKExcelDTO
    {
        public int ID {get; set; }
        public string ColName { get; set;}
        public List<string> RowValues { get; set; }
    }

    // ========= CPK data send to FII DTO Model =========
    public class Ubee_CPKData
    {
        public double? usl { get; set; }
        public double? lsl { get; set; }
        public string itemTest { get; set; }
        public double[] valueTest { get; set; }
    }
}