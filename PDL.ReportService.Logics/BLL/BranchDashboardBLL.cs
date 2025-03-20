using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Logics.Credentials;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace PDL.ReportService.Logics.BLL
{
    public class BranchDashboardBLL : BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;
        private readonly object IWebHostEnvironment;

        public BranchDashboardBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
        }
        #region Api BranchDashboard BY--------------- Satish Maurya-------
        public BranchDashBoardVM GetMasterData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string? flag, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            string query = null;
            BranchDashBoardVM branchDash = new BranchDashBoardVM();
            try
            {
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    //string query = "Usp_BranchDashBoard";

                    if (flag == "All")
                    {
                        query = "Usp_BranchDashBoardAllCreator";
                    }
                    else
                    {
                        query = "Usp_BranchDashBoard";
                    }
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
                                    branchDash.Total_FirstEsign_Count = rdrUser["Total_FirstEsign_Count"] != DBNull.Value ? Convert.ToInt64(rdrUser["Total_FirstEsign_Count"]) : 0;
                                    branchDash.Total_Sanctioned_Count = rdrUser["Total_Sanctioned_Count"] != DBNull.Value ? Convert.ToInt64(rdrUser["Total_Sanctioned_Count"]) : 0;
                                    branchDash.Total_SecondEsign_Count = rdrUser["Total_SecondEsign_Count"] != DBNull.Value ? Convert.ToInt64(rdrUser["Total_SecondEsign_Count"]) : 0;
                                    branchDash.Total_Disbursed_Count = rdrUser["Total_Disbursed_Count"] != DBNull.Value ? Convert.ToInt64(rdrUser["Total_Disbursed_Count"]) : 0;
                                    branchDash.Total_Count = rdrUser["Total_Count"] != DBNull.Value ? Convert.ToInt64(rdrUser["Total_Count"]) : 0;
                                    branchDash.Total_PostSanction_Count = rdrUser["Total_PostSanction_Count"] != DBNull.Value ? Convert.ToInt64(rdrUser["Total_PostSanction_Count"]) : 0;
                                    branchDash.Total_ReadyForAudit_Count = rdrUser["Total_ReadyForAudit_Count"] != DBNull.Value ? Convert.ToInt64(rdrUser["Total_ReadyForAudit_Count"]) : 0;
                                    branchDash.Total_ReadyForNeft_Count = rdrUser["Total_ReadyForNeft_Count"] != DBNull.Value ? Convert.ToInt64(rdrUser["Total_ReadyForNeft_Count"]) : 0;
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
        public List<BranchDashBoardDataModel> GetBranchDashboardData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, int pageNumber, int pageSize, string? flag, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            List<BranchDashBoardDataModel> dashboardList = new List<BranchDashBoardDataModel>();
            try
            {
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    string query = null;
                    if (flag == "All")
                    {
                        query = "Usp_BranchDashBoardAllCreator";
                    }
                    else
                    {
                        query = "Usp_BranchDashBoard";
                    }
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Mode", "GetBranchDashboardData");
                        cmd.Parameters.AddWithValue("@CreatorBranchId", Convert.ToString(CreatorBranchId));
                        cmd.Parameters.AddWithValue("@FromDate", FromDate.HasValue ? (object)FromDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate.HasValue ? (object)ToDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Type", Type);
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var dashboardModel = new BranchDashBoardDataModel
                                    {
                                        Fi_Id = reader["Fi_Id"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Fi_Id"]),
                                        FullName = reader["Full_Name"]?.ToString(),
                                        CreatorName = reader["CreatorName"]?.ToString(),
                                        FICode = reader["FICode"]?.ToString(),
                                        SmCode = reader["SmCode"]?.ToString(),
                                        Current_City = reader["Current_City"]?.ToString(),
                                        Branch_Code = reader["Branch_code"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Branch_code"]),
                                        Branch_Name = reader["BranchName"]?.ToString(),
                                        Group_code = reader["Group_code"]?.ToString(),
                                        LoanDuration = reader["Loan_Duration"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Loan_Duration"]),
                                        CreationDate = reader["CreatedOn"] == DBNull.Value ? (DateTime?)null : (DateTime?)reader["CreatedOn"],
                                        Approved = reader["Approved"]?.ToString(),
                                    };

                                    if (Type.ToUpper().Trim() == "SOURCING" || Type.ToUpper().Trim() == "ALL")
                                    {
                                        dashboardModel.FatherName = reader["FatherName"] as string;
                                        dashboardModel.Income = reader["Income"] != DBNull.Value ? (decimal?)reader["Income"] : null;
                                        dashboardModel.Expense = reader["Expenses"] != DBNull.Value ? Convert.ToDecimal(reader["Expenses"]) : 0;
                                    }
                                    else if (Type.ToUpper().Trim() == "SANCTION" || Type.ToUpper().Trim() == "SANCTIONPENDING" || Type.ToUpper().Trim() == "POSTSANCTION" || Type.ToUpper().Trim() == "READYFORAUDIT" || Type.ToUpper().Trim() == "READYFORNEFT")
                                    {
                                        dashboardModel.SchCode = reader["SchCode"]?.ToString();
                                        dashboardModel.Bank_IFCS = reader["Bank_IFCS"]?.ToString();
                                        dashboardModel.Bank_Ac = reader["Bank_Ac"]?.ToString();
                                        dashboardModel.SanctionedAmt = reader["SanctionedAmt"] as decimal?;
                                        dashboardModel.DtFin = reader["Dt_Fin"] == DBNull.Value ? (DateTime?)null : (DateTime?)reader["Dt_Fin"];
                                    }
                                    else if (Type.ToUpper().Trim() == "SECONDESIGN" || Type.ToUpper().Trim() == "SECONDESIGNPENDING")
                                    {
                                        dashboardModel.Loan_amount = reader["Loan_amount"] as decimal?;
                                    }
                                    else if (Type.ToUpper().Trim() == "DISBURSED")
                                    {
                                        dashboardModel.InstAmt = Convert.ToDecimal(reader["INST_AMT"]);
                                        dashboardModel.Invest = Convert.ToDecimal(reader["INVEST"]);
                                        dashboardModel.DtFin = reader["Dt_Fin"] == DBNull.Value ? (DateTime?)null : (DateTime?)reader["Dt_Fin"];
                                        dashboardModel.DtPos = reader["DT_POS"] == DBNull.Value ? (DateTime?)null : (DateTime?)reader["DT_POS"];

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
            // Validate activeuser
            if (!int.TryParse(activeuser, out int userId) || userId <= 0)
            {
                return new List<FiCreatorMaster>
                {
                   new FiCreatorMaster
                   {
                       CreatorID = -1,
                       CreatorName = "Invalid user ID."
                   }
                };
            }

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_GetCreatorsByUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", activeuser);

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
                    cmd.Parameters.AddWithValue("@CreatorIds", creatorIds);

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
        #region BranchDashboard ChatBot query Api  BY--------------- Satish Maurya-------
        public List<GetFirstEsign> GetFirstEsign(int CreatorId, long FiCode, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_BranchDashBoard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GETFIRSTESIGN");
                    cmd.Parameters.AddWithValue("@CreatorId", CreatorId);
                    cmd.Parameters.AddWithValue("@FiCode", FiCode);

                    var res = new List<GetFirstEsign>();
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(new GetFirstEsign
                            {
                                BorrSignStatus = reader["BorrSignStatus"] == DBNull.Value ? null : reader["BorrSignStatus"]?.ToString(),
                                Creation_Date = reader["Creation_Date"] == DBNull.Value ? (DateTime?)null : (DateTime?)reader["Creation_Date"]

                            });
                        }
                    }
                    return res;
                }
            }
        }
        public List<GetSecoundEsign> GetSecoundEsign(int CreatorId, long FiCode, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_BranchDashBoard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GETSECOUNDESIGN");
                    cmd.Parameters.AddWithValue("@CreatorId", CreatorId);
                    cmd.Parameters.AddWithValue("@FiCode", FiCode);

                    var res = new List<GetSecoundEsign>();
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(new GetSecoundEsign
                            {
                                Download_One_Pager_Status = reader["Download_One_Pager_Status"] == DBNull.Value ? null : reader["Download_One_Pager_Status"]?.ToString(),
                                Eligible_CSO_Id = reader["Eligible_CSO_Id"] == DBNull.Value ? null : reader["Eligible_CSO_Id"]?.ToString(),
                                Esign_Applicable_Status = reader["Esign_Applicable_Status"] == DBNull.Value ? null : reader["Esign_Applicable_Status"]?.ToString(),

                            });
                        }
                    }
                    return res;
                }
            }
        }
        public object GetCaseNotVisible(int CreatorId, long FiCode, bool islive)
        {
            string query = "Usp_BranchDashBoard";
            string dbname = Helper.Helper.GetDBName(_configuration);
            DataTable dataTable = new DataTable();
            int data = 0;
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand(query, con))
                {
                    DataTable dt = new DataTable();
                    var da = new SqlDataAdapter(cmd);
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "CHECKONEPAGER");
                    cmd.Parameters.AddWithValue("@CreatorId", CreatorId);
                    cmd.Parameters.AddWithValue("@FiCode", FiCode);

                    da.Fill(dt);
                    con.Close();
                    cmd.Dispose();
                    var smcode = dt.Rows[0]["SmCode"];

                    if (smcode == DBNull.Value || string.IsNullOrEmpty(smcode.ToString()))
                    {
                        return data = -1;
                    }
                    else
                    {
                        DataTable dtt = new DataTable();
                        using (var cmdd = new SqlCommand(query, con))
                        {
                            if (con.State == ConnectionState.Closed)
                                con.Open();
                            cmdd.CommandType = CommandType.StoredProcedure;
                            cmdd.Parameters.AddWithValue("@Mode", "GETIMEINO");
                            cmd.Parameters.AddWithValue("@CreatorId", CreatorId);
                            cmd.Parameters.AddWithValue("@FiCode", FiCode);

                            SqlDataAdapter dsa = new SqlDataAdapter(cmdd);
                            dsa.Fill(dtt);

                            con.Close();
                            cmdd.Dispose();
                        }

                        if (dtt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtt.Rows)
                            {
                                var IMEINO = row["IMEINO"];
                                var GroupCode = row["Group_code"];
                                var CreatorID = row["CreatorID"];

                                if (IMEINO == DBNull.Value || string.IsNullOrEmpty(IMEINO.ToString()))
                                {
                                    return data = -2;
                                }
                                else
                                {

                                    using (var command = new SqlCommand("GetPendingESignData", con))
                                    {
                                        if (con.State == ConnectionState.Closed)
                                            con.Open();
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@CreatorID", CreatorID);
                                        command.Parameters.AddWithValue("@GroupCode", GroupCode);
                                        command.Parameters.AddWithValue("@IMEINO", IMEINO);

                                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                                        dataAdapter.Fill(dataTable);

                                        con.Close();
                                        command.Dispose();
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return dataTable;
        }
        #endregion
        #region Api BranchDashboard TotalDemand && TotalCollection BY--------------- Satish Maurya-------
        public List<TotalDemandAndCollection> GetTotalDemandAndCollection(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_BranchDashboardTotalDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CreatorBranchId", Convert.ToString(CreatorBranchId));
                    cmd.Parameters.AddWithValue("@StartDate", FromDate.HasValue ? (object)FromDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate", ToDate.HasValue ? (object)ToDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                    var res = new List<TotalDemandAndCollection>();
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(new TotalDemandAndCollection
                            {
                                TotalDemand = reader["TotalDemand"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["TotalDemand"]),
                                TotalCollection = reader["TotalCollection"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["TotalCollection"]),
                                AdvanceCollection = reader["AdvanceCollection"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["AdvanceCollection"]),
                                OD = reader["OD"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["OD"]),
                                TotalEfficiency = reader["TotalEfficiency"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["TotalEfficiency"])
                            });
                        }
                    }
                    return res;
                }
            }
        }
        public List<GetCollectionCountVM> GetCollectionCount(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, int pageNumber, int pageSize, string? flag, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            string query = null;
            if (flag == "All")
            {
                query = "Usp_BranchDashBoardAllCreator";
            }
            else
            {
                query = "Usp_BranchDashBoard";
            }
            List<GetCollectionCountVM> res = new List<GetCollectionCountVM>();

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    string mode = Type == "Collection" ? "GetCollectionCount" :
                                         Type == "AdvanceCollection" ? "GetAdvanceCollectionCount" :
                                         Type == "OverDue" ? "GetOverDue" : null;

                    if (mode != null)
                    {
                        cmd.Parameters.AddWithValue("@Mode", mode);
                        cmd.Parameters.AddWithValue("@CreatorBranchId", CreatorBranchId);
                        cmd.Parameters.AddWithValue("@FromDate", FromDate.HasValue ? (object)FromDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate.HasValue ? (object)ToDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                    }
                    else
                    {
                        return res;
                    }
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var collectionItem = new GetCollectionCountVM
                            {
                                Fi_Id = reader["Fi_Id"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Fi_Id"]),
                                FICode = reader["FICode"] == DBNull.Value ? 0 : Convert.ToInt64(reader["FICode"]),
                                FullName = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                                CreatorName = reader["CreatorName"] == DBNull.Value ? null : reader["CreatorName"].ToString(),
                                Branch_code = reader["Branch_code"] == DBNull.Value ? null : reader["Branch_code"].ToString(),
                                SmCode = reader["SmCode"] == DBNull.Value ? null : reader["SmCode"].ToString(),
                                VNO = reader["VNO"] == DBNull.Value ? null : reader["VNO"].ToString(),
                                Branch_Name = reader["BranchName"] == DBNull.Value ? null : reader["BranchName"].ToString(),
                                Count = reader["TotalCount"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["TotalCount"]),
                                CR = reader["CR"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["CR"]),
                                VDATE = reader["VDATE"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["VDATE"]) : null,
                            };
                            if (Type == "AdvanceCollection")
                            {
                                collectionItem.TotalDemand = reader["TotalDemand"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["TotalDemand"]);
                                collectionItem.TotalCollection = reader["TotalCollection"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["TotalCollection"]);
                                collectionItem.AdvanceCollection = reader["Result"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["Result"]);
                                //collectionItem.Count = reader["TotalCount"] == DBNull.Value ? 0 : Convert.ToInt64(reader["TotalCount"]);
                            }
                            else if (Type == "OverDue")
                            {
                                collectionItem.OD = reader["OD"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["OD"]);
                            }
                            else
                            {
                                collectionItem.TotalDemand = null;
                                collectionItem.TotalCollection = null;
                                collectionItem.AdvanceCollection = null;
                                collectionItem.OD = null;
                                collectionItem.Count = null;

                            }
                            res.Add(collectionItem);

                        }
                        return res;
                    }
                }
            }
        }
        public List<GetDemandCountVM> GetDemandCount(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string? flag, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            string query = null;
            if (flag == "All")
            {
                query = "Usp_BranchDashBoardAllCreator";
            }
            else
            {
                query = "Usp_BranchDashBoard";
            }
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetDemandCount");
                    cmd.Parameters.AddWithValue("@CreatorBranchId", Convert.ToString(CreatorBranchId));
                    cmd.Parameters.AddWithValue("@FromDate", FromDate.HasValue ? (object)FromDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToDate", ToDate.HasValue ? (object)ToDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                    var res = new List<GetDemandCountVM>();
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(new GetDemandCountVM
                            {
                                FICode = reader["FICode"] == DBNull.Value ? 0 : Convert.ToInt64(reader["FICode"]),
                                FullName = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                                CreatorName = reader["CreatorName"] == DBNull.Value ? null : reader["CreatorName"].ToString(),
                                Branch_code = reader["Branch_code"] == DBNull.Value ? null : reader["Branch_code"].ToString(),
                                SmCode = reader["SmCode"] == DBNull.Value ? null : reader["SmCode"].ToString(),
                                INSTALL = reader["INSTALL"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["INSTALL"]),
                                AMT = reader["AMT"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["AMT"]),
                                PVN_RCP_DT = reader["PVN_RCP_DT"] == DBNull.Value ? null : reader["PVN_RCP_DT"].ToString()
                            });
                        }
                    }
                    return res;
                }
            }
        }
        #endregion
        public List<RaiseQueryVM> GetRaiseQuery(string activeuser, bool islive)
        {
            List<RaiseQueryVM> raiseQueries = new List<RaiseQueryVM>();
            string dbname = Helper.Helper.GetDBName(_configuration);
            string path = _configuration["filePath"];
            string sftpHost = _configuration["SftpHost"];
            string sftpUsername = _configuration["SftpUsername"];
            string sftpPassword = _configuration["SftpPassword"];
            string networkPath = _configuration["NetworkPath"];
            string newBaseUrl = _configuration["NewBaseUrl"];

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_InsertRaiseQuery", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", activeuser);
                    cmd.Parameters.AddWithValue("@Mode", "GetRaiseQuery");


                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RaiseQueryVM data = new RaiseQueryVM();
                            {
                                data.Name = reader["Name"] == DBNull.Value ? null : reader["Name"]?.ToString();
                                data.Type = reader["Type"] == DBNull.Value ? null : reader["Type"]?.ToString();
                                data.Query = reader["Query"] == DBNull.Value ? null : reader["Query"]?.ToString();
                                data.Reply = reader["Reply"] == DBNull.Value ? null : reader["Reply"]?.ToString();
                                data.ImagPath = reader["ErrorImage"] == DBNull.Value ? null : reader["ErrorImage"]?.ToString();
                                data.Createdon = reader["Createdon"] != DBNull.Value ? Convert.ToDateTime(reader["Createdon"]) : null;
                            };

                            if (data.ImagPath != null)
                            {
                                using (var sftp = new SftpClient(sftpHost, sftpUsername, sftpPassword))
                                {
                                    sftp.Connect();

                                    string networkFilePath = $"{newBaseUrl}/FiDocs/{activeuser}/{data.ImagPath}";

                                    data.ImagPath = networkFilePath;

                                    sftp.Disconnect();
                                }
                            }
                            else
                            {
                                data.ImagPath = "";
                            }

                            raiseQueries.Add(data);
                        }
                    }
                    con.Close();

                    return raiseQueries;
                }
            }
        }
        public int InsertRaiseQuery(RaiseQueryVM obj, string activeuser, bool islive)
        {
            int affected = 0;
            string sftpUsername = _configuration["SftpUsername"];
            string sftpHost = _configuration["SftpHost"];
            string sftpPassword = _configuration["SftpPassword"];
            string sftpBasePath = _configuration["SftpfilePath"];
            string dbname = Helper.Helper.GetDBName(_configuration); ;

            string fileName = obj.Imag.FileName;

            string query = "Usp_InsertRaiseQuery";

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", "InsertRaiseQuery");
                    cmd.Parameters.Add("@Query", SqlDbType.VarChar).Value = obj.Query;
                    cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = obj.Type;
                    cmd.Parameters.Add("@Img", SqlDbType.VarChar).Value = fileName;
                    cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = activeuser;

                    con.Open();
                    affected = cmd.ExecuteNonQuery();
                }
                if (affected > 0)
                {
                    string folderName = $"{activeuser}";
                    string remoteDir = $"/Data/FiDocs/{activeuser}"; // Target directory
                    string remoteFilePath = $"{remoteDir}/{obj.Imag.FileName}";
                    using var memoryStream = new MemoryStream();
                    obj.Imag.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    using var sftp = new SftpClient(sftpHost, sftpUsername, sftpPassword);
                    try
                    {
                        sftp.Connect();
                        // Ensure each level of directory exists
                        Helper.Helper.EnsureDirectoryExists(sftp, remoteDir);
                        // Upload file
                        memoryStream.Position = 0;
                        sftp.UploadFile(memoryStream, remoteFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    finally
                    {
                        sftp.Disconnect();
                    }
                }
            }

            return affected;
        }
        public string RequestForDeath(RequestForDeathVM obj, string activeuser, bool islive)
        {
            int affected = 0;
            string sftpUsername = _configuration["SftpUsername"];
            string sftpHost = _configuration["SftpHost"];
            string sftpPassword = _configuration["SftpPassword"];
            string dbname = Helper.Helper.GetDBName(_configuration);
            List<string> savedFileNames = new List<string>();
            string query = "Usp_InsertRequestForDeathForm";
            string transactionid = null;

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                foreach (var file in obj.DeathFiles)
                {
                    if (file.Length > 0)
                    {
                        string fileName = $"{obj.Type}{file.FileName}";
                        savedFileNames.Add(fileName);
                    }
                }
                string allFileNames = string.Join(",", savedFileNames);

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SmCode", SqlDbType.VarChar).Value = obj.SmCode;
                    cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = obj.Type;
                    cmd.Parameters.Add("@Query", SqlDbType.VarChar).Value = obj.Query;
                    cmd.Parameters.Add("@Img", SqlDbType.VarChar).Value = allFileNames;
                    cmd.Parameters.Add("@RequestBy", SqlDbType.Int).Value = activeuser;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = activeuser;

                    SqlParameter outputParam = new SqlParameter("@TransactionId", SqlDbType.VarChar, 20)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    con.Open();
                    affected = cmd.ExecuteNonQuery();

                    transactionid = outputParam.Value.ToString();
                    //obj.SmCode = TransactionId; // Update the object with the generated Smcode
                }

                if (affected > 0)
                {
                    string remoteDir = $"/Data/FiDocs/{activeuser}";
                    using (var sftp = new SftpClient(sftpHost, sftpUsername, sftpPassword))
                    {
                        try
                        {
                            sftp.Connect();
                            Helper.Helper.EnsureDirectoryExists(sftp, remoteDir);
                            foreach (var file in obj.DeathFiles)
                            {
                                if (file.Length > 0)
                                {
                                    string remoteFilePath = $"{remoteDir}/{obj.Type}{file.FileName}";
                                    using (var memoryStream = new MemoryStream())
                                    {
                                        file.CopyTo(memoryStream);
                                        memoryStream.Position = 0;
                                        sftp.UploadFile(memoryStream, remoteFilePath);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error during SFTP upload: {ex.Message}");
                        }
                        finally
                        {
                            sftp.Disconnect();
                        }
                    }
                }
            }

            return transactionid;
        }
        public int NOCQuery(NOCQueryVM obj, string activeuser, bool islive)
        {
            int affected = 0;
            string sftpUsername = _configuration["SftpUsername"];
            string sftpHost = _configuration["SftpHost"];
            string sftpPassword = _configuration["SftpPassword"];
            string sftpBasePath = _configuration["SftpfilePath"];
            string dbname = Helper.Helper.GetDBName(_configuration);

            string fileName = obj.Image.FileName;

            string query = "Usp_InsertNOC";

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", "InsertNOCQuery");
                    cmd.Parameters.Add("@Query", SqlDbType.VarChar).Value = obj.Query;
                    cmd.Parameters.Add("@Smcode", SqlDbType.VarChar).Value = obj.SmCode;
                    cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = "NOC";
                    cmd.Parameters.Add("@Img", SqlDbType.VarChar).Value = fileName;
                    cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = activeuser;

                    con.Open();
                    affected = cmd.ExecuteNonQuery();
                }
                if (affected > 0)
                {
                    string folderName = $"{obj.SmCode}";
                    string remoteDir = $"/Data/FiDocs/{folderName}";
                    string remoteFilePath = $"{remoteDir}/{fileName}";
                    using var memoryStream = new MemoryStream();
                    obj.Image.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    using var sftp = new SftpClient(sftpHost, sftpUsername, sftpPassword);

                    sftp.Connect();
                    Helper.Helper.EnsureDirectoryExists(sftp, remoteDir);
                    memoryStream.Position = 0;
                    sftp.UploadFile(memoryStream, remoteFilePath);

                    sftp.Disconnect();
                }
            }

            return affected;
        }

        public List<ReadyForPuchVM> GetReadyforPushData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, int pageNumber, int pageSize, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_GetReadyForPuchData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CreatorBranchId", Convert.ToString(CreatorBranchId));
                    cmd.Parameters.AddWithValue("@StartDate", FromDate.HasValue ? (object)FromDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate", ToDate.HasValue ? (object)ToDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    var res = new List<ReadyForPuchVM>();
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(new ReadyForPuchVM
                            {
                                ID = reader["ID"] == DBNull.Value ? 0 : Convert.ToInt64(reader["ID"]),
                                FullName = reader["Full_Name"] == DBNull.Value ? string.Empty : reader["Full_Name"].ToString(),
                                FICode = reader["FICode"] == DBNull.Value ? string.Empty : reader["FICode"].ToString(),
                                Creator = reader["Creator"] == DBNull.Value ? string.Empty : reader["Creator"].ToString(),
                                Branch_code = reader["Branch_code"] == DBNull.Value ? string.Empty : reader["Branch_code"].ToString(),
                                Group_code = reader["Group_code"] == DBNull.Value ? string.Empty : reader["Group_code"].ToString(),
                                Loan_amount = reader["Loan_amount"] == DBNull.Value ? 0 : Convert.ToSingle(reader["Loan_amount"]),
                                Date = reader["Creation_Date"] == DBNull.Value ? string.Empty : reader["Creation_Date"].ToString()
                            });
                        }
                    }
                    return res;
                }
            }
        }

        public int ReadyforPushData(long id, string activeuser, bool islive)
        {
            int affected = 0;
            string dbname = Helper.Helper.GetDBName(_configuration);
            string query = "Usp_UpdateFiCurrentStatus";

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = activeuser;

                    con.Open();
                    affected = cmd.ExecuteNonQuery();
                }
            }

            return affected;
        }
        #region  Notification Api  BY--------------- Satish Maurya-------
        public List<GetNotificationVM> GetNotification(string activeuser, bool islive)
        {
            DataSet dataSet = new DataSet();
            List<GetNotificationVM> getNotifications = new List<GetNotificationVM>();
            string dbname = Helper.Helper.GetDBName(_configuration);

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_GetNotification", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetNotification");
                    cmd.Parameters.AddWithValue("@UserId", activeuser);

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataSet);
                    }

                    if (dataSet.Tables.Count > 0)
                    {
                        DataTable dt1 = dataSet.Tables[0];
                        foreach (DataRow row in dt1.Rows)
                        {
                            NotificationDOBVM notificationDOB = new NotificationDOBVM
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Name = row["Name"].ToString(),
                                DepartmentName = row["DepartmentName"].ToString(),
                                DesignationName = row["DesignationName"].ToString(),
                                DOB = row["DOB"] != DBNull.Value ? Convert.ToDateTime(row["DOB"]).ToString("yyyy-MM-dd") : null,
                                NotificationType = row["NotificationType"] != DBNull.Value ? Convert.ToInt32(row["NotificationType"]) : (int?)null,
                                BirthDayStatus = row["BirthDayStatus"].ToString(),
                            };
                            getNotifications.Add(new GetNotificationVM { NotificationDOBS = new List<NotificationDOBVM> { notificationDOB } });
                        }
                    }

                    if (dataSet.Tables.Count > 1)
                    {
                        DataTable dt2 = dataSet.Tables[1];
                        foreach (DataRow row in dt2.Rows)
                        {
                            NotificationInternShip notificationInternShip = new NotificationInternShip
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Name = row["Name"].ToString(),
                                DepartmentName = row["DepartmentName"].ToString(),
                                DesignationName = row["DesignationName"].ToString(),
                                NotificationTypeName = row["NoticePeriodName"].ToString(),
                                Message = row["Message"].ToString(),
                                NotificationType = row["NotificationType"] != DBNull.Value ? Convert.ToInt32(row["NotificationType"]) : (int?)null,
                                InternShipEndDate = row["InternShipEndDate"] != DBNull.Value ? Convert.ToDateTime(row["InternShipEndDate"]).ToString("yyyy-MM-dd") : null,
                                JoiningDate = row["JoiningDate"] != DBNull.Value ? Convert.ToDateTime(row["JoiningDate"]).ToString("yyyy-MM-dd") : null,
                                IsRead = row["IsRead"] != DBNull.Value ? Convert.ToBoolean(row["IsRead"]) : null,
                            };
                            getNotifications.Add(new GetNotificationVM { NotificationInternShips = new List<NotificationInternShip> { notificationInternShip } });
                        }
                    }

                    if (dataSet.Tables.Count > 2)
                    {
                        DataTable dt3 = dataSet.Tables[2];
                        foreach (DataRow row in dt3.Rows)
                        {
                            NotificationNoticePeriod notificationNoticePeriod = new NotificationNoticePeriod
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Name = row["Name"].ToString(),
                                DepartmentName = row["DepartmentName"].ToString(),
                                DesignationName = row["DesignationName"].ToString(),
                                NotificationTypeName = row["NoticePeriodName"].ToString(),
                                Message = row["Message"].ToString(),
                                NotificationType = Convert.ToInt32(row["NotificationType"]),
                                NoticePeriodEndDate = row["NoticePeriodEndDate"] != DBNull.Value ? Convert.ToDateTime(row["NoticePeriodEndDate"]).ToString("yyyy-MM-dd") : null,
                                IsRead = row["IsRead"] != DBNull.Value ? Convert.ToBoolean(row["IsRead"]) : null,
                            };
                            getNotifications.Add(new GetNotificationVM { NotificationNoticePeriods = new List<NotificationNoticePeriod> { notificationNoticePeriod } });
                        }
                    }

                    con.Close();
                    return getNotifications;
                }
            }
        }

        #endregion    
        #region HolidayCalendar Api  BY--------------- Satish Maurya-------
        public List<GetHolidayCalendarVM> GetHolidayCalendar(bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_BranchDashBoard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetHolidayCalendar");
                    var res = new List<GetHolidayCalendarVM>();
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            res.Add(new GetHolidayCalendarVM
                            {
                                HolydayID = reader["HolydayID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["HolydayID"]),
                                HolydayName = reader["HolydayName"] == DBNull.Value ? null : reader["HolydayName"]?.ToString(),
                                Description = reader["Description"] == DBNull.Value ? null : reader["Description"]?.ToString(),
                                Type = reader["Type"] == DBNull.Value ? null : reader["Type"]?.ToString(),
                                IsPublicHolyday = reader["IsPublicHolyday"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsPublicHolyday"]),
                                HolydayDate = reader["HolydayDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["HolydayDate"]) : null,
                            });
                        }
                    }
                    return res;
                }
            }
        }
        public int ViewNotification(ViewNotificationVM obj, string activeuser, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            int affectedRows = 0;

            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                string storedProcedure = "Usp_GetNotification";

                using (SqlCommand cmd = new SqlCommand(storedProcedure, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", "UpdateNotification");
                    cmd.Parameters.Add("@NotificationType", SqlDbType.Int).Value = obj.NotificationType;
                    cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = activeuser;

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }
        #endregion
    }
}