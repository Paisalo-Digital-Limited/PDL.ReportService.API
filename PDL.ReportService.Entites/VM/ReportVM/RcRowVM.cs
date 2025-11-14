using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class RcRowVM
    {
        public long Id { get; set; }
        public string VTYPE { get; set; }
        public string VNO { get; set; }
        public DateTime VDATE { get; set; }
        public string VDESC { get; set; }
        public string AHEAD { get; set; }
        public decimal DR { get; set; }
        public decimal CR { get; set; }
        public string LINKNO { get; set; }
        public string BOOK { get; set; }
        public string CODE { get; set; }
        public string SC { get; set; }
        public string PARTY_CD { get; set; } // party identifier
        public string REMARKS { get; set; }
        public string RCNO { get; set; }
        public string Location { get; set; }
    }
}
