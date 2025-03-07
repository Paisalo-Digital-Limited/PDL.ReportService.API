using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Helper;
using Renci.SshNet.Messages;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PDL.ReportService.API.Controllers
{
    public class BranchDashboardController : BaseApiController
    {
        private readonly IBranchDashboardService _branchDashboardService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BranchDashboardController(IBranchDashboardService branchDashboardService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration)
        {
            _branchDashboardService = branchDashboardService;
            _configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }
        #region Api BranchDashboard BY--------------- Satish Maurya-------
        [HttpGet]
        public IActionResult GetMasterData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                BranchDashBoardVM res = _branchDashboardService.GetMasterData(CreatorBranchId, FromDate, ToDate, GetIslive());
                if (res != null)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL"),
                        data = ""
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetMasterData_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        #endregion
        #region Collection ---------Satish Maurya----------
        [HttpGet]
        public IActionResult CollectionStatus(string SmCode)
        {
            try
            {
                CollectionStatusVM res = _branchDashboardService.CollectionStatus(SmCode, GetIslive());
                if (res != null)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL"),
                        data = ""
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetFiCollection_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });
            }
        }
        #endregion
        #region Api BranchDashboard Count BY--------------- Satish Maurya-------
        [HttpGet]
        public IActionResult GetBranchDashboardData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, int pageNumber, int pageSize)
        {
            try
            {
                List<BranchDashBoardDataModel> res = _branchDashboardService.GetBranchDashboardData(CreatorBranchId, FromDate, ToDate, Type, pageNumber, pageSize, GetIslive());
                if (res.Count > 0)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL"),
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetBranchDashboardData_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        #endregion
        #region API GetBranchesByCreators BY--------------- Kartik -------
        [HttpGet]
        public IActionResult GetCreators()
        {
            try
            {
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<FiCreatorMaster> result = _branchDashboardService.GetCreators(activeuser, GetIslive());
                if (result != null)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL"),
                        data = ""
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetCreators_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });

            }
        }
        [HttpGet]
        public IActionResult GetBranches(string CreatorId)
        {
            try
            {
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<BranchWithCreator> result = _branchDashboardService.GetBranches(CreatorId, activeuser, GetIslive());
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetBranches_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });

            }
        }
        #endregion
        #region BranchDashboard ChatBot query Api  BY--------------- Satish Maurya-------
        [HttpGet]
        public IActionResult GetFirstEsign(int CreatorId, long FiCode)
        {
            try
            {
                List<GetFirstEsign> result = _branchDashboardService.GetFirstEsign(CreatorId, FiCode, GetIslive());
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetFirstEsign_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });

            }
        }
        [HttpGet]
        public IActionResult GetSecoundEsign(int CreatorId, long FiCode)
        {
            try
            {
                List<GetSecoundEsign> result = _branchDashboardService.GetSecoundEsign(CreatorId, FiCode, GetIslive());
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetSecoundEsign_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });

            }
        }
        [HttpGet]
        public IActionResult GetCaseNotVisible(int CreatorId, long FiCode)
        {
            try
            {
                object result = _branchDashboardService.GetCaseNotVisible(CreatorId, FiCode, GetIslive());
                if (result is int dt)
                {
                    if (dt == -1)
                    {
                        return Ok(new
                        {
                            statuscode = 205,
                            message = resourceManager.GetString("DOWNLOADONE"),//Download One Pager
                            data = result
                        });
                    }
                    else if (dt == -2)
                    {
                        return Ok(new
                        {
                            statuscode = 204,
                            message = resourceManager.GetString("IMEINOTFOUND"),//IMEI No Not Found
                            data = result
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = resourceManager.GetString("GETFAIL"),//No Record Found
                            data = JsonConvert.SerializeObject(dt)
                        });
                    }
                }
                else if (result is DataTable dataTable)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = JsonConvert.SerializeObject(dataTable)
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 400,
                        message = "Unexpected result type",
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetSecoundEsign_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });

            }
        }
        #endregion
        #region Api BranchDashboard TotalDemand && TotalCollection BY--------------- Satish Maurya-------
        [HttpGet]
        public IActionResult GetTotalDemandAndCollection(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                List<TotalDemandAndCollection> res = _branchDashboardService.GetTotalDemandAndCollection(CreatorBranchId, FromDate, ToDate, GetIslive());
                if (res.Count > 0)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetTotalDemandAndCollection_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        [HttpGet]
        public IActionResult GetCollectionCountList(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type)
        {
            try
            {
                List<GetCollectionCountVM> res = _branchDashboardService.GetCollectionCount(CreatorBranchId, FromDate, ToDate, Type, GetIslive());
                if (res.Count > 0)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetCollectionCount_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        [HttpGet]
        public IActionResult GetDemandCountList(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                List<GetDemandCountVM> res = _branchDashboardService.GetDemandCount(CreatorBranchId, FromDate, ToDate, GetIslive());
                if (res.Count > 0)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetDemandCount_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        #endregion

        [HttpGet]
        public IActionResult GetRaiseQuery(int Fi_Id)
        {
            try
            {
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<RaiseQueryVM> result = _branchDashboardService.GetRaiseQuery(Fi_Id, activeuser, GetIslive());
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetRaiseQuery_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        [HttpPost]
        public IActionResult InsertRaiseQuery(RaiseQueryVM obj)
        {
            string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                int res = _branchDashboardService.InsertRaiseQuery(obj, activeuser, GetIslive());
                if (res > 0)
                {
                    return Ok(new
                    {
                        statuscode = 200,
                        message = (resourceManager.GetString("INSERTSUCCESS")),
                        data = res
                    });
                }
                else
                {
                    return Ok(new
                    {   
                        statuscode = 201,
                        message = (resourceManager.GetString("INSERTFAIL")),
                        data = string.Empty
                    });
                }

            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "InsertRaiseQuery_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });
            }
        }
        #region Api CheckNOC BY--------------- Satish Maurya-------
        [HttpGet]
        //public IActionResult CheckNOC(string smcode)
        //{
        //    try
        //    {

        //        string result = _branchDashboardService.CheckNOC(smcode, User.FindFirstValue(ClaimTypes.NameIdentifier), GetIslive());

        //        if (result != null)
        //        {
        //            return Ok(new
        //            {
        //                statuscode = 200,
        //                message = resourceManager.GetString("GETSUCCESS"),
        //                data = result
        //            });
        //        }
        //        else
        //        {
        //            return Ok(new
        //            {
        //                statuscode = 201,
        //                message = resourceManager.GetString("GETFAIL"),
        //                data = 0
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "CheckNOC_BranchDashboard");
        //        return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });
        //    }
        //}
        public IActionResult CheckNOC(string smcode)
        {
            try
            {
                // Get the result from the BLL
                Task<string> result = _branchDashboardService.CheckNOC(smcode, User.FindFirstValue(ClaimTypes.NameIdentifier), GetIslive());

                if (result.Result == "2")
                {
                    return Ok(new
                    {
                        statuscode = 205,
                        message = "This smcode does not exist in the table",
                        data = result
                    });
                }
                else if (result.Result.StartsWith("UniqueKey: "))
                {
                    string uniqueKey = result.Result.Substring("UniqueKey: ".Length);

                    if (!string.IsNullOrEmpty(uniqueKey))
                    {
                        string msg = $"Your request has been generated successfully. Your UniqueKey is {uniqueKey}. Your request will be resolved within 48 hours.";
                        return Ok(new
                        {
                            statuscode = 205,
                            message = msg,
                            data = uniqueKey
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 205,
                            message = "UniqueKey is null or empty",
                            data = result
                        });
                    }
                }
                else
                {
                    // Check if it's JSON data and generate PDF if necessary
                    string response = _branchDashboardService.ProcessJsonForPdf(result.Result);
                    return Ok(new
                    {
                        //statuscode = 200,
                        //message = Message,
                        //data = data
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "CheckNOC_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });
            }
        }

       
        #endregion
    }

}