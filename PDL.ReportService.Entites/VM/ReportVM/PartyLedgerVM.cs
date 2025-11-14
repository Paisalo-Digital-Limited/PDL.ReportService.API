using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class PartyLedgerVM
    {
        public string PartyCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PartyName { get; set; } 
        public decimal OpeningBalance { get; set; }
        public List<LedgerRowVM> Rows { get; } = new List<LedgerRowVM>();
    }
}
