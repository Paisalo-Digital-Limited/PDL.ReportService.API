using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class TrialBalanceResponseVM
    {
        public List<TrialBalanceRow> Rows { get; set; } = new List<TrialBalanceRow>();
        public decimal? TotalDebit { get; set; }
        public decimal? TotalCredit { get; set; }
        public decimal? Difference { get; set; }
        public string DifferenceType { get; set; } = "";
    }

    public class TrialBalanceRow
    {
        public string Ahead { get; set; }
        public string Description { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Balance { get; set; }
    }

    public class AHeaddata
    {
        public List<string> ahead { get; set; }
    }
}
