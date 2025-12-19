using ClosedXML.Excel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Entites.VM.ReportVM;
using PDL.ReportService.Logics.Credentials;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;


namespace PDL.ReportService.Logics.BLL
{
    public class ReportsBLL : BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;

        public ReportsBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
        }

        public CaseHistoryResponse GetCaseHistoryBySmCodes(List<string> smCodes, string dbName, bool isLive, int pageNumber, int pageSize)
        {
            var response = new CaseHistoryResponse
            {
                CaseHistories = new List<CaseHistoryVM>(),
                TotalCount = 0,
                InvalidSmCodeCount = 0,
                NoHistoryCount = 0
            };

            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("usp_GetCaseHistoryBySmCode", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Table-valued parameter setup
                    var table = new DataTable();
                    table.Columns.Add("SmCode", typeof(string));
                    foreach (var code in smCodes)
                        table.Rows.Add(code);

                    cmd.Parameters.AddWithValue("@SmCodes", table).SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // 1️⃣ First result: Invalid SM Code Count
                        if (reader.Read())
                        {
                            response.InvalidSmCodeCount = reader["InvalidSmCodeCount"] != DBNull.Value
                                ? Convert.ToInt32(reader["InvalidSmCodeCount"])
                                : 0;
                        }

                        // 2️⃣ Second result: No History Count
                        if (reader.NextResult() && reader.Read())
                        {
                            response.NoHistoryCount = reader["NoHistoryCount"] != DBNull.Value
                                ? Convert.ToInt32(reader["NoHistoryCount"])
                                : 0;
                        }

                        // 3️⃣ Third result: Total Count
                        if (reader.NextResult() && reader.Read())
                        {
                            response.TotalCount = reader["TotalCount"] != DBNull.Value
                                ? Convert.ToInt32(reader["TotalCount"])
                                : 0;
                        }

                        // 4️⃣ Fourth result: Paginated Data
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                response.CaseHistories.Add(new CaseHistoryVM
                                {
                                    Code = reader["Code"]?.ToString(),
                                    Custname = reader["Custname"]?.ToString(),
                                    Religion = reader["Religion"]?.ToString(),
                                    PhoneNo = reader["PhoneNo"]?.ToString(),
                                    Pincode = reader["Pincode"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    Creator = reader["Creator"]?.ToString(),

                                    Income = reader.IsDBNull(reader.GetOrdinal("Income")) ? null : Convert.ToDecimal(reader["Income"]),
                                    CrifScore = reader.IsDBNull(reader.GetOrdinal("CrifScore")) ? null : Convert.ToInt32(reader["CrifScore"]),
                                    OverdueAmt = reader.IsDBNull(reader.GetOrdinal("OverdueAmt")) ? null : Convert.ToDecimal(reader["OverdueAmt"]),
                                    TotalCurrentAmt = reader.IsDBNull(reader.GetOrdinal("TotalCurrentAmt")) ? null : Convert.ToDecimal(reader["TotalCurrentAmt"]),
                                    IncomePA = reader.IsDBNull(reader.GetOrdinal("IncomePA")) ? null : Convert.ToDecimal(reader["IncomePA"]),
                                    ActiveAccount = reader.IsDBNull(reader.GetOrdinal("CountofActiveAccount")) ? null : Convert.ToInt32(reader["CountofActiveAccount"]),
                                    ActiveAmount = reader.IsDBNull(reader.GetOrdinal("AmountofActiveAccount")) ? null : Convert.ToDecimal(reader["AmountofActiveAccount"]),
                                    FiCode = reader.IsDBNull(reader.GetOrdinal("FICode")) ? null : Convert.ToInt64(reader["FICode"])
                                });
                            }
                        }
                    }
                }

                con.Close();
            }

            return response;
        }


        public List<CsoCollectionReportModelVM> GetCsoCollectionReport(DateTime fromDate, DateTime toDate, string csoCode, string dbtype, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            List<CsoCollectionReportModelVM> reportList = new List<CsoCollectionReportModelVM>();

            SqlConnection con = null;

            try
            {
                // Connection open according to dbtype
                if (dbtype == "SBIPDLCOL")
                    con = _credManager.getConnections(dbName, isLive);
                else if (dbtype == "PDLERP")
                    con = _credManager.getConnections(dbName, isLive);
                else
                    throw new Exception("Invalid dbtype provided");

                using (SqlCommand cmd = new SqlCommand("Usp_GetCsoCollectionReport", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetCsoCollectionReport");
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    cmd.Parameters.AddWithValue("@CSOCode", csoCode);
                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);

                    con.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            CsoCollectionReportModelVM item = new CsoCollectionReportModelVM
                            {
                                Creator = rdr["Creator"] != DBNull.Value ? rdr["Creator"].ToString() : "",
                                VNO = rdr["VNO"] != DBNull.Value ? rdr["VNO"].ToString() : "",
                                VDATE = rdr["VDATE"] != DBNull.Value ? Convert.ToDateTime(rdr["VDATE"]) : (DateTime?)null,
                                DrCode = rdr["DrCode"] != DBNull.Value ? rdr["DrCode"].ToString() : "",
                                CRCode = rdr["CRCode"] != DBNull.Value ? rdr["CRCode"].ToString() : "",
                                Party_CD = rdr["Party_CD"] != DBNull.Value ? rdr["Party_CD"].ToString() : "",
                                DrAmount = rdr["DrAmount"] != DBNull.Value ? Convert.ToDecimal(rdr["DrAmount"]) : 0,
                                CrAmount = rdr["CrAmount"] != DBNull.Value ? Convert.ToDecimal(rdr["CrAmount"]) : 0,
                                VDesc = rdr["VDesc"] != DBNull.Value ? rdr["VDesc"].ToString() : "",
                                GroupCode = rdr["GroupCode"] != DBNull.Value ? rdr["GroupCode"].ToString() : "",
                                BranchCode = rdr["BranchCode"] != DBNull.Value ? rdr["BranchCode"].ToString() : "",
                                CollType = "" // Always blank as per requirement
                            };
                            reportList.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception as needed
                throw ex;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
            }

            return reportList;
        }
        public List<CsoCollectionReportModelVM> GetCsoCollectionReportAllCases(DateTime fromDate, DateTime toDate, string dbtype, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            List<CsoCollectionReportModelVM> reportList = new List<CsoCollectionReportModelVM>();

            SqlConnection con = null;

            try
            {
                // DB connection type select karna
                if (dbtype == "SBIPDLCOL")
                    con = _credManager.getConnections(dbName, isLive);
                else if (dbtype == "PDLERP")
                    con = _credManager.getConnections(dbName, isLive);
                else
                    throw new Exception("Invalid dbtype provided");

                using (SqlCommand cmd = new SqlCommand("Usp_GetCsoCollectionReport", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetCsoCollectionReportAllCases");
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);
                    con.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            CsoCollectionReportModelVM item = new CsoCollectionReportModelVM
                            {
                                Creator = rdr["Creator"] != DBNull.Value ? rdr["Creator"].ToString() : "",
                                VNO = rdr["VNO"] != DBNull.Value ? rdr["VNO"].ToString() : "",
                                VDATE = rdr["VDATE"] != DBNull.Value ? Convert.ToDateTime(rdr["VDATE"]) : DateTime.MinValue, // or (DateTime?)null
                                DrCode = rdr["DrCode"] != DBNull.Value ? rdr["DrCode"].ToString() : "",
                                CRCode = rdr["CRCode"] != DBNull.Value ? rdr["CRCode"].ToString() : "",
                                Party_CD = rdr["Party_CD"] != DBNull.Value ? rdr["Party_CD"].ToString() : "",
                                DrAmount = rdr["DrAmount"] != DBNull.Value ? Convert.ToDecimal(rdr["DrAmount"]) : 0,
                                CrAmount = rdr["CrAmount"] != DBNull.Value ? Convert.ToDecimal(rdr["CrAmount"]) : 0,
                                VDesc = rdr["VDesc"] != DBNull.Value ? rdr["VDesc"].ToString() : "",
                                GroupCode = rdr["GroupCode"] != DBNull.Value ? rdr["GroupCode"].ToString() : "",
                                BranchCode = rdr["BranchCode"] != DBNull.Value ? rdr["BranchCode"].ToString() : "",
                                CollType = "" // Always blank

                            };
                            reportList.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
            }

            return reportList;
        }
        public List<BBPSPaymentReportVM> GetBBPSPaymentReport(DateTime fromDate, DateTime toDate, string? smCode, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            List<BBPSPaymentReportVM> reportData = new List<BBPSPaymentReportVM>();

            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_GetCsoCollectionReport", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetBBPSPaymentReport");
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    cmd.Parameters.AddWithValue("@SmCode", string.IsNullOrEmpty(smCode) ? DBNull.Value : (object)smCode);
                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            reportData.Add(new BBPSPaymentReportVM
                            {
                                SmCode = dr["SmCode"] != DBNull.Value ? dr["SmCode"].ToString() : "",
                                TxnReferenceId = dr["TxnReferenceId"] != DBNull.Value ? dr["TxnReferenceId"].ToString() : "",
                                BillNumber = dr["BillNumber"] != DBNull.Value ? dr["BillNumber"].ToString() : "",
                                Ahead = dr["Ahead"] != DBNull.Value ? dr["Ahead"].ToString() : "",
                                Vno = dr["Vno"] != DBNull.Value ? dr["Vno"].ToString() : "",
                                Vdate = dr["Vdate"] != DBNull.Value ? Convert.ToDateTime(dr["Vdate"]) : (DateTime?)null,
                                Creator = dr["Creator"] != DBNull.Value ? dr["Creator"].ToString() : "",
                                CreditAmt = dr["CreditAmt"] != DBNull.Value ? Convert.ToDecimal(dr["CreditAmt"]) : 0,
                                DebitAmt = dr["DebitAmt"] != DBNull.Value ? Convert.ToDecimal(dr["DebitAmt"]) : 0

                            });
                        }
                    }
                }
            }
            return reportData;
        }

        public List<SmWithoutChqVM> GetLoansWithoutInstallments(string dDbName, string dbName, bool isLive, int PageNumber, int PageSize, out int totalCount)
        {
            totalCount = 0;
            List<SmWithoutChqVM> result = new List<SmWithoutChqVM>();

            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("usp_GetLoansPendingInstallments", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);
                    cmd.Parameters.AddWithValue("@DbName", dDbName); // Pass 'PDLERP' or 'PDLSHARECOL'

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalCount = reader["TotalCount"] != DBNull.Value
                                ? Convert.ToInt32(reader["TotalCount"])
                                : 0;
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                result.Add(new SmWithoutChqVM
                                {
                                    Code = reader["code"]?.ToString(),
                                    Subs_Name = reader["subs_name"]?.ToString(),
                                    Invest = reader["invest"] != DBNull.Value ? Convert.ToDecimal(reader["invest"]) : (decimal?)null,
                                    Dt_Fin = reader["dt_fin"] as DateTime?,
                                    Loan_Type = reader["loan_type"]?.ToString(),
                                    CreatedOn = reader["CreatedOn"] as DateTime?
                                });
                            }
                        }
                    }
                }

                con.Close();
            }

            return result;
        }
        public List<EMIInformationVM> GetEMIInformation(string smCode, string dbtype, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            SqlConnection con = null;
            List<EMIInformationVM> result = new List<EMIInformationVM>();
            try
            {
                // DB connection type select karna
                if (dbtype == "SBIPDLCOL")
                    con = _credManager.getConnections(dbName, isLive);
                else if (dbtype == "PDLERP")
                    con = _credManager.getConnections(dbName, isLive);
                else
                    throw new Exception("Invalid dbtype provided");
                using (SqlCommand cmd = new SqlCommand("GetEMIInformation", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CODE", string.IsNullOrEmpty(smCode) ? DBNull.Value : (object)smCode);
                    cmd.Parameters.AddWithValue("@dbname", string.IsNullOrEmpty(dbtype) ? DBNull.Value : (object)dbtype);

                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EMIInformationVM eMIs = new EMIInformationVM();

                            eMIs.Name = reader["full_name"] == DBNull.Value ? string.Empty : reader["full_name"].ToString();
                            eMIs.Code = reader["Code"] == DBNull.Value ? string.Empty : reader["Code"].ToString();

                            eMIs.TotalNoOfEMIDue = reader["Total No. of EMI Due"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Total No. of EMI Due"]);
                            eMIs.TotalAmountDue = reader["Total Amount Due"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Total Amount Due"]);
                            eMIs.TotalNoofEMIReceive = reader["Total No. of EMI Received"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Total No. of EMI Received"]);
                            eMIs.TotalAmountReceived = reader["Total Amount Received"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Total Amount Received"]);
                            eMIs.OverdueAmount = reader["Overdue Amount"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Overdue Amount"]);
                            eMIs.CurrentBalance = reader["Current Amount"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Current Amount"]);
                            eMIs.LastPaymentRcvdDate = reader["Last Payment Rcvd Date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["Last Payment Rcvd Date"]);
                            eMIs.LastRcvdAmt = reader["Last Rcvd Amt"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Last Rcvd Amt"]);
                            eMIs.SMClosedOrNot = reader["sm_closed_or_not"] == DBNull.Value ? string.Empty : reader["sm_closed_or_not"].ToString();
                            eMIs.DOB = reader["dob"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["dob"]);

                            eMIs.PanCard = reader["PAN"] != DBNull.Value ? Helper.Helper.Decrypt(reader["PAN"].ToString(), _configuration["encryptSalts:pan"]) : null;
                            eMIs.VoterCard = reader["VID"] != DBNull.Value ? Helper.Helper.Decrypt(reader["VID"].ToString(), _configuration["encryptSalts:voterid"]) : null;

                            //eMIs.TotalTenureofCase = reader["TotalTenureofCase"] == DBNull.Value ? string.Empty : reader["TotalTenureofCase"].ToString();
                            eMIs.Address = reader["address"] == DBNull.Value ? string.Empty : reader["address"].ToString();

                            result.Add(eMIs);
                        }
                        return result;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        public List<LoanWithoutDisbVoucherVM> GetLoansWithoutDisbursements(string dDbName, string dbName, bool isLive, int PageNumber, int PageSize, out int totalCount)
        {
            totalCount = 0;
            List<LoanWithoutDisbVoucherVM> result = new List<LoanWithoutDisbVoucherVM>();

            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("usp_GetLoansWithoutDisbVoucher", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);
                    cmd.Parameters.AddWithValue("@DbName", dDbName);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalCount = reader["TotalCount"] != DBNull.Value
                                ? Convert.ToInt32(reader["TotalCount"])
                                : 0;
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                result.Add(new LoanWithoutDisbVoucherVM
                                {
                                    Code = reader["code"]?.ToString(),
                                    SubsName = reader["subs_name"]?.ToString(),
                                    Invest = reader["invest"] != DBNull.Value ? Convert.ToDecimal(reader["invest"]) : (decimal?)null,
                                    DtFin = reader["dt_fin"] as DateTime?,
                                    LoanType = reader["loan_type"]?.ToString(),
                                    CreationDate = reader["CreatedOn"] as DateTime?
                                });
                            }
                        }
                    }
                }

                con.Close();
            }

            return result;
        }
        public List<DuplicateDIBVoucherVM> GetDuplicateDIBVouchers(string dbtype, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            List<DuplicateDIBVoucherVM> reportList = new List<DuplicateDIBVoucherVM>();
            SqlConnection con = null;

            try
            {
                // DB Connection
                if (dbtype == "SBIPDLCOL")
                    con = _credManager.getConnections(dbName, isLive);
                else if (dbtype == "PDLERP")
                    con = _credManager.getConnections(dbName, isLive);
                else
                    throw new Exception("Invalid dbtype provided");

                using (SqlCommand cmd = new SqlCommand("Usp_GetDuplicateDIBVouchers", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);
                    con.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            reportList.Add(new DuplicateDIBVoucherVM
                            {
                                Code = rdr["Code"] != DBNull.Value ? rdr["Code"].ToString() : string.Empty,
                                RCNo = rdr["RCNo"] != DBNull.Value ? rdr["RCNo"].ToString() : string.Empty,
                                VDate = rdr["VDate"] != DBNull.Value ? Convert.ToDateTime(rdr["VDate"]) : (DateTime?)null,
                                DR = rdr["DR"] != DBNull.Value ? Convert.ToDecimal(rdr["DR"]) : 0,
                                AHEAD = rdr["AHEAD"] != DBNull.Value ? rdr["AHEAD"].ToString() : string.Empty,
                                Creator = rdr["Creator"] != DBNull.Value ? rdr["Creator"].ToString() : string.Empty,
                                Flag = "Duplicate DIB Entry"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
            }

            return reportList;
        }

        public List<RcTransactionVM> GetRcDisbursementTransactionReport(string dDbName, string dbName, bool isLive, int PageNumber, int PageSize, out int totalCount, DateTime fromDate, DateTime toDate, string creator)
        {
            totalCount = 0;
            List<RcTransactionVM> result = new List<RcTransactionVM>();

            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("usp_GetRcTransactions", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@creator", creator);
                    cmd.Parameters.AddWithValue("@dt1", fromDate);
                    cmd.Parameters.AddWithValue("@dt2", toDate);
                    cmd.Parameters.AddWithValue("@DbName", dDbName);
                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalCount = reader["TotalCount"] != DBNull.Value
                                ? Convert.ToInt32(reader["TotalCount"])
                                : 0;
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                result.Add(new RcTransactionVM
                                {
                                    VDate = reader["VDate"] as DateTime?,
                                    Code = reader["Code"]?.ToString(),
                                    Ahead = reader["ahead"]?.ToString(),
                                    Dr = reader["dr"] != DBNull.Value ? Convert.ToDecimal(reader["dr"]) : (decimal?)null,
                                    Cr = reader["cr"] != DBNull.Value ? Convert.ToDecimal(reader["cr"]) : (decimal?)null
                                });
                            }
                        }
                    }
                }

                con.Close();
            }

            return result;
        }
        #region Get CSO Report based on Creator and BranchCode
        public List<CSOReportVM> GetCSOReport(int creatorId, string branchCode, string dbName, bool isLive, int pageNumber, int pageSize)
        {
            List<CSOReportVM> reportList = new List<CSOReportVM>();

            try
            {
                using (SqlConnection conn = _credManager.getConnections(dbName, isLive))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetCSOReport", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@CreatorId", SqlDbType.Int).Value = creatorId;
                        cmd.Parameters.Add("@BranchCode", SqlDbType.VarChar, 5).Value = branchCode;
                        cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                        cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;


                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CSOReportVM report = new CSOReportVM
                                {
                                    Creator = reader["CreatorName"].ToString(),
                                    BranchName = reader["BranchName"].ToString(),
                                    BranchCode = reader["BranchCode"].ToString(),
                                    CSOName = reader["CSOName"].ToString(),
                                    UserName = reader["UserName"].ToString()
                                };

                                reportList.Add(report);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return reportList;
        }

        #endregion

        #region  Get EMI Details based on SMCode
        public List<LedgerReportVM> GetLedgerReport(string smCode, string dbName, bool isLive, int pageNumber, int pageSize)
        {
            List<LedgerReportVM> reportList = new List<LedgerReportVM>();

            try
            {
                using (SqlConnection conn = _credManager.getConnections(dbName, isLive))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetEMIDetailsBasedSmCode", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@SMCode", SqlDbType.VarChar, 16).Value = smCode;
                        cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                        cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;


                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LedgerReportVM report = new LedgerReportVM
                                {
                                    SMCode = reader["SmCode"].ToString(),
                                    VoucherNo = reader["VNO"].ToString(),
                                    VoucherDate = Convert.ToDateTime(reader["VDATE"]),
                                    Narration = reader["Narration"].ToString(),
                                    DebitAmount = Convert.ToDouble(reader["DR"]),
                                    CreditAmount = Convert.ToDouble(reader["CR"]),
                                    AHEAD = reader["AHEAD"].ToString(),
                                    OverdueDays = Convert.ToDateTime(reader["INS_DUE_DT"])
                                };

                                reportList.Add(report);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return reportList;
        }
        #endregion

        public async Task<data> GetAccountAggregatorReportAsync(long? fiCode, int? creatorId, string? smCode, string activeUser, bool islive, string dbName)
        {
            var result = new data();

            try
            {
                using (SqlConnection conn = _credManager.getConnections(dbName, islive))
                using (SqlCommand cmd = new SqlCommand("usp_GetAccountAggregatorReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 60;

                    cmd.Parameters.AddWithValue("@FICode", (object?)fiCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatorID", (object?)creatorId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SMCode", (object?)smCode ?? DBNull.Value);

                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Read JSON strings from result set
                            string jsonDataRaw = reader["JsonData"]?.ToString();
                            string analyticsDataRaw = reader["Data"]?.ToString();

                            // Deserialize into structured models
                            if (!string.IsNullOrWhiteSpace(jsonDataRaw))
                                result.JsonData = JsonConvert.DeserializeObject<List<JsonData>>(jsonDataRaw);

                            if (!string.IsNullOrWhiteSpace(analyticsDataRaw))
                                result.Data = JsonConvert.DeserializeObject<CrifFormattedData>(analyticsDataRaw);
                        }
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to fetch account aggregator report: " + ex.Message, ex);
            }
        }

        public async Task<List<string>> SMCodeValidation(SMCodeValidationVM file, string dbname, bool isLive)
        {
            var missingCodes = new List<string>();
            var smCodesFromExcel = new List<string>();

            // Step 1: Read Excel
            using (var stream = new MemoryStream())
            {
                await file.SmCodeFile.CopyToAsync(stream);
                stream.Position = 0;
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        var smCode = row.Cell("A").GetString()?.Trim();
                        if (!string.IsNullOrEmpty(smCode))
                            smCodesFromExcel.Add(smCode);
                    }
                }
            }

            // Separate codes based on length
            var pdlerpCodes = smCodesFromExcel.Where(c => c.Length == 16).Distinct().ToList();
            var pdlsharecolCodes = smCodesFromExcel.Where(c => c.Length == 10).Distinct().ToList();

            // Step 2: Check PDLERP Codes
            if (pdlerpCodes.Any())
            {
                using (var con = _credManager.getConnections("PDLERP", isLive))
                {
                    await con.OpenAsync();
                    var smCodeTable = new DataTable();
                    smCodeTable.Columns.Add("SmCode", typeof(string));
                    foreach (var code in pdlerpCodes) smCodeTable.Rows.Add(code);

                    using (var cmd = new SqlCommand("Usp_GetMissingSmCodes", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var param = cmd.Parameters.AddWithValue("@SmCodes", smCodeTable);
                        param.SqlDbType = SqlDbType.Structured;
                        param.TypeName = "dbo.SmCodeList";

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                missingCodes.Add(reader["SmCode"].ToString());
                            }
                        }
                    }
                }
            }

            // Step 3: Check PDLSHARECOL Codes
            if (pdlsharecolCodes.Any())
            {
                using (var con = _credManager.getConnectionPDL("PDLSHARECOL", isLive))
                {
                    await con.OpenAsync();
                    var smCodeTable = new DataTable();
                    smCodeTable.Columns.Add("SmCode", typeof(string));
                    foreach (var code in pdlsharecolCodes) smCodeTable.Rows.Add(code);

                    using (var cmd = new SqlCommand("Usp_GetMissingSmCodes", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var param = cmd.Parameters.AddWithValue("@SmCodes", smCodeTable);
                        param.SqlDbType = SqlDbType.Structured;
                        param.TypeName = "dbo.SmCodeList";

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                missingCodes.Add(reader["SmCode"].ToString());
                            }
                        }
                    }
                }
            }

            // Step 4: Return Result as JSON
            return missingCodes;
        }
        public async Task<PaginationResponse<OverduePenalties>> GetOverdueRecordsAsync(PaginationRequest<OverduePenalties> request, string dbname, bool isLive)
        {
            var result = new PaginationResponse<OverduePenalties>();
            var data = new List<OverduePenalties>();
            var filters = JObject.FromObject(request.Filters);
            //var filters = request.Filters as dynamic;
            string creatorId = (string)(filters["CreatorID"] ?? filters["CreatorId"]);
            string branchCode = (string)(filters["BranchCode"] ?? filters["branchCode"]);
            string groupCode = (string)(filters["GroupCode"] ?? filters["groupCode"]);
            DateTime startDate = (DateTime)(filters["StartDate"] ?? filters["startDate"]);
            DateTime endDate = (DateTime)(filters["EndDate"] ?? filters["endDate"]);
            using (SqlConnection conn = _credManager.getConnections(dbname, isLive))
            using (SqlCommand cmd = new SqlCommand("Usp_GetOverduePenalties", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CreatorID", creatorId);
                cmd.Parameters.AddWithValue("@BranchCode", branchCode);
                cmd.Parameters.AddWithValue("@GroupCode", groupCode);
                cmd.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));

                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        data.Add(new OverduePenalties
                        {
                            FI_Id = reader["FI_Id"] != DBNull.Value ? Convert.ToInt32(reader["FI_Id"]) : 0,
                            FICode = Convert.ToInt64(reader["FICode"]?.ToString()),
                            EMIDate = Convert.ToDateTime(reader["EMIDate"] != DBNull.Value ? Convert.ToDateTime(reader["EMIDate"]) : (DateTime?)null),
                            OverDueDays = reader["OverDueDays"] != DBNull.Value ? Convert.ToInt32(reader["OverDueDays"]) : 0,
                            TotalOverDueAmount = reader["TotalOverDueAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalOverDueAmount"]) : 0,
                            CreatorName = reader["CreatorName"]?.ToString(),
                            FullName = reader["FullName"]?.ToString(),
                            BranchName = reader["BranchName"]?.ToString(),
                            GroupName = reader["GroupName"]?.ToString(),
                            CreationDate = Convert.ToDateTime(reader["CreationDate"]?.ToString())
                        });
                    }
                }
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string term = request.SearchTerm.ToLower();
                data = data.Where(x =>
                    x.FICode.ToString().Contains(term) ||
                    x.FullName.ToLower().Contains(term) ||
                    x.BranchName.ToLower().Contains(term) ||
                    x.GroupName.ToLower().Contains(term)
                ).ToList();
            }

            if (request.MinOverdueDays.HasValue)
            {
                data = data.Where(x => x.OverDueDays >= request.MinOverdueDays.Value).ToList();
            }
            if (request.SortOrder == "ASC")
                data = data.OrderBy(x => x.GetType().GetProperty(request.SortBy)?.GetValue(x, null)).ToList();
            else
                data = data.OrderByDescending(x => x.GetType().GetProperty(request.SortBy)?.GetValue(x, null)).ToList();
            var totalRecords = data.Count;
            var pagedData = data.Skip((request.PageNumber - 1) * request.PageSize)
                                .Take(request.PageSize)
                                .ToList();

            result.Data = pagedData;
            result.TotalRecords = totalRecords;
            result.PageSize = request.PageSize;
            result.CurrentPage = request.PageNumber;
            result.TotalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);
            result.HasPreviousPage = request.PageNumber > 1;
            result.HasNextPage = request.PageNumber < result.TotalPages;

            return result;
        }
        public async Task<List<OverduePenalties>> ExportOverdueExcel(string creatorId, string branchCode, string groupCode, string startDate, string endDate, string dbname, bool isLive)
        {
            List<OverduePenalties> data = new List<OverduePenalties>();
            using (SqlConnection conn = _credManager.getConnections(dbname, isLive))
            using (SqlCommand cmd = new SqlCommand("Usp_GetOverduePenalties", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CreatorID", creatorId);
                cmd.Parameters.AddWithValue("@BranchCode", branchCode);
                cmd.Parameters.AddWithValue("@GroupCode", groupCode);
                cmd.Parameters.AddWithValue("@StartDate", (Convert.ToDateTime(startDate)).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@EndDate", (Convert.ToDateTime(endDate)).ToString("yyyy-MM-dd"));

                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        data.Add(new OverduePenalties
                        {
                            FI_Id = reader["FI_Id"] != DBNull.Value ? Convert.ToInt32(reader["FI_Id"]) : 0,
                            FICode = Convert.ToInt64(reader["FICode"]?.ToString()),
                            EMIDate = Convert.ToDateTime(reader["EMIDate"] != DBNull.Value ? Convert.ToDateTime(reader["EMIDate"]) : (DateTime?)null),
                            OverDueDays = reader["OverDueDays"] != DBNull.Value ? Convert.ToInt32(reader["OverDueDays"]) : 0,
                            TotalOverDueAmount = reader["TotalOverDueAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalOverDueAmount"]) : 0,
                            CreatorName = reader["CreatorName"]?.ToString(),
                            FullName = reader["FullName"]?.ToString(),
                            BranchName = reader["BranchName"]?.ToString(),
                            GroupName = reader["GroupName"]?.ToString(),
                            CreationDate = Convert.ToDateTime(reader["CreationDate"]?.ToString()),
                            Rate = Convert.ToDecimal(reader["Rate"]?.ToString()),
                        });
                    }
                }
            }



            return data;
        }

        public List<CibilDataVM> GetCibilReport(string searchDate, string dbName, bool isLive)
        {
            List<CibilDataVM> result = new List<CibilDataVM>();

            string spName = "usp_getCibilData";

            try
            {
                using (SqlConnection con = _credManager.getConnections(dbName, isLive))
                using (SqlCommand cmd = new SqlCommand(spName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@reportDate", SqlDbType.SmallDateTime).Value = searchDate;

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CibilDataVM vm = new CibilDataVM
                            {
                                ConsumerName = reader["Consumer Name"] == DBNull.Value ? null : reader["Consumer Name"].ToString().Trim(),
                                DOB = reader["Date Of Birth"] == DBNull.Value ? null : reader["Date Of Birth"].ToString().Trim(),
                                Gender = reader["Gender"] == DBNull.Value ? null : reader["Gender"].ToString().Trim(),
                                IncomeTaxId = reader["Income Tax ID Number"] == DBNull.Value ? null : Helper.Helper.Decrypt(reader["Income Tax ID Number"].ToString(), _configuration["encryptSalts:pan"]),

                                PassportNumber = reader["Passport Number"] == DBNull.Value ? null : reader["Passport Number"].ToString().Trim(),
                                PassportIssueDate = reader["Passport Issue Date"] == DBNull.Value ? null : reader["Passport Issue Date"].ToString().Trim(),
                                PassportExpiryDate = reader["Passport Expiry Date"] == DBNull.Value ? null : reader["Passport Expiry Date"].ToString().Trim(),

                                VoterIdNumber = reader["Voter ID Number"] == DBNull.Value ? null : Helper.Helper.Decrypt(reader["Voter ID Number"].ToString(), _configuration["encryptSalts:voterid"]),

                                DrivingLicenseNumber = reader["Driving License Number"] == DBNull.Value ? null : Helper.Helper.Decrypt(reader["Driving License Number"].ToString(), _configuration["encryptSalts:DL"]),
                                DrivingLicenseIssueDate = reader["Driving License Issue Date"] == DBNull.Value ? null : reader["Driving License Issue Date"].ToString().Trim(),
                                DrivingLicenseExpiryDate = reader["Driving License Expiry Date"] == DBNull.Value ? null : reader["Driving License Expiry Date"].ToString().Trim(),

                                AdditionalId2 = reader["Additional ID #2"] == DBNull.Value ? null : reader["Additional ID #2"].ToString().Trim(),

                                TelephoneMobile = reader["Telephone No.Mobile"] == DBNull.Value ? null : reader["Telephone No.Mobile"].ToString().Trim(),
                                TelephoneResidence = reader["Telephone No.Residence"] == DBNull.Value ? null : reader["Telephone No.Residence"].ToString().Trim(),
                                TelephoneOffice = reader["Telephone No.Office"] == DBNull.Value ? null : reader["Telephone No.Office"].ToString().Trim(),
                                ExtensionOffice = reader["Extension Office"] == DBNull.Value ? null : reader["Extension Office"].ToString().Trim(),
                                TelephoneOther = reader["Telephone No.Other"] == DBNull.Value ? null : reader["Telephone No.Other"].ToString().Trim(),
                                ExtensionOther = reader["Extension Other"] == DBNull.Value ? null : reader["Extension Other"].ToString().Trim(),

                                EmailId1 = reader["Email ID 1"] == DBNull.Value ? null : reader["Email ID 1"].ToString().Trim(),
                                EmailId2 = reader["Email ID 2"] == DBNull.Value ? null : reader["Email ID 2"].ToString().Trim(),

                                // Address 1
                                Address1 = reader["Address 1"] == DBNull.Value ? null : reader["Address 1"].ToString().Trim(),
                                StateCode1 = reader["State Code 1"] == DBNull.Value ? null : reader["State Code 1"].ToString().Trim(),
                                PinCode1 = reader["PIN Code 1"] == DBNull.Value ? null : reader["PIN Code 1"].ToString().Trim(),
                                AddressCategory1 = reader["Address Category 1"] == DBNull.Value ? null : reader["Address Category 1"].ToString().Trim(),
                                ResidenceCode1 = reader["Residence Code 1"] == DBNull.Value ? null : reader["Residence Code 1"].ToString().Trim(),

                                // Address 2
                                Address2 = reader["Address 2"] == DBNull.Value ? null : reader["Address 2"].ToString().Trim(),
                                StateCode2 = reader["State Code 2"] == DBNull.Value ? null : reader["State Code 2"].ToString().Trim(),
                                PinCode2 = reader["PIN Code 2"] == DBNull.Value ? null : reader["PIN Code 2"].ToString().Trim(),
                                AddressCategory2 = reader["Address Category 2"] == DBNull.Value ? null : reader["Address Category 2"].ToString().Trim(),
                                ResidenceCode2 = reader["Residence Code 2"] == DBNull.Value ? null : reader["Residence Code 2"].ToString().Trim(),

                                // Current/New Account
                                CurrentNewMemberCode = reader["Current/New Member Code"] == DBNull.Value ? null : reader["Current/New Member Code"].ToString().Trim(),
                                CurrentNewMemberShortName = reader["Current/New Member Short Name"] == DBNull.Value ? null : reader["Current/New Member Short Name"].ToString().Trim(),
                                CurrentNewAccountNumber = reader["Current/New Account Number"] == DBNull.Value ? null : reader["Current/New Account Number"].ToString().Trim(),
                                AccountType = reader["Account Type"] == DBNull.Value ? null : reader["Account Type"].ToString().Trim(),
                                OwnershipIndicator = reader["Ownership Indicator"] == DBNull.Value ? null : reader["Ownership Indicator"].ToString().Trim(),

                                DateOpenedDisbursed = reader["Date Opened/ Disbursed"] == DBNull.Value ? null : reader["Date Opened/ Disbursed"].ToString().Trim(),
                                DateOfLastPayment = reader["Date Of Last Payment"] == DBNull.Value ? null : reader["Date Of Last Payment"].ToString().Trim(),
                                DateClosed = reader["Date Closed"] == DBNull.Value ? null : reader["Date Closed"].ToString().Trim(),
                                DateReported = reader["Date Reported"] == DBNull.Value ? null : reader["Date Reported"].ToString().Trim(),

                                HighCreditOrSanctionedAmount = reader["High Credit / Sanctioned Amount"] == DBNull.Value ? null : reader["High Credit / Sanctioned Amount"].ToString().Trim(),
                                CurrentBalance = reader["Current Balance"] == DBNull.Value ? null : reader["Current Balance"].ToString().Trim(),
                                AmountOverdue = reader["Amount Overdue"] == DBNull.Value ? null : reader["Amount Overdue"].ToString().Trim(),
                                NumberOfDaysPastDue = reader["Number Of Days Past Due"] == DBNull.Value ? null : reader["Number Of Days Past Due"].ToString().Trim(),

                                // Old Account
                                OldMemberCode = reader["Old Member Code"] == DBNull.Value ? null : reader["Old Member Code"].ToString().Trim(),
                                OldMemberShortName = reader["Old Member Short Name"] == DBNull.Value ? null : reader["Old Member Short Name"].ToString().Trim(),
                                OldAccountNumber = reader["Old Acc No"] == DBNull.Value ? null : reader["Old Acc No"].ToString().Trim(),
                                OldAccountType = reader["Old Acc Type"] == DBNull.Value ? null : reader["Old Acc Type"].ToString().Trim(),
                                OldOwnershipIndicator = reader["Old Ownership Indicator"] == DBNull.Value ? null : reader["Old Ownership Indicator"].ToString().Trim(),

                                SuitFiledWilfulDefault = reader["Suit Filed / Wilful Default"] == DBNull.Value ? null : reader["Suit Filed / Wilful Default"].ToString().Trim(),
                                WrittenOffSettledStatus = reader["Written-off and Settled Status"] == DBNull.Value ? null : reader["Written-off and Settled Status"].ToString().Trim(),
                                AssetClassification = reader["Asset Classification"] == DBNull.Value ? null : reader["Asset Classification"].ToString().Trim(),

                                ValueOfCollateral = reader["Value of Collateral"] == DBNull.Value ? null : reader["Value of Collateral"].ToString().Trim(),
                                TypeOfCollateral = reader["Type of Collateral"] == DBNull.Value ? null : reader["Type of Collateral"].ToString().Trim(),

                                CreditLimit = reader["Credit Limit"] == DBNull.Value ? null : reader["Credit Limit"].ToString().Trim(),
                                CashLimit = reader["Cash Limit"] == DBNull.Value ? null : reader["Cash Limit"].ToString().Trim(),
                                RateOfInterest = reader["Rate of Interest"] == DBNull.Value ? null : reader["Rate of Interest"].ToString().Trim(),
                                RepaymentTenure = reader["RepaymentTenure"] == DBNull.Value ? null : reader["RepaymentTenure"].ToString().Trim(),
                                EMIAmount = reader["EMI Amount"] == DBNull.Value ? null : reader["EMI Amount"].ToString().Trim(),

                                WrittenOffAmountTotal = reader["Written- off Amount (Total)"] == DBNull.Value ? null : reader["Written- off Amount (Total)"].ToString().Trim(),
                                WrittenOffPrincipalAmount = reader["Written- off Principal Amount"] == DBNull.Value ? null : reader["Written- off Principal Amount"].ToString().Trim(),
                                SettlementAmount = reader["Settlement Amt"] == DBNull.Value ? null : reader["Settlement Amt"].ToString().Trim(),

                                PaymentFrequency = reader["Payment Frequency"] == DBNull.Value ? null : reader["Payment Frequency"].ToString().Trim(),
                                ActualPaymentAmount = reader["Actual Payment Amt"] == DBNull.Value ? null : reader["Actual Payment Amt"].ToString().Trim(),

                                OccupationCode = reader["Occupation Code"] == DBNull.Value ? null : reader["Occupation Code"].ToString().Trim(),
                                Income = reader["Income"] == DBNull.Value ? null : reader["Income"].ToString().Trim(),
                                NetGrossIncomeIndicator = reader["Net/Gross Income Indicator"] == DBNull.Value ? null : reader["Net/Gross Income Indicator"].ToString().Trim(),
                                MonthlyAnnualIncomeIndicator = reader["Monthly/Annual Income Indicator"] == DBNull.Value ? null : reader["Monthly/Annual Income Indicator"].ToString().Trim(),

                                NoInsDue = reader["no_ins_due"] == DBNull.Value ? (short?)null : Convert.ToInt16(reader["no_ins_due"]),
                                DiffDtClosedDtReport = reader["DiffDtClosedDtReport"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["DiffDtClosedDtReport"])
                            };
                            result.Add(vm);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public List<InsuranceDataVM> GetInsuranceReport(string fromDate, string toDate, string dbName, bool isLive)
        {
            List<InsuranceDataVM> result = new List<InsuranceDataVM>();

            string spName = "Usp_GetInsuranceData";

            try
            {
                using (SqlConnection con = _credManager.getConnections(dbName, isLive))
                using (SqlCommand cmd = new SqlCommand(spName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InsuranceDataVM vm = new InsuranceDataVM
                            {
                                BranchName = reader["BranchName"]?.ToString(),
                                LoanAppliRecDt = reader["LoanAppliRecDt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LoanAppliRecDt"]),

                                LoanAcctNo = reader["LoanAcctNo"]?.ToString(),
                                LoanAmount = reader["LoanAmount"] == DBNull.Value ? null : (double?)Convert.ToDouble(reader["LoanAmount"]),
                                LoanTenure = reader["LoanTenure"] == DBNull.Value ? null : (double?)Convert.ToDouble(reader["LoanTenure"]),
                                LoanType = reader["LoanType"]?.ToString(),

                                Title = reader["Title"]?.ToString(),
                                ApplicantFirstName = reader["ApplicantFirstName"]?.ToString(),
                                ApplicantLastName = reader["ApplicantLastName"]?.ToString(),

                                DOB = reader["DOB"]?.ToString(),
                                Gender = reader["Gender"]?.ToString(),

                                Address = reader["Address"]?.ToString(),
                                Address2 = reader["Address2"]?.ToString(),
                                PinCode = reader["PinCode"]?.ToString(),
                                EmailID = reader["EmailID"]?.ToString(),
                                MobileNo = reader["MobileNo"]?.ToString(),

                                InsuredFirstName = reader["InsuredFirstName"]?.ToString(),
                                InsuredLastName = reader["InsuredLastName"]?.ToString(),
                                InsuredDOB = reader["InsuredDOB"]?.ToString(),
                                InsuredRelationShip = reader["InsuredRelationShip"]?.ToString(),

                                Insured2FirstName = reader["Insured2FirstName"]?.ToString(),
                                Insured2LastName = reader["Insured2LastName"]?.ToString(),
                                Insured2DOB = reader["Insured2DOB"]?.ToString(),
                                Insured2RelationShip = reader["Insured2RelationShip"]?.ToString(),

                                NomineeName = reader["NomineeName"]?.ToString(),
                                NomineeDOB = reader["NomineeDOB"]?.ToString(),
                                Relation = reader["Relation"]?.ToString(),

                                SumAssured = reader["SumAssured"] == DBNull.Value ? null : (double?)Convert.ToDouble(reader["SumAssured"]),
                                TransactionAmount = reader["TransactionAmount"] == DBNull.Value ? null : (double?)Convert.ToDouble(reader["TransactionAmount"]),
                                TransactionDate = reader["TransactionDate"]?.ToString(),
                                TransactionDetails_UTR = reader["TransactionDetails_UTR"]?.ToString()
                            };


                            result.Add(vm);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        #region QR Payment Logs
        public List<QrPaymentLogsVM> GetQrPaymentsLogs(string fromDate, string toDate, string? paymentMode, string activeUser, string dbName, bool isLive)
        {
            List<QrPaymentLogsVM> list = new List<QrPaymentLogsVM>();

            int activeUserId = Convert.ToInt32(activeUser);

            try
            {
                using (SqlConnection con = _credManager.getConnections(dbName, isLive))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetQRPaymentLogs", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.Add("@ActiveUser", SqlDbType.Int).Value = activeUserId;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(paymentMode) ? DBNull.Value : paymentMode;

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                QrPaymentLogsVM vm = new QrPaymentLogsVM
                                {
                                    TxnId = reader["txnId"]?.ToString(),
                                    VNO = reader["VNO"]?.ToString(),
                                    CustRef = reader["custRef"]?.ToString(),
                                    Amount = reader["amount"] != DBNull.Value ? Convert.ToDecimal(reader["amount"]) : 0,
                                    TxnStatus = reader["txnStatus"]?.ToString(),
                                    PayerVpa = reader["payerVpa"]?.ToString(),
                                    PayeeVpa = reader["payeeVpa"]?.ToString(),
                                    TxnDateTime = reader["txnDateTime"] != DBNull.Value ? Convert.ToDateTime(reader["txnDateTime"]) : (DateTime?)null,
                                    VirtualVpa = reader["virtualVpa"]?.ToString(),
                                    Fname = reader["Fname"]?.ToString(),
                                    Branchname = reader["branchname"]?.ToString(),
                                    GroupCode = reader["GroupCode"]?.ToString(),
                                    FiSmCode = reader["FISMCODE"]?.ToString(),
                                    QRSmCode = reader["QRSMCODE"]?.ToString(),
                                    Creationdate = reader["creationdate"] != DBNull.Value ? Convert.ToDateTime(reader["creationdate"]) : (DateTime?)null,
                                    Ahead = reader["AHEAD"]?.ToString(),
                                    SmCodeStatus = reader["SmCodeStatus"]?.ToString(),
                                    QrEntryFlag = reader["QrEntryFlag"]?.ToString(),
                                    PaymentMode = reader["PaymentMode"]?.ToString()
                                };

                                list.Add(vm);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }
        #endregion

        #region ICICI Payment Logs
        public bool CheckTransactionExists(string bankRRN, string merchantTranId, string dbName, bool isLive)
        {
            try
            {
                using (SqlConnection con = _credManager.getConnections(dbName, isLive))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_CheckExistsdata", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "CheckExistsData");
                        cmd.Parameters.AddWithValue("@BankRRN", bankRRN);
                        cmd.Parameters.AddWithValue("@MerchantTranId", merchantTranId);

                        con.Open();
                        var result = cmd.ExecuteScalar();

                        return result != null && Convert.ToInt32(result) == 1;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetPayerNameBySmCode(string smCode, string dbName, bool isLive)
        {
            try
            {
                using (SqlConnection con = _credManager.getConnections(dbName, isLive))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_CheckExistsdata", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "GetfullNamebySm");
                        cmd.Parameters.AddWithValue("@SmCode", smCode);

                        con.Open();
                        var result = cmd.ExecuteScalar();

                        return result == null || result == DBNull.Value ? "Not Found" : result.ToString().Trim();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int UploadIciciTransFile(IciciExcelFileVM row, string activeUser, string dbName, bool isLive)
        {
            try
            {
                //if (string.IsNullOrWhiteSpace(row.BankRRN) || string.IsNullOrWhiteSpace(row.MerchantTranId))
                //    return -1;
                //if (CheckTransactionExists(row.BankRRN, row.MerchantTranId, dbName, isLive))
                //    return 0;
             DateTime? txnDate = row.Date;
                //if (txnDate == null)
                //    return -2;

               // string txnDateStr = txnDate.Value.ToString("yyyyMMddHHmmss");
                string txnDateStr = txnDate?.ToString("yyyyMMddHHmmss");
                DateTime? creationDate = txnDate.Value;

               // string smCode = string.IsNullOrWhiteSpace(row.MerchantTranId) ? string.Empty : row.MerchantTranId.Length >= 16 ? row.MerchantTranId.Substring(row.MerchantTranId.Length - 16) : row.MerchantTranId;

               // string payerName = GetPayerNameBySmCode(smCode, dbName, isLive);

                string responseJson = JsonConvert.SerializeObject(new
                {
                    Id = "0",
                    Entry = "Manual entry based on the MIS report"
                });

                ICICICallBackResponse data = new ICICICallBackResponse
                {
                    BankRRN = row.BankRRN,
                    MerchantTranId = row.MerchantTranId,
                    PayerVA = row.PayerVA,
                    PayerAccountType = row.PayerAccountType,
                    PayerAmount = row.PayerAmount,
                    TerminalId = row.TerminalId,
                    SubMerchantId = row.SubMerchantId,

                    ResponseCode = "00",
                    RespCodeDescription = "APPROVED OR COMPLETED SUCCESSFULLY",
                    PayerMobile = "0",

                    TxnInitDate = txnDate,
                    TxnCompletionDate = txnDate,
                    TxnStatus = "SUCCESS",

                    PayeeVPA = "pdlIc00001@icici",
                    MerchantId = row.SubMerchantId,

                    CreationDate =DateTime.Now,
                    UPIVersion = "UPI 1.0",
                    ResponseJSON = responseJson,
                    SequenceNum = row.SeqNo,
                    PayerName=activeUser
                };
                int iciciId = InsertIciciCallback(data, activeUser, dbName, isLive);
                if (iciciId <= 0)
                    return -3;

                //QrPaymentResultVM qr = InsertQrPayment(data, dbName, isLive);
                //if (qr == null)
                //    return -4;

                //if (data.TxnStatus == "SUCCESS")
                //{
                //    RcManualVM rcVm = new RcManualVM();
                //    rcVm.TxnId = qr.TxnId;
                //    rcVm.TxnDate = qr.TxnDateTime;
                //    rcVm.Amt = qr.Amount.ToString();
                //    rcVm.SmCode = qr.VirtualVpa.Substring(qr.VirtualVpa.Length - 16);

                //    ICICIRcPostManualVM rcPostVm = PrepareRcPostData(rcVm, dbName, isLive);
                //    if (rcPostVm == null)
                //        return -5;

                //    if (rcPostVm != null)
                //    {
                //        APIResponseVM apiResp = Helper.Helper.SaveRcManualByExcel(rcPostVm, activeUser, allUrl, token);
                //        if (!apiResp.IsSuccessStatusCode)
                //        {
                //            return -7;
                //        }
                //    }
                //}
                return 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int InsertIciciCallback(ICICICallBackResponse data, string activeUser, string dbName, bool isLive)
        {
            try
            {
                using SqlConnection con = _credManager.getConnections(dbName, isLive);
                {
                    using SqlCommand cmd = new("Usp_InsertIciciCallback", con);
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@BankRRN", data.BankRRN);
                        cmd.Parameters.AddWithValue("@MerchantTranId", data.MerchantTranId);
                        cmd.Parameters.AddWithValue("@PayerVA", data.PayerVA);
                        cmd.Parameters.AddWithValue("@PayerAccountType", data.PayerAccountType);
                        cmd.Parameters.AddWithValue("@PayerAmount", data.PayerAmount);
                        cmd.Parameters.AddWithValue("@TerminalId", data.TerminalId);
                        cmd.Parameters.AddWithValue("@SubMerchantId", data.SubMerchantId);
                        cmd.Parameters.AddWithValue("@MerchantId", data.MerchantId);
                        cmd.Parameters.AddWithValue("@TxnInitDate", data.TxnInitDate);
                        cmd.Parameters.AddWithValue("@TxnCompletionDate", data.TxnCompletionDate);
                        cmd.Parameters.AddWithValue("@TxnStatus", data.TxnStatus);
                        cmd.Parameters.AddWithValue("@ResponseCode", data.ResponseCode);
                        cmd.Parameters.AddWithValue("@RespCodeDescription", data.RespCodeDescription);
                        cmd.Parameters.AddWithValue("@PayerName", data.PayerName);
                        cmd.Parameters.AddWithValue("@PayerMobile", data.PayerMobile);
                        cmd.Parameters.AddWithValue("@PayeeVPA", data.PayeeVPA);
                        cmd.Parameters.AddWithValue("@UPIVersion", data.UPIVersion);
                        cmd.Parameters.AddWithValue("@ResponseJSON", data.ResponseJSON);
                        cmd.Parameters.AddWithValue("@SequenceNum", data.SequenceNum);
                        cmd.Parameters.AddWithValue("@CreationDate", data.CreationDate);
                        cmd.Parameters.AddWithValue("@CreatedBy", activeUser);

                        con.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private QrPaymentResultVM InsertQrPayment(ICICICallBackResponse model, string dbName, bool isLive)
        {
            try
            {
                using SqlConnection con = _credManager.getConnections(dbName, isLive);
                {
                    using SqlCommand cmd = new("Usp_InsertQrPayment", con);
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                       // DateTime txnDate = DateTime.ParseExact(model.TxnInitDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                        //DateTime txnDate = model.TxnInitDate.Value;
                        DateTime? txnDate = model.TxnInitDate;

                        cmd.Parameters.AddWithValue("@TxnId", model.BankRRN);
                        cmd.Parameters.AddWithValue("@VirtualVpa", model.MerchantTranId);
                        cmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(model.PayerAmount));
                        cmd.Parameters.AddWithValue("@TxnStatus", model.TxnStatus);
                        cmd.Parameters.AddWithValue("@TxnDateTime", txnDate);
                        cmd.Parameters.AddWithValue("@MrchId", model.MerchantId);
                        cmd.Parameters.AddWithValue("@PayeeVpa", model.PayeeVPA);
                        cmd.Parameters.AddWithValue("@PayerVpa", model.PayerVA);
                        cmd.Parameters.AddWithValue("@PayerAccountType", model.PayerAccountType);
                        cmd.Parameters.AddWithValue("@PayerMobile", model.PayerMobile);
                        cmd.Parameters.AddWithValue("@PayerVerifiedName", model.PayerName);

                        con.Open();
                        int affected = Convert.ToInt32(cmd.ExecuteScalar());

                        if (affected <= 0)
                            return null;

                        return new QrPaymentResultVM
                        {
                            TxnId = model.BankRRN,
                            TxnDateTime = txnDate,
                            Amount = Convert.ToDecimal(model.PayerAmount),
                            VirtualVpa = model.MerchantTranId
                        };
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ICICIRcPostManualVM PrepareRcPostData(RcManualVM rcManualVM, string dbName, bool isLive)
        {
            ICICIRcPostManualVM objVM = null;

            try
            {
                using SqlConnection con = _credManager.getConnections(dbName, isLive);
                {
                    using SqlCommand cmd = new("Usp_RcPostManual", con);
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SmCode", SqlDbType.VarChar, 16).Value = rcManualVM.SmCode.Trim();
                        cmd.Parameters.Add("@CollDt", SqlDbType.VarChar, 20).Value = rcManualVM.TxnDate.HasValue ? rcManualVM.TxnDate.Value.ToString("dd-MMM-yyyy") : DBNull.Value;
                        cmd.Parameters.Add("@TxnId", SqlDbType.VarChar, 30).Value = rcManualVM.TxnId.Trim();
                        cmd.Parameters.Add("@Amt", SqlDbType.Decimal).Value = Convert.ToDecimal(rcManualVM.Amt);

                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    objVM = new ICICIRcPostManualVM();
                                    objVM.InstRcvID = Convert.ToInt64(reader["InstRcvID"]);
                                    objVM.IMEI = Convert.ToInt64(reader["IMEI"]);
                                    objVM.CaseCode = reader["CaseCode"].ToString();
                                    objVM.RcptNo = Convert.ToInt32(reader["RcptNo"]);
                                    objVM.InstRcvAmt = Convert.ToInt32(Convert.ToDouble(rcManualVM.Amt));
                                    objVM.InstRcvDateTimeUTC = rcManualVM.TxnDate.ToString();
                                    objVM.Flag = reader["Flag"].ToString();
                                    objVM.BatchNo = 0;
                                    objVM.FoCode = reader["FoCode"].ToString();
                                    objVM.DataBaseName = reader["DataBaseName"].ToString();
                                    objVM.Creator = reader["CreatorName"].ToString();
                                    objVM.CustName = reader["CustName"].ToString();
                                    objVM.PartyCd = reader["PartyCd"].ToString();
                                    objVM.PayFlag = reader["PayFlag"].ToString();
                                    objVM.SmsMobNo = reader["SmsMobNo"].ToString();
                                    objVM.InterestAmt = Convert.ToInt32(reader["InterestAmt"]);
                                    objVM.CollPoint = reader["CollPoint"].ToString();
                                    objVM.PaymentMode = "QR";
                                    objVM.CollBranchCode = reader["collBranchCode"].ToString();
                                    objVM.TxnId = rcManualVM.TxnId;
                                    objVM.CSOID = reader["CSOID"].ToString();
                                    objVM.VDATE = DateTime.Now;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return objVM;
        }
        #endregion
    }
}