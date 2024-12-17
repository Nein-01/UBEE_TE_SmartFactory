using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.HelperModels
{
    public class LogRecord
    {
        static public void WriteErrorLogRecord(string errorLogDirPath, DateTime recordDate, string module, string function, string errorMessage)
        {
            string logFileName = "ErrorLog_" + recordDate.ToString("yyyy-MM-dd") + ".txt";            
            string logFilePath = errorLogDirPath + logFileName;
            string messageFormat = "==============================\n" +
                                   "Module: " + module + "\nFunction: " + function + "\n" +
                                   recordDate.ToString() +
                                   "\nError detail: " + errorMessage +
                                   "\n==============================\n";
            File.AppendAllText(logFilePath, messageFormat);

        }
        /*static public void WriteErrorLogRecordLocale(DateTime recordDate, string module, string function, string errorMessage)
        {
            string logDirPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName ;
            string logFileName = "ErrorLog_" + recordDate.ToString("yyyy-MM-dd") + ".txt";
            string logFilePath = logDirPath + logFileName;
            string messageFormat = "==============================\n" +
                                   module + "-" + function + "\n" +
                                   recordDate.ToString() + "\n" +
                                   "Error detail: " + errorMessage +
                                   "\n==============================\n";
            File.AppendAllText(logFilePath, messageFormat);

        }*/
    }
}