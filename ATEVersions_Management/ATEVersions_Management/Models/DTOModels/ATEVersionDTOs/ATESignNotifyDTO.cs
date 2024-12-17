using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.DTOModels
{
    public class ATESignNotifyDTO
    {
        public List<ATEListDTO> ATEListNotify { get; set; } 
        public List<GRRTableDTO> GRRNotify { get; set; } 
    }
}