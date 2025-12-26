using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class FIInfoVM
    {
        public string FICode { get; set; }
        public string SmCode { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        // ✅ ESIGN (NEW – OPTIONAL)
        public string? EsignStatus { get; set; }     // BorrSignStatus
        //public DateTime? EsignDate { get; set; }    // Creation_Date
    }
}

