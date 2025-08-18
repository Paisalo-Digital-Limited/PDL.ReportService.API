using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class LoanWithoutDisbVoucherVM
    {
        public string Code { get; set; }
        public string SubsName { get; set; }
        public decimal? Invest { get; set; }
        public DateTime? DtFin { get; set; }
        public string LoanType { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
