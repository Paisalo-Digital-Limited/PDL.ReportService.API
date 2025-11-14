using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class LedgerResponse
    {
        public LedgerHeader Header { get; set; }
        public List<LedgerRow> Rows { get; set; }
    }
    public class LedgerRow
    {
        public string Date { get; set; }
        public string Particulars { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public DateTime VoucherDate { get; set; }
        public string OverdueDays { get; set; }
    }
    public class LedgerHeader
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string RegdNo { get; set; }
        public decimal InvestAmount { get; set; }
        public int TotalInstallment { get; set; }
    }

}
