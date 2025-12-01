using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Credentials;
using PDL.ReportService.Logics.Helper;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        [HttpGet]
        public IActionResult GetICICIQrCallbackResponse(string? FromDate, string? ToDate, int? PageSize, int? PageNumber)
        {
            string dbname = GetDBName();
            bool isLive = GetIslive();

            try
            {
                DataTable result = _allReportsService.GetICICIQrCallbackResponse(FromDate, ToDate, PageSize, PageNumber, dbname, isLive);
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
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetICICIQrCallbackResponse_AllReports");
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpGet]
        public async Task<IActionResult> DownloadGeneralLedger(string SmCode)
        {
            string dbname = GetDBName();
            bool isLive = GetIslive();

            try
            {
                bool res = _allReportsService.GetSmCode(SmCode, dbname, isLive);
                if (res == true)
                {
                    var pdfBytes = await _allReportsService.GenerateGeneralLedgerExcel(SmCode, dbname, isLive);


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
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "DownloadGeneralLedger_AllReports");
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAheadOptions()
        
        {
            string dbname = GetDBName();
            bool isLive = GetIslive();
            try
            {
                var aheadresponces = await _allReportsService.GetAllAhead(dbname, isLive);


                if (aheadresponces != null)
                {
                    return Ok(new
                    {
                        message = (resourceManager.GetString("GETSUCCESS")),
                        data = aheadresponces
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("NORECORD"),
                        data = ""

                    });
                }
            }

            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetAheadOptions_AllReports");
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult>GetTrailBalance( [FromBody] AHeaddata listaheaddata )
        {
            string dbname = GetDBName();
            bool isLive = GetIslive();

            try
            {
                 int fyStartYear = (System.DateTime.Now.Month <= 3) ? System.DateTime.Now.Year - 1 : System.DateTime.Now.Year;
                int fyEndYear = (System.DateTime.Now.Month <= 3) ? System.DateTime.Now.Year : System.DateTime.Now.Year + 1;

                DateTime financialYearStart = new DateTime(fyStartYear, 4, 1);
                DateTime financialYearEnd = new DateTime(fyEndYear, 3, 31);

                var pdfBytes = await _allReportsService.GetTrailBalance(listaheaddata.ahead, financialYearStart, financialYearEnd, dbname, isLive);


                    if (pdfBytes != null)
                    {
                        return Ok(new
                        {
                            message = (resourceManager.GetString("GETSUCCESS")),
                            data = File(
                                      pdfBytes,
                                      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                      $"TrialBalance_{financialYearStart}_{financialYearEnd}.xlsx"
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
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetTrailBalance_AllReports");
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
