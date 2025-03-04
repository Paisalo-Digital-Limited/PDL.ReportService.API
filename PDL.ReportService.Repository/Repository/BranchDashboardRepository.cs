using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.BLL;

namespace PDL.ReportService.Repository.Repository
{
    public class BranchDashboardRepository : BaseBLL, IBranchDashboardService
    {
        private readonly IConfiguration _configuration;
        public BranchDashboardRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public BranchDashBoardVM GetMasterData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetMasterData(CreatorBranchId, FromDate, ToDate, islive);
            }
        }
        public CollectionStatusVM CollectionStatus(string SmCode, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.CollectionStatus(SmCode, islive);
            }
        }
        public List<BranchDashBoardDataModel> GetBranchDashboardData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetBranchDashboardData(CreatorBranchId, FromDate, ToDate, Type, islive);
            }
        }
        public List<FiCreatorMaster> GetCreators(string activeuser, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetCreators(activeuser, islive);
            }
        }
        public List<BranchWithCreator> GetBranches(string CreatorId, string activeuser, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetBranches(CreatorId, activeuser, islive);
            }
        }
    }
}
