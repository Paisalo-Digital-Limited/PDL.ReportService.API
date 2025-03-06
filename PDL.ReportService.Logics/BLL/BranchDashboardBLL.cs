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
        public List<BranchDashBoardDataModel> GetBranchDashboardData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, int pageNumber, int pageSize, bool islive)
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
                                        Group_code = reader["Group_code"]?.ToString(),
                                        LoanDuration = reader["Loan_Duration"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Loan_Duration"]),
                                        CreationDate = reader["CreatedOn"] == DBNull.Value ? (DateTime?)null : (DateTime?)reader["CreatedOn"],
                                        Approved = reader["Approved"]?.ToString(),
                                    };

                                    if (Type.ToUpper().Trim() == "SOURCING"|| Type.ToUpper().Trim() == "ALL")
                                    {
                                        dashboardModel.FatherName = reader["FatherName"] as string;
                                        dashboardModel.Income = reader["Income"] != DBNull.Value ? (decimal?)reader["Income"] : null;
                                        dashboardModel.Expense = reader["Expenses"] != DBNull.Value ? Convert.ToDecimal(reader["Expenses"]) : 0;
                                    }
                                    else if (Type.ToUpper().Trim() == "SANCTION" || Type.ToUpper().Trim() == "SANCTIONPENDING" || Type.ToUpper().Trim() == "POSTSANCTION"||Type.ToUpper().Trim()== "READYFORAUDIT"||Type.ToUpper().Trim() == "READYFORNEFT")
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
                using (var cmd = new SqlCommand("Usp_BranchDashboard_TotalDetails", con))
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
        public List<GetCollectionCountVM> GetCollectionCount(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate,string Type, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            List<GetCollectionCountVM> res = new List<GetCollectionCountVM>();
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_BranchDashBoard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    string mode = Type == "Collection" ? "GetCollectionCount" : Type == "AdvanceCollection" ? "GetAdvanceCollectionCount" : null;
                    if (mode != null)
                    {
                        cmd.Parameters.AddWithValue("@Mode", mode);
                        cmd.Parameters.AddWithValue("@CreatorBranchId", CreatorBranchId);
                        cmd.Parameters.AddWithValue("@FromDate", FromDate.HasValue ? (object)FromDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", ToDate.HasValue ? (object)ToDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
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
                            res.Add(new GetCollectionCountVM
                            {
                                FICode = reader["FICode"] == DBNull.Value ? 0 : Convert.ToInt64(reader["FICode"]),
                                FullName = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                                CreatorName = reader["CreatorName"] == DBNull.Value ? null : reader["CreatorName"].ToString(),
                                Branch_code = reader["Branch_code"] == DBNull.Value ? null : reader["Branch_code"].ToString(),
                                SmCode = reader["SmCode"] == DBNull.Value ? null : reader["SmCode"].ToString(),
                                VNO = reader["VNO"] == DBNull.Value ? null : reader["VNO"].ToString(),
                                CR = reader["CR"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["CR"]),
                                VDATE = reader["VDATE"] == DBNull.Value ? null : reader["VDATE"].ToString(),
                            });
                        }
                    }
                    return res;
                }
            }
        }
        public List<GetDemandCountVM> GetDemandCount(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, bool islive)
        {
            string dbname = Helper.Helper.GetDBName(_configuration);
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (var cmd = new SqlCommand("Usp_BranchDashBoard", con))
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
    }
}
