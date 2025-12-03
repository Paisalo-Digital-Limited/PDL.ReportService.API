using Azure;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NPOI.HSSF.Record;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Entites.VM.ReportVM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Helper;
using Renci.SshNet.Messages;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;

namespace PDL.ReportService.API.Controllers
{
    public class ReportsController : BaseApiController
    {
        private readonly IReports _reports;
        private readonly IConfiguration _configuration;

        public ReportsController(IConfiguration configuration, IReports reports) : base(configuration)
        {
            _reports = reports;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult GetCaseHistoryBySmCodes([FromBody] List<string> smCodes, [FromQuery] int PageNumber, [FromQuery] int PageSize)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();

                if (string.IsNullOrEmpty(dbName) || smCodes == null || smCodes.Count == 0)
                {
                    return BadRequest(new { message = resourceManager.GetString("NULLDBNAME") });
                }

                var result = _reports.GetCaseHistoryBySmCodes(smCodes, dbName, isLive, PageNumber, PageSize);

                return Ok(new
                {
                    message = resourceManager.GetString("GETSUCCESS"),
                    data = result.CaseHistories,
                    totalCount = result.TotalCount,
                    invalidSmCodeCount = result.InvalidSmCodeCount,
                    noHistoryCount = result.NoHistoryCount
                });
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetCaseHistoryBySmCodes_Reports");
                return BadRequest(ex.Message);
            }
        }

