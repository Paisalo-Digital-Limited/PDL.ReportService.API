using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class CSOReportVM
    {
        public string Creator { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string CSOName { get; set; }
        public string UserName { get; set; }
    }
}
