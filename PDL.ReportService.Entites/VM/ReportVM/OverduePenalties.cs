using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class OverduePenalties
    {
        public int Id { get; set; }
        public long FICode { get; set; }
        public string SMCode { get; set; }
        public string CreatorName { get; set; }
        public string FullName { get; set; }
        public string BranchName { get; set; }
        public string GroupName { get; set; }
        public int FI_Id { get; set; }
        public DateTime EMIDate { get; set; }
        public int OverDueDays { get; set; }
        public decimal Rate { get; set; }
        public decimal OverDuePrincipal { get; set; }
        public decimal TotalOverDueAmount { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int IsActive { get; set; }
        
    }
}
