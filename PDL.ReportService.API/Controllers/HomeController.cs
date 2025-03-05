using Microsoft.AspNetCore.Mvc;
using PDL.ReportService.Interfaces.Interfaces;

namespace PDL.ReportService.API.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly IBranchDashboardService _branchDashboardService;
        private readonly IConfiguration _configuration;
        public HomeController(IBranchDashboardService branchDashboardService, IConfiguration configuration) : base(configuration)
        {
            _branchDashboardService = branchDashboardService;
            _configuration = configuration;
        }
        [HttpGet]
        public string Test()
        {
            string message = "Hello World!";
            return (message);
        }
    }
}
