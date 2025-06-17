using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class SmWithoutChqVM
    {
        public string Code { get; set; }
        public string Subs_Name { get; set; }
        public decimal? Invest { get; set; }
        public DateTime? Dt_Fin { get; set; }
        public string Loan_Type { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
