using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Logics.Helper
{
    public class ExceptionLog
    {
        public static void InsertLogException(Exception exc, IConfiguration configuration, bool islive, string source = null)
        {
            string connection = string.Empty;
          
            string dbnames = Helper.GetDBName(configuration);
            string passwdKey = configuration["dbSalt"];
            string cipherText = configuration["pwencyped"];
            string userkey = configuration["userSalt"];
            string userText = configuration["userNameEnc"];
            string password = Helper.Decrypt(cipherText, passwdKey);
            string username = Helper.Decrypt(userText, userkey);
            if(islive)
            connection = @"Data Source=192.168.1.78;Initial Catalog=" + dbnames + ";User Id="+ username + ";password=" + password + ";Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";
            else
            connection = @"Data Source=192.168.10.2;Initial Catalog=" + dbnames + ";User Id=" + username + ";password=" + password + ";Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";

            ExceptionLogVM losCodeExceptionLog = new ExceptionLogVM();
            if (exc.InnerException != null)
            {
                losCodeExceptionLog.InnerExeType = exc.InnerException.GetType().ToString().Replace("'", "_");
                losCodeExceptionLog.InnerExeMessage = exc.InnerException.Message.Replace("'", "_");
                losCodeExceptionLog.InnerExeSource = exc.InnerException.Source.Replace("'", "_");
                losCodeExceptionLog.InnerExeStackTrace = exc.InnerException.StackTrace.Replace("'", "_");
            }
            losCodeExceptionLog.ExeType = exc.GetType().ToString().Replace("'", "_");
            losCodeExceptionLog.ExeMessage = exc.Message.Replace("'", "_");
            losCodeExceptionLog.ExeStackTrace = exc.StackTrace != null ? exc.StackTrace.Replace("'", "_") : null;
            losCodeExceptionLog.ExeSource = source;

            string query = @"INSERT INTO LosCodeExceptionLogs (ExeSource,ExeType,ExeMessage,ExeStackTrace,InnerExeSource,InnerExeType,InnerExeMessage,InnerExeStackTrace,CreationDate)
                             VALUES  ('" + losCodeExceptionLog.ExeSource + "','" + losCodeExceptionLog.ExeType + "','" + losCodeExceptionLog.ExeMessage + "','" + losCodeExceptionLog.ExeStackTrace + "','" + losCodeExceptionLog.InnerExeSource + "','" + losCodeExceptionLog.InnerExeType + "','" + losCodeExceptionLog.InnerExeMessage + "','" + losCodeExceptionLog.InnerExeStackTrace + "',GETDATE())";

            using (var con = new SqlConnection(connection))
            {
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    if (con.State == ConnectionState.Closed) con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}
