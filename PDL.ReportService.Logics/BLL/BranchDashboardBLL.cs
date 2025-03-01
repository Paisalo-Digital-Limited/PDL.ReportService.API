using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Logics.Credentials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PDL.ReportService.Logics.BLL
{
    public class BranchDashboardBLL:BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;
        public BranchDashboardBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
        }
        #region Api BranchDashboard BY--------------- Satish Maurya-------
        public string GetMasterData(string CreatorID, string BranchCode, DateTime? FromDate, DateTime? ToDate, string activeuser, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);

            try
            {
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    string query = "Usp_BranchDashBoard";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Mode", "GetMasterData");
                        cmd.Parameters.AddWithValue("@CreatorID", CreatorID); 
                        cmd.Parameters.AddWithValue("@BranchCode", BranchCode);
                        cmd.Parameters.AddWithValue("@FromDate", FromDate.HasValue ? (object)FromDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate.HasValue ? (object)ToDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreatedBy", activeuser);

                        con.Open();

                        using (SqlDataReader rdrUser = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (rdrUser.HasRows)
                            {
                                while (rdrUser.Read())
                                {
                                    CreatorID = rdrUser["CreatorID"] != DBNull.Value ? rdrUser["CreatorID"].ToString() : "0"; 
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }
                return CreatorID; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
