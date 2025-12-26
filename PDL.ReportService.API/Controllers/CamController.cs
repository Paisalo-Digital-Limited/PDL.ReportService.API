using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Helper;
using System.Security.Claims;

namespace PDL.ReportService.API.Controllers
{
    public class CamController : BaseApiController
    {
        private readonly ICamInterface _camInterface;
        private readonly IConfiguration _configuration;

        public CamController(IConfiguration configuration, ICamInterface camInterface) : base(configuration)
        {
            _camInterface = camInterface;
            _configuration = configuration;
        }



        [HttpPost]
        public IActionResult GetCamGeneration(string ficodes , int creatorId)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();

                if (string.IsNullOrEmpty(dbName) || ficodes == null || ficodes.Length<1 || creatorId<1)
                {
                    return BadRequest(new { message = resourceManager.GetString("NULLDBNAME") });
                }

                var result = _camInterface.GetCamGeneration(ficodes, creatorId, dbName, isLive);

                return Ok(new
                {
                    message = resourceManager.GetString("GETSUCCESS"),
                    data = result
                    
                });
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetCamGeneration_CamController");
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetFiCodeByCreator(int CreatorId)
        {
            try
            {
                string dbname = GetDBName();
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);

                List<string> lst = _camInterface.GetFiCodeByCreator(CreatorId, dbname, GetIslive());
                if (lst.Count > 0)
                {
                    return Ok(new
                    {
                        message = (resourceManager.GetString("GETSUCCESS")),
                        data = lst
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = (resourceManager.GetString("NORECORD")),
                        data = lst
                    });
                }
            }

            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetIslive(), "GetFiCodeByCreator_CamController");
                return BadRequest(new { message = (resourceManager.GetString("BADREQUEST")), data = "" });
            }
        }
        [HttpGet]
        public IActionResult GetBranchTypeWiseReport([FromQuery] BranchTypeWiseReportVM model)
        {
            try
            {
                string dbname = GetDBName();
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Declare the out parameter for FI details
                List<FIInfoVM> fiDetails;

                // Pass the VM object directly and get both branch and FI details
                var result = _camInterface.GetBranchTypeWiseReport(model, dbname, GetIslive(), out fiDetails);

                if ((result != null && result.Count > 0) || (fiDetails != null && fiDetails.Count > 0))
                {
                    return Ok(new
                    {
                        message = resourceManager.GetString("GETSUCCESS"),
                        data = new
                        {
                            Branches = result,       // Branch/Dealer info
                            FIDetails = fiDetails    // FI info
                        }
                    });
                }

                return Ok(new
                {
                    message = resourceManager.GetString("NORECORD"),
                    data = new
                    {
                        Branches = result,
                        FIDetails = fiDetails
                    }
                });
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(
                    ex,
                    _configuration,
                    GetIslive(),
                    "GetBranchTypeWiseReport_CamController"
                );

                return BadRequest(new
                {
                    message = resourceManager.GetString("BADREQUEST"),
                    data = ""
                });
            }
        }




    }
}
