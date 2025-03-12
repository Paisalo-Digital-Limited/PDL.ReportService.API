using PDL.ReportService.Entites.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PDL.ReportService.Interfaces.Interfaces
{
    public interface IBranchDashboardService
    {
        BranchDashBoardVM GetMasterData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string? flag, bool islive);
        CollectionStatusVM CollectionStatus(string SmCode, bool islive);
        List<BranchDashBoardDataModel> GetBranchDashboardData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, int pageNumber, int pageSize, string? flag, bool islive);
        List<FiCreatorMaster> GetCreators(string activeuser, bool islive);
        List<BranchWithCreator> GetBranches(string CreatorId, string activeuser, bool islive);
        List<GetFirstEsign> GetFirstEsign(int CreatorId, long FiCode, bool islive);
        List<GetSecoundEsign> GetSecoundEsign(int CreatorId, long FiCode, bool islive);
        object GetCaseNotVisible(int CreatorId, long FiCode, bool islive);
        List<TotalDemandAndCollection> GetTotalDemandAndCollection(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, bool islive);
        List<GetCollectionCountVM> GetCollectionCount(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, int pageNumber, int pageSize, string? flag, bool islive);
        List<GetDemandCountVM> GetDemandCount(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string? flag, bool islive);
        List<RaiseQueryVM> GetRaiseQuery(string activeuser, bool islive);
        int InsertRaiseQuery(RaiseQueryVM obj, string activeuser, bool islive);
        string RequestForDeath(RequestForDeathVM obj, string activeuser, bool islive);
        int NOCQuery(NOCQueryVM obj, string activeuser, bool islive);
        List<ReadyForPuchVM> GetReadyforPushData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, int pageNumber, int pageSize, bool islive);
        int ReadyforPushData(long id, string activeuser, bool islive);
    }
}