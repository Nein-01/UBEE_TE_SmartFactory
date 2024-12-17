using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels.TestMonitorDTOs
{
    public class FaceRecognizeDTO
    {        
        [StringLength(40)]
        public string MACHINE_NAME { get; set; }
        
        [StringLength(8)]
        public string CARD_ID { get; set; }
        
        [StringLength(100)]
        public string NAME { get; set; }
        
        public DateTime TIME { get; set; }

    }

    public class FaceRecognizeOnwerDTO{        

        [StringLength(8)]
        public string CARD_ID { get; set; }

        [StringLength(100)]
        public string NAME { get; set; }
        
        public double ACTION_COUNT { get; set; }
    }
}