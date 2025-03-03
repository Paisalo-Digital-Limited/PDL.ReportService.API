using PDL.ReportService.Entites.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Interfaces.Interfaces
{
    public interface IBranchDashboardService
    {
        BranchDashBoardVM GetMasterData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, bool islive);
        CollectionStatusVM CollectionStatus(string SmCode, bool islive);
        List<BranchDashBoardDataModel> GetBranchDashboardData(string CreatorBranchId, DateTime? FromDate, DateTime? ToDate, string Type, bool islive);
    }
}
