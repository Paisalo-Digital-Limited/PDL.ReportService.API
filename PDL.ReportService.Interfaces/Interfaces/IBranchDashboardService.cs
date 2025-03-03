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
        string GetMasterData(string? CreatorID, string? BranchCode, DateTime? FromDate, DateTime? ToDate, string activeuser, bool islive);
        //DropdownResult GetCreatorsAndBranches(string CreatorID, string activeuser, bool islive);
        List<FiCreatorMaster> GetCreators(string activeuser, bool islive);
        List<BranchWithCreator> GetBranches(string CreatorId, string activeuser, bool islive);
    }
}
