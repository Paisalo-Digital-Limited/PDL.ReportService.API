using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class LedgerRowVM
    {
        public DateTime Date { get; set; }
        public string Particulars { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public decimal Debit { get; set; }   // DR
        public decimal Credit { get; set; }  // CR
        public decimal Balance { get; set; }
        public int OverdueDays { get; set; }
        public decimal OverdueIntt { get; set; }
        public string VchNo { get; set; }
    }
}
