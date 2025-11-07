using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Helper;
using System.Data;

namespace PDL.ReportService.API.Controllers
{
    public class AllReportsController : BaseApiController
    {
        private readonly IAllReportsService _allReportsService;
        private readonly IConfiguration _configuration;

        public AllReportsController(IConfiguration configuration, IAllReportsService allReportsService) : base(configuration)
        {
            _allReportsService = allReportsService;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult AllReportsList([FromBody] AllReportParameterVM obj)
        {
            string dbname = GetDBName();
            bool isLive = GetIslive();

            try
            {
                DataTable result = _allReportsService.AllReportsList(obj, dbname, isLive);
                if (result.Rows.Count > 0)
                {
                    return Ok(new
                    {
                        message = (resourceManager.GetString("GETSUCCESS")),
                        data = JsonConvert.SerializeObject(result)
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("NORECORD"),
                        data = JsonConvert.SerializeObject(result)

                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "AllReportsList_AllReports");
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult RcPostReportsList(int CreatorID, string? VDate, string? VNO, string? FromDate, string? ToDate, int? PageSize, int? PageNumber)
        {
            string dbname = GetDBName();
            bool isLive = GetIslive();

            try
            {
                DataTable result = _allReportsService.RcPostReportsList(CreatorID, VDate, VNO, FromDate, ToDate, PageSize, PageNumber, dbname, isLive);
                if (result.Rows.Count > 0)
                {
                    return Ok(new
                    {
                        message = (resourceManager.GetString("GETSUCCESS")),
                        data = JsonConvert.SerializeObject(result)
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("NORECORD"),
                        data = JsonConvert.SerializeObject(result)

                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "RcPostReportsList_AllReports");
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult DownloadLedgerPdf(string SmCode)
        {
            string dbname = GetDBName();
            bool isLive = GetIslive();

            try
            {
                bool res = _allReportsService.GetSmCode(SmCode,dbname,isLive);
                if (res == true)
                {
                    var pdfBytes = _allReportsService.GenerateLedgerPdf(SmCode, dbname, isLive);
                    if (pdfBytes != null)
                    {
                        return Ok(new
                        {
                            message = (resourceManager.GetString("GETSUCCESS")),
                            data = File(
                                      pdfBytes,
                                      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                      $"Ledger_{SmCode}_{DateTime.Now:yyyyMMdd}.xlsx"
                                  )
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = resourceManager.GetString("NORECORD"),
                            data = pdfBytes

                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("SMCODENOTFOUND"),
                        data = res

                    });
                }
                
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "DownloadLedgerPdf_AllReports");
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
