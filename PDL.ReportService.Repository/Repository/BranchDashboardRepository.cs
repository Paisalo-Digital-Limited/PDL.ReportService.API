﻿using Microsoft.Extensions.Configuration;
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
        public BranchDashBoardVM GetMasterData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string? flag, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetMasterData(CreatorBranchId, FromDate, ToDate, flag, islive);
            }
        }
        public CollectionStatusVM CollectionStatus(string SmCode, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.CollectionStatus(SmCode, islive);
            }
        }
        public List<BranchDashBoardDataModel> GetBranchDashboardData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, int pageNumber, int pageSize, string? flag, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetBranchDashboardData(CreatorBranchId, FromDate, ToDate, Type, pageNumber, pageSize, flag, islive);
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
        public List<GetCollectionCountVM> GetCollectionCount(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, int pageNumber, int pageSize, string? flag, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetCollectionCount(CreatorBranchId, FromDate, ToDate, Type, pageNumber, pageSize, flag, islive);
            }
        }
        public List<GetDemandCountVM> GetDemandCount(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string? flag, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetDemandCount(CreatorBranchId, FromDate, ToDate, flag, islive);
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
        public string RequestForDeath(RequestForDeathVM obj, string activeUser, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.RequestForDeath(obj, activeUser, islive);
            }
        }
        public int NOCQuery(NOCQueryVM obj, string activeUser, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                int codeCreator = branchDashboard.NOCQuery(obj, activeUser, islive);
                return codeCreator;
            }
        }

        public List<ReadyForPuchVM> GetReadyforPushData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, int pageNumber, int pageSize, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetReadyforPushData(CreatorBranchId, FromDate, ToDate, pageNumber, pageSize, islive);
            }
        }

        public int ReadyforPushData(long id, string activeUser, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                int codeCreator = branchDashboard.ReadyforPushData(id, activeUser, islive);
                return codeCreator;
            }
        }
        public List<GetNotificationVM> GetNotification(string activeuser, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetNotification(activeuser,islive);
            }
        }
        public List<GetHolidayCalendarVM> GetHolidayCalendar(bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.GetHolidayCalendar(islive);
            }
        }
        public int ViewNotification(ViewNotificationVM obj, string activeuser, bool islive)
        {
            using (BranchDashboardBLL branchDashboard = new BranchDashboardBLL(_configuration))
            {
                return branchDashboard.ViewNotification(obj, activeuser, islive);
            }
        }
    }
}