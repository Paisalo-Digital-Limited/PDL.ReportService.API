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
    }
}
