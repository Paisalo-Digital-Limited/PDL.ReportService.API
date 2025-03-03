using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class BranchWithCreator
    {
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
    }
}
