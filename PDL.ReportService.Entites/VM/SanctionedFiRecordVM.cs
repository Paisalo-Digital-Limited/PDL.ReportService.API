using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class SanctionedFiRecordVM
    {
        public int TotalCount { get; set; }
        public string FICode { get; set; }
        public string Creator { get; set; }
        public decimal SanctionedAmt { get; set; }
        public string SchemeCode { get; set; }
        public string CustomerName { get; set; }
    }
}
