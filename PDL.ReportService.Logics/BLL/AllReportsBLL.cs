using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Logics.Credentials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;
using Paragraph = iTextSharp.text.Paragraph;

namespace PDL.ReportService.Logics.BLL
{
    public class AllReportsBLL : BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;

        public AllReportsBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
        }
        public DataTable AllReportsList(AllReportParameterVM model, string dbname, bool isLive)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = _credManager.getConnections(dbname, isLive))
            {
                using (var cmd = new SqlCommand("Usp_GetAllReportsList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "AllReportsList");
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@ReportName", (object?)model.ReportName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DatabaseName", (object?)model.DatabaseName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Creator", (object?)model.Creator ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@BranchCodeFrom", (object?)model.BranchCodeFrom ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@BranchCodeTo", (object?)model.BranchCodeTo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FromDate", (object?)model.FromDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToDate", (object?)model.ToDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SchemeType", (object?)model.SchemeType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FO1", (object?)model.FO1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FO2", (object?)model.FO2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToCode", (object?)model.ToCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Area", (object?)model.Area ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tag", (object?)model.Tag ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FICode", (object?)model.FICode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Code", (object?)model.Code ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ahead", (object?)model.Ahead ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name", (object?)model.Name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FinAmount", (object?)model.FinAmount ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FromCode", (object?)model.FromCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Trench", (object?)model.Trench ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MobNo", (object?)model.MobNo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Endpoints", (object?)model.Endpoints ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModuleType", (object?)model.ModuleType ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("@Mode", (object?)model.Mode ?? DBNull.Value);
                    con.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
        public DataTable RcPostReportsList(int CreatorID, string? VDate, string? VNO, string? FromDate, string? ToDate, int? PageSize, int? PageNumber, string dbname, bool isLive)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = _credManager.getConnections(dbname, isLive))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_GetAllReportsList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "RcPostReportsList");
                    cmd.Parameters.AddWithValue("@CreatorID", CreatorID);
                    cmd.Parameters.AddWithValue("@VDate", VDate);
                    cmd.Parameters.AddWithValue("@VNO", VNO);
                    cmd.Parameters.AddWithValue("@FromDate", FromDate);
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);
                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                    con.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
        public byte[] GenerateLedgerExcel(List<LedgerRow> rows, LedgerHeader header)
        {
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Ledger");

                int currentRow = 1;

                // Title
                ws.Cell(currentRow, 1).Value = "Paisalo Digital Limited";
                ws.Cell(currentRow, 1).Style.Font.Bold = true;
                ws.Cell(currentRow, 1).Style.Font.FontSize = 16;
                ws.Range(currentRow, 1, currentRow, 5).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                currentRow++;

                ws.Cell(currentRow, 1).Value = "Personal Ledger";
                ws.Cell(currentRow, 1).Style.Font.Bold = true;
                ws.Cell(currentRow, 1).Style.Font.FontSize = 14;
                ws.Range(currentRow, 1, currentRow, 5).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                currentRow += 2;

                // Header table
                ws.Cell(currentRow, 1).Value = "From Date";
                ws.Cell(currentRow, 2).Value = header.FromDate;
                ws.Cell(currentRow, 3).Value = "To Date";
                ws.Cell(currentRow, 4).Value = header.ToDate;
                currentRow++;

                ws.Cell(currentRow, 1).Value = "Code";
                ws.Cell(currentRow, 2).Value = header.PartyCode;
                ws.Cell(currentRow, 3).Value = "Party Name";
                ws.Cell(currentRow, 4).Value = header.PartyName;
                currentRow++;

                ws.Cell(currentRow, 1).Value = "Regd No";
                ws.Cell(currentRow, 2).Value = header.RegdNo;
                ws.Cell(currentRow, 3).Value = "Invest Amount";
                ws.Cell(currentRow, 4).Value = header.InvestAmount;
                currentRow++;

                ws.Cell(currentRow, 2).Value = "Total No of Installments";
                ws.Cell(currentRow, 3).Value = header.TotalInstallment;
                currentRow += 2;

                // Ledger table header
                string[] excelHeaders = { "Date", "Particulars", "Debit", "Credit", "Balance" };
                for (int i = 0; i < excelHeaders.Length; i++)
                {
                    ws.Cell(currentRow, i + 1).Value = excelHeaders[i];
                    ws.Cell(currentRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    ws.Cell(currentRow, i + 1).Style.Font.Bold = true;
                }
                currentRow++;
                int startDataRow = currentRow;

                // Ledger rows
                foreach (var r in rows)
                {
                    ws.Cell(currentRow, 1).Value = r.Date;
                    ws.Cell(currentRow, 2).Value = r.Particulars;
                    ws.Cell(currentRow, 3).Value = r.Debit;
                    ws.Cell(currentRow, 4).Value = r.Credit;
                    ws.Cell(currentRow, 5).Value = r.Balance;

                    // Format numbers
                    ws.Cell(currentRow, 3).Style.NumberFormat.Format = "#,##0.00";
                    ws.Cell(currentRow, 4).Style.NumberFormat.Format = "#,##0.00";
                    ws.Cell(currentRow, 5).Style.NumberFormat.Format = "#,##0.00";

                    currentRow++;
                }

                ws.Cell(currentRow, 2).Value = "Total";
                ws.Cell(currentRow, 3).FormulaA1 = $"=SUM(C{startDataRow}:C{currentRow - 1})";
                ws.Cell(currentRow, 4).FormulaA1 = $"=SUM(D{startDataRow}:D{currentRow - 1})";
                ws.Cell(currentRow, 5).FormulaA1 = $"=SUM(E{startDataRow}:E{currentRow - 1})";

                ws.Range(currentRow, 2, currentRow, 5).Style.Font.Bold = true;
                currentRow += 2;

                // Footer notes
                ws.Cell(currentRow, 1).Value = "Unless the constituent notifies the company immediately of any discrepancy found by him/her in this statement of account, it will be taken that he/she has found the account correct.";
                ws.Range(currentRow, 1, currentRow + 1, 5).Merge().Style.Alignment.WrapText = true;
                currentRow += 2;
                ws.Cell(currentRow, 1).Value = "This is a system generated output and requires no signature.";
                ws.Range(currentRow, 1, currentRow, 5).Merge();
                ws.Columns().AdjustToContents();

                using (var ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

        public LedgerResponse GetLedgerData(string SmCode, string dbname, bool isLive)
        {
            var result = new LedgerResponse();
            result.Rows = new List<LedgerRow>();

            using (SqlConnection con = _credManager.getConnections(dbname, isLive))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_GetRcLedgerData", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SmCode", SmCode);
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            result.Header = new LedgerHeader
                            {
                                FromDate = dr["FromDate"] != DBNull.Value ? dr["FromDate"].ToString() : string.Empty,
                                ToDate = dr["ToDate"] != DBNull.Value ? dr["ToDate"].ToString() : string.Empty,
                                PartyCode = dr["PartyCode"] != DBNull.Value ? dr["PartyCode"].ToString() : string.Empty,
                                PartyName = dr["PartyName"] != DBNull.Value ? dr["PartyName"].ToString() : string.Empty,
                                //RegdNo = dr["RegdNo"] != DBNull.Value ? dr["RegdNo"].ToString() : string.Empty,
                                InvestAmount = dr["Invest"] != DBNull.Value ? Convert.ToDecimal(dr["Invest"]) : 0m,
                                TotalInstallment = dr["TotalInstallment"] != DBNull.Value ? Convert.ToInt32(dr["TotalInstallment"]) : 0
                            };
                        }

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                result.Rows.Add(new LedgerRow
                                {
                                    Date = dr["Date"] != DBNull.Value ? dr["Date"].ToString() : string.Empty,
                                    Particulars = dr["Particulars"] != DBNull.Value ? dr["Particulars"].ToString() : string.Empty,
                                    Debit = dr["Debit"] != DBNull.Value ? Convert.ToDecimal(dr["Debit"]) : 0m,
                                    Credit = dr["Credit"] != DBNull.Value ? Convert.ToDecimal(dr["Credit"]) : 0m,
                                    Balance = dr["Balance"] != DBNull.Value ? Convert.ToDecimal(dr["Balance"]) : 0m
                                });
                            }
                        }

                    }
                }
            }

            return result;
        }
        public bool GetSmCode(string smCode,string dbname, bool isLive)
        {
            bool result = false;

            using (SqlConnection con = _credManager.getConnections(dbname, isLive))
            {
                con.Open();

                using (var cmd = new SqlCommand("Usp_GetAllReportsList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetSmCode");
                    cmd.Parameters.AddWithValue("@SmCode", smCode);

                    var res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        result = Convert.ToInt32(res) == 1;
                    }
                }
            }

            return result;
        }


    }
}
