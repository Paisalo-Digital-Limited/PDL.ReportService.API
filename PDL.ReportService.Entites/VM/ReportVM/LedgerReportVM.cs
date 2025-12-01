using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class LedgerReportVM
    {
        public string SMCode { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public Double DebitAmount { get; set; }
        public Double CreditAmount { get; set; }
        public string Narration { get; set; }
        public string AHEAD { get; set; }
        public DateTime OverdueDays { get; set; }
        public string LateFeeInterest { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
