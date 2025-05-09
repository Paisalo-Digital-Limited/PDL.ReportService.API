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
        public IActionResult GetMasterData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string? flag)
        {
            try
            {
                BranchDashBoardVM res = _branchDashboardService.GetMasterData(CreatorBranchId, FromDate, ToDate, flag, GetIslive());
                if (res != null)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                        data = ""
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetMasterData_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
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
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                        data = ""
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetFiCollection_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        #endregion
        #region Api BranchDashboard Count BY--------------- Satish Maurya-------
        [HttpGet]
        public IActionResult GetBranchDashboardData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, int pageNumber, int pageSize, string? flag)
        {
            try
            {
                List<BranchDashBoardDataModel> res = _branchDashboardService.GetBranchDashboardData(CreatorBranchId, FromDate, ToDate, Type, pageNumber, pageSize, flag, GetIslive());
                if (res.Count > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetBranchDashboardData_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
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
                if (result != null && result.Count > 0 && result[0].CreatorID == -1)
                {
                    return NotFound(new
                    {
 
                        message = result[0].CreatorName,
                        data = ""
 
                        message = result[0].CreatorName
 
                    });
                }

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
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetCreators_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
 
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
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetBranches_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
 
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
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetFirstEsign_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });

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
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetSecoundEsign_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });

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
                        return NotFound(new
                        {
                            message = resourceManager.GetString("DOWNLOADONE"),//Download One Pager
                            data = result
                        });
                    }
                    else if (dt == -2)
                    {
                        return NotFound(new
                        {
                            message = resourceManager.GetString("IMEINOTFOUND"),//IMEI No Not Found
                            data = result
                        });
                    }
                    else
                    {
                        return NotFound(new
                        {
                            message = resourceManager.GetString("GETFAIL"),//No Record Found
                            data = JsonConvert.SerializeObject(dt)
                        });
                    }
                }
                else if (result is DataTable dataTable)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = JsonConvert.SerializeObject(dataTable)
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = "Unexpected result type",
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetSecoundEsign_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });

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
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetTotalDemandAndCollection_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        [HttpGet]
        public IActionResult GetCollectionCountList(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, int pageNumber, int pageSize, string? flag)
        {
            try
            {
                List<GetCollectionCountVM> res = _branchDashboardService.GetCollectionCount(CreatorBranchId, FromDate, ToDate, Type, pageNumber, pageSize, flag, GetIslive());
                if (res.Count > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetCollectionCount_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        [HttpGet]
        public IActionResult GetDemandCountList(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string? flag)
        {
            try
            {
                List<GetDemandCountVM> res = _branchDashboardService.GetDemandCount(CreatorBranchId, FromDate, ToDate, flag, GetIslive());
                if (res.Count > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = res
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetDemandCount_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        #endregion
        // NOC
        [HttpPost]
        public IActionResult NOCQuery(NOCQueryVM obj)
        {
            string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                int res = _branchDashboardService.NOCQuery(obj, activeuser, GetIslive());
                if (res > 0)
                {
                    return Ok(new
                    {
                        message = (resourceManager.GetString("INSERTSUCCESS")),
                        data = res
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = (resourceManager.GetString("INSERTFAIL")),
                        data = string.Empty
                    });
                }

            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "NOCQuery_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        #region GET AND INSERT RaiseQuery BY ---- AMIT KUMAR ------
        [HttpGet]
        public IActionResult GetRaiseQuery()
        {
            try
            {
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<RaiseQueryVM> result = _branchDashboardService.GetRaiseQuery(activeuser, GetIslive());
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetRaiseQuery_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
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
                        message = (resourceManager.GetString("INSERTSUCCESS")),
                        data = res
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = (resourceManager.GetString("INSERTFAIL")),
                        data = string.Empty
                    });
                }

            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "InsertRaiseQuery_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
            }
        }

        [HttpPost]
        public IActionResult RequestForDeath([FromForm] RequestForDeathVM obj)
        {
            string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                string generatedSmcode = _branchDashboardService.RequestForDeath(obj, activeuser, GetIslive());
                if (!string.IsNullOrEmpty(generatedSmcode))
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("INSERTSUCCESS"),
                        data = generatedSmcode
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("INSERTFAIL"),
                        data = string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "RequestForDeath_BranchDashboard");
                return BadRequest(new { message = resourceManager.GetString("BADREQUEST") });
            }
        }

        #endregion

        [HttpGet]
        public IActionResult GetReadyforPushData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, int pageNumber, int pageSize)
        {
            try
            {
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<ReadyForPuchVM> result = _branchDashboardService.GetReadyforPushData(CreatorBranchId, FromDate, ToDate, pageNumber, pageSize, GetIslive());
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetReadyforPuchData_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });

            }
        }

        [HttpPost]
        public IActionResult ReadyforPushData([FromForm] long id)
        {
            try
            {
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int result = _branchDashboardService.ReadyforPushData(id, activeuser, GetIslive());
                if (result > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("PUSHRECORD"),
                        data = result
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("NOTPUSHRECORD"),
                        data = result
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "ReadyforPushData_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });

            }
        }
        #region  Notification Api  BY--------------- Satish Maurya-------
        [HttpGet]
        [Authorize]
        public IActionResult GetNotification()
        {
            try
            {
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);

                List<GetNotificationVM> result = _branchDashboardService.GetNotification(activeuser, GetIslive());
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetNotification_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });

            }
        }
        #endregion
        #region HolidayCalendar Api  BY--------------- Satish Maurya-------
        [HttpGet]
        public IActionResult GetHolidayCalendar()
        {
            try
            {
                List<GetHolidayCalendarVM> result = _branchDashboardService.GetHolidayCalendar(GetIslive());
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = result
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("GETFAIL"),
                        data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetHolidayCalendar_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });

            }
        }
        #endregion
        [HttpPost]
        [Authorize]
        public IActionResult ViewNotification(ViewNotificationVM obj)
        {
            string activeUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                int affected = _branchDashboardService.ViewNotification(obj, activeUser, GetIslive());

                if (affected > 0)
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("UPDATESUCCESS"),
                        data = affected
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        message = resourceManager.GetString("UPDATEFAIL"),
                        data = affected
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "ViewNotification_BranchDashboard");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")) });
            }
        }

    }
}