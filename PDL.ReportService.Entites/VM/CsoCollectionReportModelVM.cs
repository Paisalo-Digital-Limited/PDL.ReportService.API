using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class CsoCollectionReportModelVM
    {
        public string Creator { get; set; }
        public string VNO { get; set; }
        public DateTime VDATE { get; set; }
        public string DrCode { get; set; }
        public string CRCode { get; set; }
        public string Party_CD { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
        public string VDesc { get; set; }
        public string GroupCode { get; set; }
        public string BranchCode { get; set; }
        public string CollType { get; set; } // Always blank as per requirement
    }

}
