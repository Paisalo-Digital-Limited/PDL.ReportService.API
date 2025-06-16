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

        public List<CaseHistoryVM> GetCaseHistoryBySmCodes(List<string> smCodes, string dbName, bool isLive, int PageNumber, int PageSize)
        {
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
                            while (reader.Read())
                            {
                                result.Add(new CaseHistoryVM
                                {
                                    Code = reader["Code"]?.ToString(),
                                    Custname = reader["Custname"]?.ToString(),
                                    Religion = reader["Religion"]?.ToString(),
                                    PhoneNo = reader["PhoneNo"]?.ToString(),
                                    Income = reader["Income"] as decimal?,
                                    CrifScore = reader["CrifScore"] as int?,
                                    OverdueAmt = reader["OverdueAmt"] as decimal?,
                                    TotalCurrentAmt = reader["TotalCurrentAmt"] as decimal?,
                                    Pincode = reader["Pincode"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    Creator = reader["Creator"]?.ToString(),
                                    IncomePA = reader["IncomePA"] as decimal?
                                });
                            }
                        }
                    }
                }

                con.Close();
            }

            return result;
        }
        public List<CsoCollectionReportModelVM> GetCsoCollectionReport(DateTime fromDate, DateTime toDate, string csoCode, string dbtype, string dbName, bool isLive)
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

                    con.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            CsoCollectionReportModelVM item = new CsoCollectionReportModelVM
                            {
                                Creator = rdr["Creator"].ToString(),
                                VNO = rdr["VNO"].ToString(),
                                VDATE = Convert.ToDateTime(rdr["VDATE"]),
                                DrCode = rdr["DrCode"].ToString(),
                                CRCode = rdr["CRCode"].ToString(),
                                Party_CD = rdr["Party_CD"].ToString(),
                                DrAmount = Convert.ToDecimal(rdr["DrAmount"]),
                                CrAmount = Convert.ToDecimal(rdr["CrAmount"]),
                                VDesc = rdr["VDesc"].ToString(),
                                GroupCode = rdr["GroupCode"].ToString(),
                                BranchCode = rdr["BranchCode"].ToString(),
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
        public List<CsoCollectionReportModelVM> GetCsoCollectionReportAllCases(DateTime fromDate, DateTime toDate, string dbtype, string dbName, bool isLive)
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

                    con.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            CsoCollectionReportModelVM item = new CsoCollectionReportModelVM
                            {
                                Creator = rdr["Creator"].ToString(),
                                VNO = rdr["VNO"].ToString(),
                                VDATE = Convert.ToDateTime(rdr["VDATE"]),
                                DrCode = rdr["DrCode"].ToString(),
                                CRCode = rdr["CRCode"].ToString(),
                                Party_CD = rdr["Party_CD"].ToString(),
                                DrAmount = Convert.ToDecimal(rdr["DrAmount"]),
                                CrAmount = Convert.ToDecimal(rdr["CrAmount"]),
                                VDesc = rdr["VDesc"].ToString(),
                                GroupCode = rdr["GroupCode"].ToString(),
                                BranchCode = rdr["BranchCode"].ToString(),
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
        public List<BBPSPaymentReportVM> GetBBPSPaymentReport(DateTime fromDate, DateTime toDate, string? smCode, string dbName, bool isLive)
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

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            reportData.Add(new BBPSPaymentReportVM
                            {
                                SmCode = dr["SmCode"].ToString(),
                                TxnReferenceId = dr["TxnReferenceId"].ToString(),
                                BillNumber = dr["BillNumber"].ToString(),
                                Ahead = dr["Ahead"].ToString(),
                                Vno = dr["Vno"].ToString(),
                                Vdate = dr["Vdate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["Vdate"]),
                                Creator = dr["Creator"].ToString(),
                                CreditAmt = dr["CreditAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["CreditAmt"]),
                                DebitAmt = dr["DebitAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["DebitAmt"])
                            });
                        }
                    }
                }
            }
            return reportData;
        }
    }
}
