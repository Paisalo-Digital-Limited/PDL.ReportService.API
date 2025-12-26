using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class BranchTypeWiseReportVM
    {
        public string SearchMode { get; set; }      // CREATOR / DATE
        public string Type { get; set; }            // SOURCING / DISBURSED / ESIGN
        public int? CreatorId { get; set; }        // CREATOR mode

        public string ? CreatorName { get; set; }
        public string? BranchCode { get; set; }     // optional
        public string ?BranchName { get; set; }
        public DateTime? FromDate { get; set; }     // DATE mode
        public DateTime? ToDate { get; set; }       // DATE mode
    }
}
