using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Logics.BLL;


namespace PDL.ReportService.Logics.Credentials
{
    public class CredManager : BaseBLL
    {
        private IConfiguration _configuration;
        public CredManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public SqlConnection getConnections(string db, bool islive)
        {
            var config = _configuration.GetSection("encryptSalts");
            var dbip = _configuration.GetSection("DB_IP").Value;



            string conStr = string.Empty;
            string passwdKey = config["dbSalt"];
            string cipherText = config["pwencyped"];
            string userkey = config["userSalt"];
            string userText = config["userNameEnc"];


            SqlConnection newConn = new SqlConnection();
            try
            {
                string password = Helper.Helper.Decrypt(cipherText, passwdKey);
                string username = Helper.Helper.Decrypt(userText, userkey);

                if (!islive)
                    conStr = $"Data Source={dbip};Initial Catalog={db};User ID={username};Password={password};Connection Timeout=120;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";
                else
                    conStr = $"Data Source={dbip};Initial Catalog={db};User ID={username};Password={password};Connection Timeout=120;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";
                newConn = new SqlConnection(conStr);
                return newConn;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL error: " + ex.Message);
                throw;
            }
        }
        public string getConnectionString(string db, bool islive)
        {
            var config = _configuration.GetSection("encryptSalts");
            var dbip = _configuration.GetSection("DB_IP").Value;
            string conStr = string.Empty;
            string passwdKey = config["dbSalt"];
            string cipherText = config["pwencyped"];
            string userkey = config["userSalt"];
            string userText = config["userNameEnc"];

            SqlConnection newConn = new SqlConnection();

            try
            {
                string password = Helper.Helper.Decrypt(cipherText, passwdKey);
                string username = Helper.Helper.Decrypt(userText, userkey);

                if (!islive)
                    conStr = $"Data Source={dbip};Initial Catalog={db};User ID={username};Password={password};Connection Timeout=120;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";
                else
                    conStr = $"Data Source={dbip};Initial Catalog={db};User ID={username};Password={password};Connection Timeout=120;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";
                return conStr;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL error: " + ex.Message);
                throw;
            }
        }
        public SqlConnection getConnectionPDL(string db, bool islive)
        {
            string conStr = string.Empty;
            SqlConnection newConn = new SqlConnection();

            try
            {
                if (!islive)
                    conStr = $"Data Source=BETA;Initial Catalog=PDLSHARECOL;User Id=sa;password=Sql@2019;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false;Connect Timeout=0";
                else
                    conStr = $"Data Source=BETA;Initial Catalog=PDLSHARECOL;User Id=sa;password=Sql@2019;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false;Connect Timeout=0";

                newConn = new SqlConnection(conStr);
                return newConn;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL error: " + ex.Message);
                throw;
            }
        }
    }
}
