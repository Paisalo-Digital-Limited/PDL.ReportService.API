using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class CaseHistoryResponse
    {
        public List<CaseHistoryVM> CaseHistories { get; set; }
        public int TotalCount { get; set; }
        public int UnmatchedCount { get; set; }
    }
}
