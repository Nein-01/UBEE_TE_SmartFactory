using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.HelperModels
{
    public class ServerFileSystemHandler
    {
        static public bool IsDirExist(string filePath)
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}