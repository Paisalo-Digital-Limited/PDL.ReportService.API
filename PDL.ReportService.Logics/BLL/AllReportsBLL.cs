using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Office2016.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Entites.VM.ReportVM;
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
        public bool GetSmCode(string smCode, string dbname, bool isLive)
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
        public DataTable GetICICIQrCallbackResponse(string? FromDate, string? ToDate, int? PageSize, int? PageNumber, string dbname, bool isLive)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = _credManager.getConnections(dbname, isLive))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_GetAllReportsList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetICICIQrCallbackResponse");
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


        //Ramesh general ledger report excel generation

        public DateTime FindLastMonthDate(DateTime vdate)
        {
            return new DateTime(vdate.Year, vdate.Month, DateTime.DaysInMonth(vdate.Year, vdate.Month));
        }

        public async Task<byte[]> GenerateGeneralLedgerExcel(string SmCode, string dbname, bool isLive)
        {
            PartyLedgerResponseVM ledgerData = new PartyLedgerResponseVM();
            ledgerData.Transactions = new List<LedgerTransactionVM>();
            ledgerData.Summary = null;

            byte[] excelBytes = Array.Empty<byte>();


            var rCdata = await GetRcList(SmCode, dbname, isLive);
            var EMIdata = await GetEmiData(SmCode, dbname, isLive);


            foreach (var emi in EMIdata)
            {
                rCdata.Add(new RCdata
                {
                    VDATE = emi.INS_DUE_DT,
                    VDESC = $"Inst No {emi.INSTALL} Due",
                    AHEAD = "INLNINS",
                    DR = 0,
                    CR = 0,
                    InstllDR = emi.AMT,
                    CODE = emi.CODE,
                    REMARKS = $"Installment No: {emi.INSTALL} Principal Amount: {emi.INST_PRINC}"
                });
            }
            if (rCdata != null && rCdata.Count > 0)
            {
                var groups = rCdata
                    .Where(x => x.VDATE.HasValue)
                    .GroupBy(x => new { Year = x.VDATE.Value.Year, Month = x.VDATE.Value.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month);

                var newRows = new List<RCdata>();
                foreach (var grp in groups)
                {
                    var sampleDate = grp.First().VDATE.Value;
                    DateTime endDate = FindLastMonthDate(sampleDate);

                    bool exists = rCdata.Any(x =>
                        x.VDATE.HasValue &&
                        x.VDATE.Value.Date == endDate.Date &&
                        !string.IsNullOrEmpty(x.VDESC) &&
                        x.VDESC.IndexOf("End of Month", StringComparison.OrdinalIgnoreCase) >= 0);

                    if (!exists)
                    {
                        newRows.Add(new RCdata
                        {
                            VDATE = endDate,
                            VDESC = "End of Month Entry",
                            AHEAD = "Auto-generated",
                            DR = 0,
                            CR = 0,
                            CODE = "",
                            REMARKS = $"Auto entry for {endDate:MMMM yyyy}"
                        });
                    }
                }

                if (newRows.Count > 0) rCdata.AddRange(newRows);
                rCdata = rCdata.OrderBy(x => x.VDATE ?? DateTime.MinValue).ToList();
            }
            var perEmiResults = ComputePerEmiOverdues(EMIdata, rCdata);
            var emiLookup = perEmiResults.ToDictionary(
              p => (Date: p.DueDate.Date, Amt: Math.Round(p.EmiAmount, 2)),
              p => p);
            decimal runningBalance = 0m;
            DateTime? prevDate = null;
            var orderedRc = rCdata.OrderBy(x => x.VDATE ?? DateTime.MinValue).ToList();

            foreach (var item in orderedRc)
            {
                if (!item.VDATE.HasValue)
                    continue;


                decimal debit = item.DR;
                decimal credit = item.CR;
                runningBalance += (debit - credit);


                int days = 0;
                if (prevDate.HasValue)
                {
                    days = (int)(item.VDATE.Value.Date - prevDate.Value.Date).TotalDays;
                    if (days < 0) days = 0;
                }
                prevDate = item.VDATE.Value.Date;


                decimal emiAmountFlag = item.InstllDR;
                decimal emiPaidAmount = 0m;
                decimal emiOverdueAmount = 0m;
                int emiOverdueDays = 0;
                decimal emiCumulative = 0m;
                decimal installmentBalance = 0m;
                decimal installmentDebit = item.InstllDR;
                decimal installmentCredit = item.DR;

                if (emiAmountFlag > 0m)
                {

                    var key = (Date: item.VDATE.Value.Date, Amt: Math.Round(emiAmountFlag, 2));
                    if (!emiLookup.TryGetValue(key, out var emiRes))
                    {

                        emiRes = perEmiResults.FirstOrDefault(p => p.DueDate.Date == item.VDATE.Value.Date);
                    }

                    if (emiRes != null)
                    {
                        emiPaidAmount = emiRes.PaidForThisEmi;
                        emiOverdueAmount = emiRes.OverdueAmount;
                        emiOverdueDays = emiRes.OverdueDays;
                        emiCumulative = emiRes.CumulativeOverdue;
                        installmentBalance = Math.Round(emiRes.EmiAmount - emiRes.PaidForThisEmi, 2);
                        installmentDebit = emiRes.EmiAmount;
                        installmentCredit = emiRes.PaidForThisEmi;
                    }
                    else
                    {

                        installmentBalance = emiAmountFlag;
                        emiOverdueAmount = 0m;
                        emiOverdueDays = 0;
                        emiCumulative = 0m;
                    }
                }


                var row = new LedgerTransactionVM
                {
                    Date = item.VDATE.Value,
                    Particulars = item.VDESC,
                    VoucherNo = item.VNO,
                    Debit = decimal.Round(debit, 2),
                    Credit = decimal.Round(credit, 2),
                    Balance = decimal.Round(runningBalance, 2),
                    Days = days,

                    Interest = decimal.Round(emiOverdueAmount, 2),
                    CumulativeInterest = decimal.Round(emiCumulative, 2),
                    InvestAmount = 0m,
                    FromDate = item.VDATE,
                    ToDate = item.VDATE,
                    InstCode = item.CODE ?? "0",
                    InstallmentBalance = decimal.Round(installmentBalance, 2),
                    Installmentcredit = decimal.Round(installmentCredit, 2),
                    Installmentdebit = decimal.Round(installmentDebit, 2),
                    InstallmentNumber = 0,
                    InstallmentDueDate = item.VDATE,
                    InstallmentAmount = decimal.Round(emiPaidAmount, 2),
                    InstallmentPrincipal = 0m,
                    InstallmentInterest = 0m,
                    InstallmentStatus = item.AHEAD ?? "0",
                    BranchCode = "0",
                    Remarks = item.REMARKS
                };

                ledgerData.Transactions.Add(row);
            }


            var ledgerSummaryVM = await LedgerSummaryVM(SmCode, dbname, isLive);
            if (ledgerSummaryVM != null && ledgerSummaryVM.Count > 0)
            {
                ledgerData.Summary = ledgerSummaryVM[0];
            }


            excelBytes = await ExportPartyLedgerToExcel(ledgerData, ledgerData.Summary?.partyname ?? string.Empty);
            return excelBytes;
        }


        private List<PerEmiResult> ComputePerEmiOverdues(List<EMIdata> EMIdata, List<RCdata> rCdata)
        {
            var results = new List<PerEmiResult>();
            DateTime today = DateTime.Today;


            var emis = EMIdata
                .Where(e => e.INS_DUE_DT != DateTime.MinValue)
                .OrderBy(e => e.INS_DUE_DT.Date)
                .Select((e, idx) => new
                {
                    EmiNo = idx + 1,
                    DueDate = e.INS_DUE_DT.Date,
                    EmiAmount = Math.Round(e.AMT, 2)
                })
                .ToList();


            decimal totalPaymentsTillToday = rCdata
                .Where(rc => rc.VDATE.HasValue && rc.VDATE.Value.Date <= today && rc.CR > 0m && string.Equals(rc.AHEAD, "INLNINS", StringComparison.OrdinalIgnoreCase))
                .Sum(rc => rc.CR);

            decimal remainingPool = Math.Round(totalPaymentsTillToday, 2);
            decimal cumulOverdue = 0m;

            foreach (var e in emis)
            {
                decimal paid = 0m;
                if (remainingPool > 0m)
                {
                    paid = Math.Min(remainingPool, e.EmiAmount);
                    remainingPool -= paid;
                }

                decimal overdue = 0m;
                int overdueDays = 0;

                if (e.DueDate <= today && paid < e.EmiAmount)
                {
                    overdue = Math.Round(e.EmiAmount - paid, 2);
                    overdueDays = (today - e.DueDate).Days;
                    if (overdueDays < 0) overdueDays = 0;
                }

                cumulOverdue = Math.Round(cumulOverdue + overdue, 2);

                results.Add(new PerEmiResult
                {
                    EmiNo = e.EmiNo,
                    DueDate = e.DueDate,
                    EmiAmount = e.EmiAmount,
                    PaidForThisEmi = paid,
                    OverdueAmount = overdue,
                    OverdueDays = overdueDays,
                    CumulativeOverdue = cumulOverdue
                });
            }

            return results;
        }
        public async Task<List<RCdata>> GetRcList(string SMCode, string dbname, bool isLive)
        {
            var rCdatas = new List<RCdata>();

            using (SqlConnection con = _credManager.getConnections(dbname, isLive))
            {
                await con.OpenAsync();

                using (var cmd = new SqlCommand("Usp_GetRCDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SMCode", SMCode);
                    cmd.Parameters.AddWithValue("@Mode", "RC");

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new RCdata
                            {
                                Fi_ID = reader["Fi_ID"] != DBNull.Value ? Convert.ToInt64(reader["Fi_ID"]) : 0,
                                VNO = reader["VNO"]?.ToString(),
                                VDATE = reader["VDATE"] != DBNull.Value ? Convert.ToDateTime(reader["VDATE"]) : (DateTime?)null,
                                VDESC = reader["VDESC"]?.ToString(),
                                AHEAD = reader["AHEAD"]?.ToString(),
                                CR = reader["CR"] != DBNull.Value ? Convert.ToDecimal(reader["CR"]) : 0,
                                DR = reader["DR"] != DBNull.Value ? Convert.ToDecimal(reader["DR"]) : 0,
                                CODE = reader["CODE"]?.ToString(),
                                REMARKS = reader["REMARKS"]?.ToString()
                            };
                            rCdatas.Add(row);
                        }
                    }
                }
            }

            return rCdatas;
        }
        public async Task<List<EMIdata>> GetEmiData(string SMCode, string dbname, bool isLive)
        {
            var emidata = new List<EMIdata>();

            using (SqlConnection con = _credManager.getConnections(dbname, isLive))
            {
                await con.OpenAsync();

                using (var cmd = new SqlCommand("Usp_GetRCDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SMCode", SMCode);
                    cmd.Parameters.AddWithValue("@Mode", "EMI");

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new EMIdata
                            {
                                Fi_ID = reader["Fi_ID"] != DBNull.Value ? Convert.ToInt64(reader["Fi_ID"]) : 0,
                                INSTALL = reader["INSTALL"] != DBNull.Value ? Convert.ToInt32(reader["INSTALL"]) : 0,
                                CODE = reader["CODE"] != DBNull.Value ? reader["CODE"].ToString() : string.Empty,
                                INS_DUE_DT = Convert.ToDateTime(reader["INS_DUE_DT"] != DBNull.Value ? Convert.ToDateTime(reader["INS_DUE_DT"]) : (DateTime?)null),
                                AMT = reader["AMT"] != DBNull.Value ? Convert.ToDecimal(reader["AMT"]) : 0m,
                                INST_PRINC = reader["INST_PRINC"] != DBNull.Value ? Convert.ToDecimal(reader["INST_PRINC"]) : 0m,

                            };
                            emidata.Add(row);
                        }
                    }
                }
            }

            return emidata;
        }
        public async Task<List<LedgerSummaryVM>> LedgerSummaryVM(string SMCode, string dbname, bool isLive)
        {
            var LedgerSummary = new List<LedgerSummaryVM>();

            using (SqlConnection con = _credManager.getConnections(dbname, isLive))
            {
                await con.OpenAsync();

                using (var cmd = new SqlCommand("Usp_GetRCDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SMCode", SMCode);
                    cmd.Parameters.AddWithValue("@Mode", "Summary");

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var s = new LedgerSummaryVM
                            {
                                InstCode = reader["InstCode"]?.ToString(),
                                PartyCode = reader["PartyCode"]?.ToString(),
                                TotalInstallments = reader["TotalInstallments"] != DBNull.Value ? Convert.ToInt32(reader["TotalInstallments"]) : 0,
                                InvestAmount = reader["InvestAmount"] != DBNull.Value ? Convert.ToDecimal(reader["InvestAmount"]) : 0m,
                                FromDate = Convert.ToDateTime(reader["FromDate"] != DBNull.Value ? (DateTime?)reader["FromDate"] : null),
                                ToDate = Convert.ToDateTime(reader["ToDate"] != DBNull.Value ? (DateTime?)reader["ToDate"] : null),
                                CurrentBalance = reader["CurrentBalance"] != DBNull.Value ? Convert.ToDecimal(reader["CurrentBalance"]) : 0m,
                                OverdueCount = reader["OverdueCount"] != DBNull.Value ? Convert.ToInt32(reader["OverdueCount"]) : 0,
                                partyname = reader["partyname"]?.ToString()



                            };
                            LedgerSummary.Add(s);
                        }
                    }
                }
            }

            return LedgerSummary;
        }
        public async Task<byte[]> ExportPartyLedgerToExcel(PartyLedgerResponseVM ledgerData, string partyName)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Party Ledger");

                // Set up the header section
                CreateHeader(worksheet, ledgerData.Summary, partyName);

                // Create the main ledger table
                CreateLedgerTable(worksheet, ledgerData.Transactions);

                // Create footer
                CreateFooter(worksheet, ledgerData.Transactions);

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Save to memory stream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
        private void CreateHeader(IXLWorksheet ws, LedgerSummaryVM summary, string partyName)
        {
            // ====== Title ======
            ws.Range(1, 1, 1, 15).Merge();
            ws.Cell(1, 1).Value = "Party Ledger";
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 14;
            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ====== Company Info ======
            ws.Range(2, 1, 2, 15).Merge();
            ws.Cell(2, 1).Value = "Paisalo Digital Limited";
            ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Range(3, 1, 3, 15).Merge();
            ws.Cell(3, 1).Value = "Paisalo House, 74, Gandhi Nagar, NH-2, Agra 282003";
            ws.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ====== Party Info Block ======
            int r = 5;
            ws.Cell(r, 1).Value = "From Date";
            ws.Cell(r, 2).Value = summary.FromDate.ToString("dd-MM-yyyy");
            ws.Cell(r, 4).Value = "To Date";
            ws.Cell(r, 5).Value = summary.ToDate.ToString("dd-MM-yyyy");

            r++;
            ws.Cell(r, 1).Value = "Party Name";
            ws.Cell(r, 2).Value = partyName;
            ws.Cell(r, 4).Value = "InstCode";
            ws.Cell(r, 5).Value = summary.InstCode;

            r++;
            ws.Cell(r, 1).Value = "Total No of Inst";
            ws.Cell(r, 2).Value = summary.TotalInstallments;
            ws.Cell(r, 4).Value = "Invest";
            ws.Cell(r, 5).Value = summary.InvestAmount;
            ws.Cell(r, 5).Style.NumberFormat.Format = "#,##0.00";

            // ====== Header Rows ======
            int headerTop = r + 3;      // e.g. row 9
            int headerBottom = headerTop + 1; // second header row

            // --- Top Row (Grouped Headers) ---
            ws.Cell(headerTop, 1).Value = "Date";
            ws.Cell(headerTop, 2).Value = "Particulars";
            ws.Cell(headerTop, 3).Value = "Voucher";

            ws.Range(headerTop, 4, headerTop, 6).Merge();
            ws.Cell(headerTop, 4).Value = "General Ledger";

            ws.Range(headerTop, 7, headerTop, 9).Merge();
            ws.Cell(headerTop, 7).Value = "Installment Ledger";

            ws.Range(headerTop, 10, headerTop, 13).Merge();
            ws.Cell(headerTop, 10).Value = "Overdue  Charges"; // double space as requested

            ws.Cell(headerTop, 14).Value = "Vch No";
            ws.Cell(headerTop, 15).Value = "Overdue Intt Recd";

            // --- Second Row (Subheaders) ---
            ws.Cell(headerBottom, 1).Value = "Date";
            ws.Cell(headerBottom, 2).Value = "";
            ws.Cell(headerBottom, 3).Value = "Date";

            ws.Cell(headerBottom, 4).Value = "Debit";
            ws.Cell(headerBottom, 5).Value = "Credit";
            ws.Cell(headerBottom, 6).Value = "Balance";

            ws.Cell(headerBottom, 7).Value = "Debit";
            ws.Cell(headerBottom, 8).Value = "Credit";
            ws.Cell(headerBottom, 9).Value = "Balance";

            ws.Cell(headerBottom, 10).Value = "Days";
            ws.Cell(headerBottom, 11).Value = "Intt.";
            ws.Cell(headerBottom, 12).Value = "Cumm_Intt";
            ws.Cell(headerBottom, 13).Value = "Balance"; // added new column for Overdue Balance

            ws.Cell(headerBottom, 14).Value = ""; // Vch No
            ws.Cell(headerBottom, 15).Value = ""; // Overdue Intt Recd

            // ====== Header Styling ======
            for (int col = 1; col <= 15; col++)
            {
                var topCell = ws.Cell(headerTop, col);
                var bottomCell = ws.Cell(headerBottom, col);

                topCell.Style.Font.Bold = true;
                topCell.Style.Fill.BackgroundColor = XLColor.LightGray;
                topCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                topCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                topCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                bottomCell.Style.Font.Bold = true;
                bottomCell.Style.Fill.BackgroundColor = XLColor.LightGray;
                bottomCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                bottomCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                bottomCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            // ====== Column Widths ======
            ws.Column(1).Width = 12;   // Date
            ws.Column(2).Width = 50;   // Particulars
            ws.Column(3).Width = 12;   // Voucher Date
            ws.Column(4).Width = 12;   // Gen Debit
            ws.Column(5).Width = 12;   // Gen Credit
            ws.Column(6).Width = 12;   // Gen Balance
            ws.Column(7).Width = 12;   // Inst Debit
            ws.Column(8).Width = 12;   // Inst Credit
            ws.Column(9).Width = 12;   // Inst Balance
            ws.Column(10).Width = 12;  // Days
            ws.Column(11).Width = 14;  // Intt
            ws.Column(12).Width = 14;  // Cumm_Intt
            ws.Column(13).Width = 14;  // Overdue Balance
            ws.Column(14).Width = 12;  // Vch No
            ws.Column(15).Width = 16;  // Overdue Intt Recd

            // Wrap text in Particulars
            ws.Column(2).Style.Alignment.WrapText = true;
        }
        private void CreateLedgerTable(IXLWorksheet ws, List<LedgerTransactionVM> txns)
        {
            int headerTop = 9;
            int startRow = headerTop + 3;
            int r = startRow;

            foreach (var t in txns)
            {

                ws.Cell(r, 1).Value = t.Date == DateTime.MinValue ? "" : t.Date.ToString("dd-MM-yyyy");
                ws.Cell(r, 2).Value = t.Particulars;
                ws.Cell(r, 3).Value = t.Date == DateTime.MinValue ? "" : t.Date.ToString("dd-MM-yyyy");


                if (t.Debit != 0) ws.Cell(r, 4).Value = t.Debit;
                if (t.Credit != 0)
                {
                    ws.Cell(r, 5).Value = t.Credit;
                    ws.Cell(r, 5).Style.Font.FontColor = XLColor.Red;
                }
                ws.Cell(r, 6).Value = t.Balance;
                if (t.Balance < 0) ws.Cell(r, 6).Style.Font.FontColor = XLColor.Red;


                ws.Cell(r, 7).Value = t.Debit;
                ws.Cell(r, 8).Value = t.Credit;
                ws.Cell(r, 9).Value = t.Balance;


                if (t.Days.HasValue) ws.Cell(r, 10).Value = t.Days.Value;
                ws.Cell(r, 11).Value = t.Interest;
                ws.Cell(r, 12).Value = t.CumulativeInterest;



                ws.Cell(r, 14).Value = t.VoucherNo;
                ws.Cell(r, 15).Value = t.InstallmentInterest;


                for (int c = 4; c <= 13; c++)
                {
                    ws.Cell(r, c).Style.NumberFormat.Format = "#,##0.00";
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                }
                ws.Cell(r, 10).Style.NumberFormat.Format = "0";


                for (int c = 1; c <= 15; c++)
                {
                    ws.Cell(r, c).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(r, c).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                }

                r++;
            }


            int dataStart = startRow;
            int dataEnd = r - 1;
            if (dataEnd >= dataStart)
            {
                ws.Cell(r, 2).Value = "Total";
                ws.Cell(r, 2).Style.Font.Bold = true;

                ws.Cell(r, 4).FormulaA1 = $"=SUM(D{dataStart}:D{dataEnd})";
                ws.Cell(r, 5).FormulaA1 = $"=SUM(E{dataStart}:E{dataEnd})";
                ws.Cell(r, 7).FormulaA1 = $"=SUM(G{dataStart}:G{dataEnd})";
                ws.Cell(r, 8).FormulaA1 = $"=SUM(H{dataStart}:H{dataEnd})";
                ws.Cell(r, 9).Value = "Remaining Days";
                ws.Cell(r, 9).Style.Font.Bold = true;
                ws.Cell(r, 11).FormulaA1 = $"=SUM(K{dataStart}:K{dataEnd})";
                ws.Cell(r, 12).FormulaA1 = $"=SUM(L{dataStart}:L{dataEnd})";
                ws.Cell(r, 13).FormulaA1 = $"=SUM(M{dataStart}:M{dataEnd})";

                for (int c = 1; c <= 15; c++)
                {
                    ws.Cell(r, c).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    ws.Cell(r, c).Style.Fill.BackgroundColor = XLColor.LightGray;
                    ws.Cell(r, c).Style.Font.Bold = true;
                }
            }
        }
        private void CreateFooter(IXLWorksheet worksheet, List<LedgerTransactionVM> transactions)
        {
            int footerRow = 10 + transactions.Count + 3;

            // Disclaimer
            worksheet.Cell(footerRow, 1).Value = "Unless the constituent notifies the company immediately of any discrepancy found by him / her in this statement of account, it will be taken that he / she has found the account correct.";
            worksheet.Range(footerRow, 1, footerRow, 10).Merge();
            worksheet.Cell(footerRow, 1).Style.Font.FontSize = 9;
            worksheet.Cell(footerRow, 1).Style.Font.Italic = true;

            footerRow++;
            worksheet.Cell(footerRow, 1).Value = "This is a system generated output and requires no signature.";
            worksheet.Range(footerRow, 1, footerRow, 10).Merge();
            worksheet.Cell(footerRow, 1).Style.Font.FontSize = 9;
            worksheet.Cell(footerRow, 1).Style.Font.Italic = true;
        }

        //Trial Balalnce Started
        //list of ahead
        public async Task<List<RCdata>> GetAllAhead(string dbname, bool isLive)
        {
              RCdata amast = new RCdata();
            List<RCdata> ahead = new List<RCdata>();

            using (SqlConnection con = _credManager.getConnections(dbname, isLive))
            {
                await con.OpenAsync();

                using (var cmd = new SqlCommand("Usp_GetRC_ForTrialBalance", con))
                {
                    cmd.Parameters.AddWithValue("@mode", "Ahead");
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new RCdata {

                                AHEAD = reader["Code"]?.ToString()?.Trim() ?? string.Empty

                            };
                            ahead.Add(row);
                        }
                    }
                }
            }

            return ahead;
        }




        //calculating ahead data
        public async Task<byte[]> GetTrailBalance(List<string> Ahead, DateTime startdate, DateTime enddate, string dbname, bool isLive)
        {
            var rawRows = await GetRawRcForTrialBalance(Ahead, startdate, enddate, dbname, isLive);
            var response = await ComputeTrialBalanceFromRawRows(rawRows);
            string companyName = "Paisalo Digital Limited";
            byte[] trailbalance = await ExportTrialBalanceToExcel_WithSubtotals(response, startdate, enddate, companyName);
            return trailbalance;
        }

        public async Task<List<RCdata>> GetRawRcForTrialBalance(List<string> aheads, DateTime fromDate, DateTime toDate, string dbname, bool isLive)
        {
            var results = new List<RCdata>();
            try
            {
               
                if (aheads == null || aheads.Count == 0)
                    return results;
                var normalizedCsv = string.Join(",", aheads
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim()));

                using (SqlConnection con = _credManager.getConnections(dbname, isLive))
                {
                    await con.OpenAsync();

                    using (var cmd = new SqlCommand("Usp_GetRC_ForTrialBalance", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                        cmd.Parameters.AddWithValue("@ToDate", toDate.Date);
                        cmd.Parameters.AddWithValue("@Aheads", normalizedCsv);
                        cmd.Parameters.AddWithValue("@mode", "TB");

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var row = new RCdata
                                {
                                    AHEAD = reader["AHEAD"]?.ToString()?.Trim() ?? string.Empty,
                                    VDESC = reader["DESCRIPTION"]?.ToString()?.Trim() ?? string.Empty,
                                    DR = reader["DR"] != DBNull.Value ? Convert.ToDecimal(reader["DR"]) : 0m,
                                    CR = reader["CR"] != DBNull.Value ? Convert.ToDecimal(reader["CR"]) : 0m
                                };
                                results.Add(row);
                            }
                        }
                    }
                }

                return results;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            return results;
        }
        public async Task<byte[]> ExportTrialBalanceToExcel_WithSubtotals(TrialBalanceResponseVM tb, DateTime startdate, DateTime endate, string reportTitle = "Paisalo Digital Limited")
        {
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Trial Balance");
                ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;

                
                CreateStyledHeader(ws, reportTitle, startdate, endate);

                int headerRow = 5;
                CreateTableHeader(ws, headerRow);

                var r = headerRow + 1;
                foreach (var row in tb.Rows)
                {
                    ws.Cell(r, 1).Value = row.Ahead;
                    ws.Cell(r, 2).Value = row.Description ?? "";

                    if (row.Debit.HasValue && row.Debit.Value != 0m)
                    {
                        var ccell = ws.Cell(r, 3);
                        ccell.Value = row.Debit.Value;
                        ccell.Style.NumberFormat.Format = "#,##0.00";
                        ccell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    }
                    else
                    {
                        ws.Cell(r, 3).Value = "";
                    }

                    if (row.Credit.HasValue && row.Credit.Value != 0m)
                    {
                        var ccell = ws.Cell(r, 4);
                        ccell.Value = row.Credit.Value;
                        ccell.Style.NumberFormat.Format = "#,##0.00";
                        ccell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    }
                    else
                    {
                        ws.Cell(r, 4).Value = "";
                    }

                    for (int c = 1; c <= 4; c++)
                    {
                        ws.Cell(r, c).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Cell(r, c).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }

                    r++;
                }

              
                ws.Cell(r, 1).Value = "GRAND TOTAL";
                ws.Range(r, 1, r, 2).Merge();
                ws.Cell(r, 1).Style.Font.Bold = true;
                ws.Cell(r, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                if (tb.TotalDebit.HasValue)
                {
                    var td = ws.Cell(r, 3);
                    td.Value = tb.TotalDebit.Value;
                    td.Style.NumberFormat.Format = "#,##0.00";
                    td.Style.Font.Bold = true;
                    td.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    td.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                else
                {
                    ws.Cell(r, 3).Value = "";
                }

                if (tb.TotalCredit.HasValue)
                {
                    var tc = ws.Cell(r, 4);
                    tc.Value = tb.TotalCredit.Value;
                    tc.Style.NumberFormat.Format = "#,##0.00";
                    tc.Style.Font.Bold = true;
                    tc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    tc.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
                else
                {
                    ws.Cell(r, 4).Value = "";
                }

               
                r += 2;
                var diffVal = tb.Difference ?? 0m;
                var diffAbs = Math.Abs(diffVal);
                var diffType = string.IsNullOrWhiteSpace(tb.DifferenceType) ? (diffVal > 0 ? "Debit" : diffVal < 0 ? "Credit" : "Balanced") : tb.DifferenceType;
                ws.Cell(r, 1).Value = $"Difference In Trial Balance = {diffAbs:N2} {diffType}";
                ws.Range(r, 1, r, 4).Merge();

                
                ws.Column(1).Width = 18;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Columns().AdjustToContents();

                using (var ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }





        public async Task<TrialBalanceResponseVM> ComputeTrialBalanceFromRawRows(List<RCdata> rawRows)
        {
            var response = new TrialBalanceResponseVM();
            if (rawRows == null || rawRows.Count == 0) return response;

            
            var aheadOrder = rawRows
                .Select((x, idx) => new { x.AHEAD, idx })
                .GroupBy(x => (x.AHEAD ?? "").Trim())
                .Select(g => new { Ahead = g.Key, FirstIndex = g.Min(x => x.idx) })
                .OrderBy(x => x.FirstIndex)
                .Select(x => x.Ahead)
                .ToList();

          
            var gsKey = aheadOrder.FirstOrDefault(a => string.Equals(a, "GSUSPANC", StringComparison.OrdinalIgnoreCase));
            if (gsKey != null)
            {
                aheadOrder.Remove(gsKey);
                aheadOrder.Add(gsKey);
            }

           
            var grouped = rawRows
                .GroupBy(r => (r.AHEAD ?? "").Trim())
             
                .OrderBy(g =>
                {
                    var idx = aheadOrder.IndexOf(g.Key);
                    return idx >= 0 ? idx : int.MaxValue;
                });

            foreach (var g in grouped)
            {
               
                var debitSum = g.Sum(x => x.DR);
                var creditSum = g.Sum(x => x.CR);

             
                var net = Math.Round(debitSum - creditSum, 2);

                decimal? debitToShow = null;
                decimal? creditToShow = null;

                if (net > 0m)
                    debitToShow = net;
                else if (net < 0m)
                    creditToShow = Math.Abs(net);

                
                string description =
                    g.LastOrDefault(x => !string.IsNullOrWhiteSpace(x.VDESC))?.VDESC
                    ?? g.First().VDESC
                    ?? "";

                var row = new TrialBalanceRow
                {
                    Ahead = g.Key,
                    Description = description,
                    Debit = debitToShow,
                    Credit = creditToShow
                };

                response.Rows.Add(row);
            }

         
            decimal displayedTotalDebit = response.Rows.Where(r => r.Debit.HasValue).Sum(r => r.Debit.Value);
            decimal displayedTotalCredit = response.Rows.Where(r => r.Credit.HasValue).Sum(r => r.Credit.Value);

            displayedTotalDebit = Math.Round(displayedTotalDebit, 2);
            displayedTotalCredit = Math.Round(displayedTotalCredit, 2);

            response.TotalDebit = displayedTotalDebit != 0m ? (decimal?)displayedTotalDebit : null;
            response.TotalCredit = displayedTotalCredit != 0m ? (decimal?)displayedTotalCredit : null;

            response.Difference = Math.Round(displayedTotalDebit - displayedTotalCredit, 2);

            if (response.Difference > 0m)
                response.DifferenceType = "Debit";
            else if (response.Difference < 0m)
                response.DifferenceType = "Credit";
            else
                response.DifferenceType = "Balanced";

            return response;
        }


        private void CreateStyledHeader(IXLWorksheet ws, string companyName, DateTime fromDate, DateTime toDate)
        {

            ws.Range(1, 1, 1, 4).Merge();
            ws.Cell(1, 1).Value = companyName;
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 16;
            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            ws.Range(2, 1, 2, 4).Merge();
            ws.Cell(2, 1).Value = "TRIAL BALANCE OF ALL ACCOUNT HEADS";
            ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell(2, 1).Style.Font.Underline = XLFontUnderlineValues.Single;
            ws.Cell(2, 1).Style.Font.Bold = true;


            ws.Cell(1, 5).Value = "From Date";
            ws.Cell(1, 6).Value = fromDate.ToString("dd/MM/yyyy");
            ws.Cell(2, 5).Value = "To Date";
            ws.Cell(2, 6).Value = toDate.ToString("dd/MM/yyyy");


            ws.Row(4).Height = 8;
        }


        private void CreateTableHeader(IXLWorksheet ws, int headerRow)
        {
            ws.Cell(headerRow, 1).Value = "AHEAD";
            ws.Cell(headerRow, 2).Value = "DESCRIPTION";
            ws.Cell(headerRow, 3).Value = "DEBIT";
            ws.Cell(headerRow, 4).Value = "CREDIT";

            for (int c = 1; c <= 4; c++)
            {
                var cell = ws.Cell(headerRow, c);
                cell.Style.Font.Bold = true;
                cell.Style.Font.Underline = XLFontUnderlineValues.Single;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            }


            ws.Cell(headerRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            ws.Cell(headerRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        }

        public DataTable GetNewCasesForAMonth(string? FromDate, string? ToDate, string dbname, bool isLive)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = _credManager.getConnections(dbname, isLive))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_GetAllReportsList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "NewCasesForAMonth");
                    cmd.Parameters.AddWithValue("@FromDate", FromDate);
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                    con.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }

}

