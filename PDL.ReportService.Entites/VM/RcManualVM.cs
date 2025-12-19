using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class RcManualVM
    {
        public string SmCode { get; set; }
        public string TxnId { get; set; }
        public DateTime? TxnDate { get; set; }
        public string Amt { get; set; }
    }
}
