using Microsoft.AspNetCore.Mvc;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Helper;

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
    }
}
