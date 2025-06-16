using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Helper;
using Renci.SshNet.Messages;
using System.Data;
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
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetCaseHistoryBySmCodes_Reports");
                return BadRequest(ex.Message);
            }
        }
        #region   ----- Cso Collection Report and BBPS Report   ------  Satish Maurya
        [HttpPost]
        public IActionResult GetCsoCollectionReport(DateTime fromDate, DateTime toDate, string csoCode,string dbtype, int PageNumber, int PageSize)
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

                List<string> smcodes =Helper.ReadExcelFileToSMCodeList(file);

                if (smcodes.Count == 0)
                {
                    return BadRequest("No Smcodes found in the Excel file");
                }
                List<EMIInformationVM> eMIs = new List<EMIInformationVM>();
                foreach (var smcode in smcodes)
                {
                    List<EMIInformationVM> emiList = _reports.GetEMIInformation(smcode,dbtype, dbName, isLive, PageNumber, PageSize);
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
        
    }
}