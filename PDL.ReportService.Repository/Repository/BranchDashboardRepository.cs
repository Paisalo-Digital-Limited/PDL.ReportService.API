using Microsoft.Extensions.Configuration;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.BLL;

namespace PDL.ReportService.Repository.Repository
{
    public class BranchDashboardRepository: BaseBLL ,IBranchDashboardService
    {
        private readonly IConfiguration _configuration;
        public BranchDashboardRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetMasterData(string CreatorID, string BranchCode, DateTime? FromDate, DateTime? ToDate, string activeuser, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetMasterData(CreatorID,BranchCode,FromDate,ToDate,activeuser,islive);
            }
        }

    }
}
