using Microsoft.AspNetCore.Mvc;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Helper;

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


    }
}
