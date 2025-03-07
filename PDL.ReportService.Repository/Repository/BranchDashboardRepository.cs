using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.BLL;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public List<BranchDashBoardDataModel> GetBranchDashboardData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, int pageNumber, int pageSize, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetBranchDashboardData(CreatorBranchId, FromDate, ToDate, Type,pageNumber,pageSize, islive);
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
        public List<GetFirstEsign> GetFirstEsign(int CreatorId, long FiCode, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetFirstEsign(CreatorId, FiCode, islive);
            }
        }
        public List<GetSecoundEsign> GetSecoundEsign(int CreatorId, long FiCode, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetSecoundEsign(CreatorId, FiCode, islive);
            }
        }
        public object GetCaseNotVisible(int CreatorId, long FiCode, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetCaseNotVisible(CreatorId, FiCode, islive);
            }
        }
        public List<TotalDemandAndCollection> GetTotalDemandAndCollection(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetTotalDemandAndCollection(CreatorBranchId, FromDate, ToDate, islive);
            }
        }
        public List<GetCollectionCountVM> GetCollectionCount(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetCollectionCount(CreatorBranchId, FromDate, ToDate,Type, islive);
            }
        }
        public List<GetDemandCountVM> GetDemandCount(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetDemandCount(CreatorBranchId, FromDate, ToDate, islive);
            }
        }
        public List<RaiseQueryVM> GetRaiseQuery(string activeuser, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetRaiseQuery(activeuser, islive);
            }
        }
        public int InsertRaiseQuery(RaiseQueryVM obj, string activeUser, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                int codeCreator = branchDashboard.InsertRaiseQuery(obj, activeUser, islive);
                return codeCreator;
            }
        }
    }
}