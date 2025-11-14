using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class LedgerSummaryVM
    {
        public string InstCode { get; set; }
        public string partyname { get; set; }
        public string PartyCode { get; set; }
        public int TotalInstallments { get; set; }
        public decimal InvestAmount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal CurrentBalance { get; set; }
        public int OverdueCount { get; set; }
    }
}
