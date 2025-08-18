using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class RcTransactionVM
    {
        public DateTime? VDate { get; set; }
        public string Code { get; set; }
        public string Ahead { get; set; }
        public decimal? Dr { get; set; }
        public decimal? Cr { get; set; }
    }
}