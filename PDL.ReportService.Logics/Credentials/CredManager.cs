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
            string conStr = string.Empty;

            SqlConnection newConn = new SqlConnection();
            try
            {
                if (!islive)
                    conStr = $"Data Source=192.168.10.2;Initial Catalog={db};User ID=Satish_Dev;Password=Satish@2067;Connection Timeout=120;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";
                else
                    conStr = $"Data Source=192.168.10.2;Initial Catalog={db};User ID=Satish_Dev;Password=Satish@2067;Connection Timeout=120;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";


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
