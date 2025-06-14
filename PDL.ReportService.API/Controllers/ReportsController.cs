using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        public IActionResult GetCaseHistoryBySmCodes([FromBody] List<string> smCodes)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();

                if (string.IsNullOrEmpty(dbName) || smCodes == null || smCodes.Count == 0)
                {
                    return BadRequest(new { message = resourceManager.GetString("NULLDBNAME") });
                }

                var result = _reports.GetCaseHistoryBySmCodes(smCodes, dbName, isLive);

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
    }
}