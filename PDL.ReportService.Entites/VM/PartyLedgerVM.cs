using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class PartyLedgerVM
    {
        public string SmCode { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime EffectiveDt { get; set; }
        public string Vno { get; set; }
        public decimal Dr { get; set; }
        public decimal Cr { get; set; }
        public string Ahead { get; set; }
        public string Narr { get; set; }
        public string OvDrDays { get; set; }
        public decimal LateFeeIntt { get; set; }
    }
    public class PartyLedgerVMresponse
    {
        public List<PartyLedgerVM> list { get; set; }
        public int StatusCode { get; set; }
    }
}