        #region   ----- Cso Collection Report and BBPS Report   ------  Satish Maurya
        [HttpPost]
        public IActionResult GetCsoCollectionReport(DateTime fromDate, DateTime toDate, string csoCode, string dbtype, int PageNumber, int PageSize)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();
                List<CsoCollectionReportModelVM> result = _reports.GetCsoCollectionReport(fromDate, toDate, csoCode, dbtype, dbName, isLive, PageNumber, PageSize);
                if (result != null && result.Any())
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }

                return NotFound(new { message = resourceManager.GetString("GETFAIL") });
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetCsoCollectionReport_Reports");
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult GetCsoCollectionReportAllCases(DateTime fromDate, DateTime toDate, string dbtype, int PageNumber, int PageSize)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();
                List<CsoCollectionReportModelVM> result = _reports.GetCsoCollectionReportAllCases(fromDate, toDate, dbtype, dbName, isLive, PageNumber, PageSize);

                if (result != null && result.Any())
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }

                return NotFound(new { message = resourceManager.GetString("GETFAIL") });
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetCsoCollectionReportAllCases_Reports");
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult GetBBPSPaymentReport(DateTime fromDate, DateTime toDate, string? smCode, int PageNumber, int PageSize)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();
                List<BBPSPaymentReportVM> result = _reports.GetBBPSPaymentReport(fromDate, toDate, smCode, dbName, isLive, PageNumber, PageSize);

                if (result != null && result.Any())
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }

                return NotFound(new { message = resourceManager.GetString("GETFAIL") });
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetBBPSPaymentReport_Reports");
                return BadRequest(ex.Message);
            }
        }
        #endregion

        [HttpPost]
        public IActionResult GetEMIInformation(IFormFile file, string dbtype, int PageNumber, int PageSize)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();

                if (file == null || file.Length == 0)
                {
                    return BadRequest("File not selected or empty");
                }

                List<string> smcodes = Helper.ReadExcelFileToSMCodeList(file);

                if (smcodes.Count == 0)
                {
                    return BadRequest("No Smcodes found in the Excel file");
                }
                List<EMIInformationVM> eMIs = new List<EMIInformationVM>();
                foreach (var smcode in smcodes)
                {
                    List<EMIInformationVM> emiList = _reports.GetEMIInformation(smcode, dbtype, dbName, isLive, PageNumber, PageSize);
                    if (emiList != null && emiList.Any())
                    {
                        eMIs.AddRange(emiList);
                    }
                }
                if (eMIs.Count > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = eMIs
                    });
                }
                else
                {
                    return NotFound(new { message = resourceManager.GetString("GETFAIL") });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "UploadExcelFile_Reports");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetLoansWithoutInstallments(string dDbName, int PageNumber, int PageSize)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();

                if (string.IsNullOrEmpty(dbName))
                {
                    return BadRequest(new { message = resourceManager.GetString("NULLDBNAME") });
                }
                int totalCount;
                var result = _reports.GetLoansWithoutInstallments(dDbName, dbName, isLive, PageNumber, PageSize, out totalCount);

                if (result != null && result.Any())
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result,
                        totalRecords = totalCount
                    });
                }

                return NotFound(new { message = resourceManager.GetString("GETFAIL") });
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetLoansWithoutInstallments_Reports");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetLoansWithoutDisbursements(string dDbName, int PageNumber, int PageSize)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();

                if (string.IsNullOrEmpty(dbName))
                {
                    return BadRequest(new { message = resourceManager.GetString("NULLDBNAME") });
                }
                int totalCount;
                var result = _reports.GetLoansWithoutDisbursements(dDbName, dbName, isLive, PageNumber, PageSize, out totalCount);

                if (result != null && result.Any())
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result,
                        totalRecords = totalCount
                    });
                }

                return NotFound(new { message = resourceManager.GetString("GETFAIL"), totalRecords = 0 });
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetLoansWithoutDisbursements_Reports");
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult GetDuplicateDIBVouchers(string dbtype, int PageNumber, int PageSize)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();
                List<DuplicateDIBVoucherVM> result = _reports.GetDuplicateDIBVouchers(dbtype, dbName, isLive, PageNumber, PageSize);

                if (result != null && result.Any())
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }

                return NotFound(new { message = resourceManager.GetString("GETFAIL") });
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetDuplicateDIBVouchers_Reports");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetRcDisbursementTransactionReport(string dDbName, int PageNumber, int PageSize, DateTime fromDate, DateTime toDate, string creator)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();

                if (string.IsNullOrEmpty(dbName))
                {
                    return BadRequest(new { message = resourceManager.GetString("NULLDBNAME") });
                }
                int totalCount;
                var result = _reports.GetRcDisbursementTransactionReport(dDbName, dbName, isLive, PageNumber, PageSize, out totalCount, fromDate, toDate, creator);

                if (result != null && result.Any())
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result,
                        totalRecords = totalCount
                    });
                }

                return NotFound(new { message = resourceManager.GetString("GETFAIL"), totalRecords = 0 });
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetRcDisbursementTransactionReport_Reports");
                return BadRequest(ex.Message);
            }
        }

        #region Get CSO Report based on Creator and BranchCode
        [HttpGet]
        public IActionResult GetCSOReport(int creatorId, string branchCode, int pageNumber, int pageSize)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();
                List<CSOReportVM> result = _reports.GetCSOReport(creatorId, branchCode, dbName, isLive, pageNumber, pageSize);

                if (result.Count > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetCSOReport_Reports");
                return BadRequest(new { message = resourceManager.GetString("BADREQUEST") });
            }
        }
        #endregion

        #region Get EMI Details based on SMCode
        [HttpGet]
        public IActionResult GetLedgerReport(string smCode, int pageNumber, int pageSize)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();
                List<LedgerReportVM> result = _reports.GetLedgerReport(smCode, dbName, isLive, pageNumber, pageSize);

                if (result.Count > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetLedgerReport_Reports");
                return BadRequest(new { message = resourceManager.GetString("BADREQUEST") });
            }
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAccountAggregatorReport([FromQuery] long? fiCode, [FromQuery] int? creatorId, [FromQuery] string? smCode)
        {
            string dbName = GetDBName();
            bool isLive = GetIslive();

            try
            {
                if ((fiCode == null || creatorId == null) && string.IsNullOrWhiteSpace(smCode))
                {
                    return BadRequest(new { message = "Either (FiCode and Creator) or SMCode is required." });
                }

                string activeUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _reports.GetAccountAggregatorReportAsync(fiCode, creatorId, smCode, activeUser, isLive, dbName);

                if (result != null)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETFAIL")
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetAccountAggregatorReport_Reports");
                return BadRequest(new { message = resourceManager.GetString("BADREQUEST") });
            }
        }
        [HttpPost]
        public async Task<IActionResult> SMCodeValidation([FromForm] SMCodeValidationVM file)
        {
            string dbname = GetDBName();
            bool isLive = GetIslive();

            try
            {
                dynamic result = await _reports.SMCodeValidation(file, dbname, isLive);
                if (result != null)
                {
                    return Ok(new
                    {
                        message = (resourceManager.GetString("GETSUCCESS")),
                        data = result
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("NORECORD"),
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "SMCodeValidation_Reports");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOverdueRecords(
    [FromQuery] string creatorId,
    [FromQuery] string branchCode,
    [FromQuery] string groupCode,
    [FromQuery] DateTime? startDate,
    [FromQuery] DateTime? endDate,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string searchTerm = "",
    [FromQuery] int? minOverdueDays = null,
    [FromQuery] string sortBy = "OverDueDays",
    [FromQuery] string sortOrder = "DESC")
        {
            try
            {
                string dbname = GetDBName();
                bool isLive = GetIslive();
                // Basic validation


                // Prepare pagination + filter request
                var request = new PaginationRequest<OverduePenalties>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                    MinOverdueDays = minOverdueDays,
                    SortBy = sortBy,
                    SortOrder = sortOrder.ToUpper(),
                    Filters = new
                    {
                        CreatorID = creatorId,
                        BranchCode = branchCode,
                        GroupCode = groupCode,
                        StartDate = startDate.Value,
                        EndDate = endDate.Value
                    }
                };

                // Call your service layer
                var response = await _reports.GetOverdueRecordsAsync(request, dbname, isLive);
                if (response != null)
                {
                    return Ok(new
                    {
                        message = (resourceManager.GetString("GETSUCCESS")),
                        data = response
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("NORECORD"),
                    });
                }

            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetOverdueRecords_Reports");
                return BadRequest(new { message = ex.Message });
            }
        }




        [HttpGet]
        public async Task<IActionResult> ExportOverdueExcel(string creatorId, string branchCode, string groupCode, string startDate, string endDate)
        {
            try
            {
                string dbname = GetDBName();
                bool isLive = GetIslive();
                var response = await _reports.ExportOverdueExcel(creatorId, branchCode, groupCode, startDate, endDate, dbname, isLive);

                if (response != null)
                {
                    byte[] exceldata = null;
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Overdue Records");


                        string[] headers = {
            "FI ID", "Full Name", "Branch Name", "Group Name", "Creator Name",
            "EMI Date", "Rate", "Overdue Days", "Total Overdue Amount", "Creation Date"
        };


                        for (int i = 0; i < headers.Length; i++)
                        {
                            var cell = worksheet.Cell(1, i + 1);
                            cell.Value = headers[i];
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        }


                        for (int i = 0; i < response.Count; i++)
                        {
                            var row = response[i];
                            int rowNum = i + 2;

                            worksheet.Cell(rowNum, 1).Value = row.FI_Id;
                            worksheet.Cell(rowNum, 2).Value = row.FullName ?? "";
                            worksheet.Cell(rowNum, 3).Value = row.BranchName ?? "";
                            worksheet.Cell(rowNum, 4).Value = row.GroupName ?? "";
                            worksheet.Cell(rowNum, 5).Value = row.CreatorName ?? "";


                            worksheet.Cell(rowNum, 6).Value = row.EMIDate != DateTime.MinValue ? row.EMIDate : (DateTime?)null;
                            worksheet.Cell(rowNum, 6).Style.DateFormat.Format = "yyyy-MM-dd";


                            worksheet.Cell(rowNum, 7).Value = row.Rate;
                            worksheet.Cell(rowNum, 8).Value = row.OverDueDays;
                            worksheet.Cell(rowNum, 9).Value = row.TotalOverDueAmount;
                            worksheet.Cell(rowNum, 9).Style.NumberFormat.Format = "#,##0.00";


                            worksheet.Cell(rowNum, 10).Value = DateTime.Now;
                            worksheet.Cell(rowNum, 10).Style.DateFormat.Format = "yyyy-MM-dd HH:mm:ss";
                        }



                        worksheet.Columns().AdjustToContents();
                        worksheet.Rows().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


                        worksheet.RangeUsed().SetAutoFilter();


                        worksheet.SheetView.FreezeRows(1);


                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            exceldata = stream.ToArray();
                        }
                    }



                    return Ok(new
                    {
                        message = (resourceManager.GetString("GETSUCCESS")),
                        data = exceldata
                    });


                }
                else
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("NORECORD"),
                    });
                }
            }

            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "ExportOverdueExcel_Reports");
                return BadRequest(new
                {
                    message = ex.Message
                });
            }

        }

        #region CibilReport
        [HttpGet]
        public IActionResult GetCibilReport(string searchDate)
        {
            string dbName = GetDBName();
            bool isLive = GetIslive();
            try
            {
                var response = _reports.GetCibilReport(searchDate,dbName,isLive);

                if (response.Count>0)
                {
                    return Ok(new
                    {
                        message = (resourceManager.GetString("GETSUCCESS")),
                        data = response
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("NORECORD"),
                        data=""
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration,GetIslive(), "GetCibilReport_Reports");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")), data = "" });
            }
        }
        #endregion
    }


    //[HttpGet]
    //public async Task<IActionResult> GetOverdueRecords(
    //        [FromQuery] int pageNumber = 1,
    //        [FromQuery] int pageSize = 10,
    //        [FromQuery] string searchTerm = "",
    //        [FromQuery] int? minOverdueDays = null,
    //        [FromQuery] string sortBy = "OverDueDays",
    //        [FromQuery] string sortOrder = "DESC")
    //{
    //    try
    //    {
    //        // Validation
    //        if (pageNumber < 1)
    //        {
    //            return BadRequest(new { message = "Page number must be greater than 0" });
    //        }

    //        if (pageSize < 1 || pageSize > 100)
    //        {
    //            return BadRequest(new { message = "Page size must be between 1 and 100" });
    //        }

    //        var request = new PaginationRequest<OverduePenalties>
    //        {
    //            PageNumber = pageNumber,
    //            PageSize = pageSize,
    //            SearchTerm = searchTerm,
    //            MinOverdueDays = minOverdueDays,
    //            SortBy = sortBy,
    //            SortOrder = sortOrder.ToUpper(),
    //            data = new List<OverduePenalties>()
    //        };



    //        var response = await _overdueService.GetOverdueRecordsAsync(request);


    //        return Ok(response);
    //    }
    //    catch (Exception ex)
    //    {

    //        return StatusCode(500, new { message = "An error occurred while fetching overdue records", error = ex.Message });
    //    }
    //}


}