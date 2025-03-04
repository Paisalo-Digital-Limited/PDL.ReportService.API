using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Helper;
using System.Security.Claims;
using System.Xml.Linq;

namespace PDL.ReportService.API.Controllers
{
    public class BranchDashboardController : BaseApiController
    {
        private readonly IBranchDashboardService _branchDashboardService;
        private readonly IConfiguration _configuration;
        public BranchDashboardController(IBranchDashboardService branchDashboardService, IConfiguration configuration) : base(configuration)
        {
            _branchDashboardService = branchDashboardService;
            _configuration = configuration;
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
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });
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
                List<BranchDashBoardDataModel> res = _branchDashboardService.GetBranchDashboardData(CreatorBranchId, FromDate, ToDate, Type,pageNumber,pageSize, GetIslive());
                if (res.Count>0)
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
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });
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
                        data =result
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 201,
                        message = resourceManager.GetString("GETFAIL")
                        
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetCreators_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });

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
                        message = resourceManager.GetString("GETFAIL")
                        
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetBranches_BranchDashboard");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = "" });

            }
        }

        #endregion

    }
}
