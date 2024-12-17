using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels
{
    public class StationInforDTO
    {        
        [StringLength(50)]
        public string PRODUCT_NAME { get; set; }
        
        [StringLength(15)]
        public string MACHINE_NAME { get; set; }

        public bool? STATUS { get; set; }

        [StringLength(1024)]
        public string ROOT_CAUSE { get; set; }
        [StringLength(1024)]
        public string UNLOCK_BY { get; set; }
    }
}