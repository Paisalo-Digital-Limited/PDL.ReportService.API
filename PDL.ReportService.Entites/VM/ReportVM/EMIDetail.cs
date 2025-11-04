using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class EMIDetail
    {
        public int EMINo { get; set; }
        public DateTime EMIDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}
