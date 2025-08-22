using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetCamGeneration(string ficodes , string creator)
        {
            try
            {
                string dbName = GetDBName();
                bool isLive = GetIslive();

                if (string.IsNullOrEmpty(dbName) || ficodes == null || ficodes.Length<1 || creator.Length<0)
                {
                    return BadRequest(new { message = resourceManager.GetString("NULLDBNAME") });
                }

                var result = _camInterface.GetCamGeneration(ficodes, creator, dbName, isLive);

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


    }
}
