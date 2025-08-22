using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDL.ReportService.Logics.Helper;
using System.Resources;

namespace PDL.ReportService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
   [Authorize]
    public class BaseApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public BaseApiController(IConfiguration configuration)
        {
            _configuration = configuration; 
        }
        protected static ResourceManager resourceManager = new ResourceManager("PDL.ReportService.API.Extensions.ReturnMessages", typeof(BaseApiController).Assembly);
        protected bool GetIslive()
        {
            bool val = false;
            val = _configuration.GetValue<bool>("isliveDb");
            return val;
        }
        protected string GetDBName()
        {
            return Helper.GetDBName(_configuration);
        }
    }
}
