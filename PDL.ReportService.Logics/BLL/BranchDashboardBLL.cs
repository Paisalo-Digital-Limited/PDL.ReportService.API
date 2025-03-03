using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Logics.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
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
        #region Collection ---------Satish Maurya----------
        public CollectionStatusVM CollectionStatus(string SmCode, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            DataTable fihq = CollectionStatusFICHQ(SmCode, dbname, islive);
            DataTable dt = new DataTable();
            if (fihq.Rows.Count > 0)
            {
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {

                    string query = "Usp_GetCollectionStatus";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandTimeout = 600;
                        var da = new SqlDataAdapter(cmd);
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "CollectionStatus");
                        cmd.Parameters.AddWithValue("@SmCode", SmCode);

                        da.Fill(dt);
                        con.Close();
                        cmd.Dispose();
                    }
                }
            }

            CollectionStatusVM data = new CollectionStatusVM();
            List<Emi> emilist = new List<Emi>();
            List<EmiCollection> emiCollectionslist = new List<EmiCollection>();

            foreach (DataRow row in fihq.Rows)
            {
                Emi item = new Emi

                {
                    AMT = Convert.ToDecimal(row["AMT"]),
                    PVN_RCP_DT = Convert.ToDateTime(row["PVN_RCP_DT"])
                };
                emilist.Add(item);
            }

            foreach (DataRow row in dt.Rows)
            {
                EmiCollection item = new EmiCollection
                {
                    CR = Convert.ToDecimal(row["CR"]),
                    VDATE = Convert.ToDateTime(row["VDATE"])
                };
                emiCollectionslist.Add(item);
            }
            data.emis = emilist;
            data.emiCollections = emiCollectionslist;
            return data;
        }
        public DataTable CollectionStatusFICHQ(string SmCode, string dbname, bool islive)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {

                string query = "Usp_GetCollectionStatus";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandTimeout = 600;
                    var da = new SqlDataAdapter(cmd);
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "CollectionStatusFICHQ");
                    cmd.Parameters.AddWithValue("@SmCode", SmCode);

                    da.Fill(dt);
                    con.Close();
                    cmd.Dispose();
                }
            }
            return dt;
        }
        #endregion
        #region Api BranchDashboard Count BY--------------- Satish Maurya-------
        public List<BranchDashBoardDataModel> GetBranchDashboardData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            List<BranchDashBoardDataModel> dashboardList = new List<BranchDashBoardDataModel>();
            try
            {
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    string query = "Usp_BranchDashBoard";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Mode", "GetBranchDashboardData");
                        cmd.Parameters.AddWithValue("@CreatorBranchId", Convert.ToString(CreatorBranchId));
                        cmd.Parameters.AddWithValue("@FromDate", FromDate.HasValue ? (object)FromDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate.HasValue ? (object)ToDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Type", Type);

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var dashboardModel = new BranchDashBoardDataModel
                                    {
                                        FullName = reader["Full_Name"]?.ToString(),
                                        CreatorName = reader["CreatorName"]?.ToString(),
                                        FICode = reader["FICode"]?.ToString(),
                                        SmCode = reader["SmCode"]?.ToString(),
                                        Current_City = reader["Current_City"]?.ToString(),
                                        Group_code = reader["Group_code"]?.ToString(),
                                        LoanDuration = Convert.ToInt32(reader["Loan_Duration"]),
                                        CreationDate = reader["CreatedOn"] == DBNull.Value ? (DateTime?)null : (DateTime?)reader["CreatedOn"],
                                        Approved = reader["Approved"]?.ToString(),
                                    };

                                    if (Type.ToUpper().Trim() == "SOURCING")
                                    {
                                        dashboardModel.FatherName = reader["FatherName"] as string;
                                        dashboardModel.Income = reader["Income"] != DBNull.Value ? (decimal?)reader["Income"] : null;
                                        dashboardModel.Expense = reader["Expenses"] != DBNull.Value ? Convert.ToDecimal(reader["Expenses"]) : 0; // Default to 0 if DBNull
                                    }
                                    dashboardList.Add(dashboardModel);
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
                return dashboardList;
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
                                CreatorID = reader["CreatorID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CreatorID"]),
                                CreatorName = reader["CreatorName"] == DBNull.Value ? null : reader["CreatorName"]?.ToString()
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
                                BranchCode = reader["BranchCode"] == DBNull.Value ? null : reader["BranchCode"]?.ToString(),
                                BranchName = reader["BranchName"] == DBNull.Value ? null : reader["BranchName"]?.ToString(),
                                CreatorId = reader["CreatorID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CreatorID"]),
                                CreatorName = reader["CreatorName"] == DBNull.Value ? null : reader["CreatorName"]?.ToString()
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
