using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Logics.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                                    CreatorID = rdrUser["CreatorID"] != DBNull.Value ? rdrUser["CreatorID"].ToString() : "0"; 
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
                return CreatorID; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
        #region API GetCreators BY--------------- Kartik -------
        public List<FiCreatorMaster> GetCreators(string activeuser, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_GetCreatorsByUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", "135");

                    var creators = new List<FiCreatorMaster>();

                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            creators.Add(new FiCreatorMaster
                            {
                                CreatorID = reader.GetInt32(0),
                                CreatorName = reader.GetString(1)
                            });
                        }
                    }
                    return creators;
                }
            }
        }

        public List<BranchWithCreator> GetBranches(string creatorIds, string activeUser, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_GetBranchesByCreators", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", int.Parse(activeUser));
                    cmd.Parameters.AddWithValue("@CreatorIds", string.IsNullOrEmpty(creatorIds) ? "ALL" : creatorIds);

                    var branches = new List<BranchWithCreator>();
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            branches.Add(new BranchWithCreator
                            {
                                BranchCode = reader.GetString(0),
                                BranchName = reader.GetString(1),
                                CreatorId = reader.IsDBNull(2) ? "ALL" : reader.GetInt32(2).ToString(),
                                CreatorName = reader.IsDBNull(3) ? "" : reader.GetString(3)
                            });
                        }
                    }
                    return branches;
                }
            }
        }

        #endregion
    }
}
