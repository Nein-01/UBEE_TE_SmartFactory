using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
//using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;

namespace ATEVersions_Management.Models.HelperModels 
{ 
    public class OracleConnector
    {
        public bool IsConnected = false;
        private OracleConnection Connection
        {
            get
            {
                return GetConnection();
            }
        }
        OracleCommand Command = null;

        public OracleConnection GetConnection()
        {
            
            string ConnString = "User ID=re; Password=repro;data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=10.220.99.144)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=redata)))";
            OracleConnection myConn = new OracleConnection(ConnString);
            return myConn;
        }
        public bool CheckConnection()
        {
            try
            {                
                Connection.Open();
                IsConnected = true;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        public DataTable ExecSqlQuery(string SqlCommand)
        {            
            Connection.Open();
            OracleDataAdapter oda = new OracleDataAdapter(SqlCommand, Connection);
            DataTable dt = new DataTable();
            oda.Fill(dt);
            Connection.Close();
            return dt;
        }
    }
}