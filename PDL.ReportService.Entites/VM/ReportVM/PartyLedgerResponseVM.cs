using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class PartyLedgerResponseVM
    {
        public LedgerSummaryVM Summary { get; set; }
        public List<LedgerTransactionVM> Transactions { get; set; }
    }
}
