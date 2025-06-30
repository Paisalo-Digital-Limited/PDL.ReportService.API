using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class TransactionsVM
    {
        public string TxnID { get; set; }
        public string Amount { get; set; }
        public string Narration { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }
        public string CurrentBalance { get; set; }
        public string BookingDate { get; set; }
        public string ValueDate { get; set; }
        public string Reference { get; set; }
    }
}
