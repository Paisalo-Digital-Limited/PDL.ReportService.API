using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Entites.VM.ReportVM;
using PDL.ReportService.Logics.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public List<CaseHistoryVM> GetCaseHistoryBySmCodes(List<string> smCodes, string dbName, bool isLive, int PageNumber, int PageSize, out int totalCount)
        {
            totalCount = 0;
            List<CaseHistoryVM> result = new List<CaseHistoryVM>();

            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                con.Open();

                foreach (var smCode in smCodes)
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GetCaseHistoryBySmCode", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SmCode", smCode);
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
                                    result.Add(new CaseHistoryVM
                                    {
                                        Code = reader["Code"]?.ToString(),
                                        Custname = reader["Custname"]?.ToString(),
                                        Religion = reader["Religion"]?.ToString(),
                                        PhoneNo = reader["PhoneNo"]?.ToString(),
                                        Pincode = reader["Pincode"]?.ToString(),
                                        Address = reader["Address"]?.ToString(),
                                        Creator = reader["Creator"]?.ToString(),

                                        Income = reader.IsDBNull(reader.GetOrdinal("Income"))
                                            ? (decimal?)null
                                            : Convert.ToDecimal(reader["Income"]),

                                        CrifScore = reader.IsDBNull(reader.GetOrdinal("CrifScore"))
                                            ? (int?)null
                                            : Convert.ToInt32(reader["CrifScore"]),

                                        OverdueAmt = reader.IsDBNull(reader.GetOrdinal("OverdueAmt"))
                                            ? (decimal?)null
                                            : Convert.ToDecimal(reader["OverdueAmt"]),

                                        TotalCurrentAmt = reader.IsDBNull(reader.GetOrdinal("TotalCurrentAmt"))
                                            ? (decimal?)null
                                            : Convert.ToDecimal(reader["TotalCurrentAmt"]),

                                        IncomePA = reader.IsDBNull(reader.GetOrdinal("IncomePA"))
                                            ? (decimal?)null
                                            : Convert.ToDecimal(reader["IncomePA"]),

                                        ActiveAccount = reader.IsDBNull(reader.GetOrdinal("CountofActiveAccount"))
                                            ? (int?)null
                                            : Convert.ToInt32(reader["CountofActiveAccount"]),

                                        ActiveAmount = reader.IsDBNull(reader.GetOrdinal("AmountofActiveAccount"))
                                            ? (decimal?)null
                                            : Convert.ToDecimal(reader["AmountofActiveAccount"])
                                    });
                                }
                            }
                        }
                    }
                }

                con.Close();
            }

            return result;
        }

        public List<CsoCollectionReportModelVM> GetCsoCollectionReport(DateTime fromDate, DateTime toDate, string csoCode, string dbtype, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            List<CsoCollectionReportModelVM> reportList = new List<CsoCollectionReportModelVM>();

            SqlConnection con = null;

            try
            {
                // Connection open according to dbtype
                if (dbtype == "SBIPDLCOL")
                    con = _credManager.getConnectionString(dbName, isLive);
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
                    con = _credManager.getConnectionString(dbName, isLive);
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
                                    Invest = reader["invest"] as decimal?,
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
                    con = _credManager.getConnectionString(dbName, isLive);
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
                                    Invest = reader["invest"] as decimal?,
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
                    con = _credManager.getConnectionString(dbName, isLive);
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
               throw ;
            }

            return reportList;
        }

        #endregion
    }
}