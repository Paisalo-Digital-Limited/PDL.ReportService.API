using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
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
    public class BranchDashboardBLL : BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;
        public BranchDashboardBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
        }
        #region Api BranchDashboard BY--------------- Satish Maurya-------
        public BranchDashBoardVM GetMasterData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            BranchDashBoardVM branchDash = new BranchDashBoardVM();
            try
            {
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    string query = "Usp_BranchDashBoard";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Mode", "GetMasterData");
                        cmd.Parameters.AddWithValue("@CreatorBranchId", Convert.ToString(CreatorBranchId));
                        cmd.Parameters.AddWithValue("@FromDate", FromDate.HasValue ? (object)FromDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate.HasValue ? (object)ToDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);

                        con.Open();

                        using (SqlDataReader rdrUser = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (rdrUser.HasRows)
                            {
                                while (rdrUser.Read())
                                {
                                    branchDash.Total_FirstEsign_Count = rdrUser["Total_FirstEsign_Count"] != DBNull.Value ? Convert.ToInt32(rdrUser["Total_FirstEsign_Count"]) : 0;
                                    branchDash.Total_Sanctioned_Count = rdrUser["Total_Sanctioned_Count"] != DBNull.Value ? Convert.ToInt32(rdrUser["Total_Sanctioned_Count"]) : 0;
                                    branchDash.Total_SecondEsign_Count = rdrUser["Total_SecondEsign_Count"] != DBNull.Value ? Convert.ToInt32(rdrUser["Total_SecondEsign_Count"]) : 0;
                                    branchDash.Total_Disbursed_Count = rdrUser["Total_Disbursed_Count"] != DBNull.Value ? Convert.ToInt32(rdrUser["Total_Disbursed_Count"]) : 0;
                                    branchDash.Total_Count = rdrUser["Total_Count"] != DBNull.Value ? Convert.ToInt32(rdrUser["Total_Count"]) : 0;
                                }
                            }
                            else
                            {
                            }
                        }
                        con.Close();
                        cmd.Dispose();
                    }
                }
                return branchDash;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
