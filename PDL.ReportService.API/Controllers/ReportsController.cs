using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Helper;
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
    }
}