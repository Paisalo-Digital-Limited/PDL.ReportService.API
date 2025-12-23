using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class QrPaymentResultVM
    {
        public string TxnId { get; set; }
        public DateTime? TxnDateTime { get; set; }
        public decimal Amount { get; set; }
        public string VirtualVpa { get; set; }
    }
}
