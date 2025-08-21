using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class CrifWithFIVM
    {
        public string? voter_id;
        public string full_name;
        public string? emailid;
        public string? dob;
        public string? co;
        public string address;
        public string? city;
        public string? state;
        public string? pin;
        public string? loan_amount;
        public string mobile;

        public string? FiCode { get; set; }
        public string? Creator { get; set; }
        public int? CrifScore { get; set; }
        public int? OverdueAmt { get; set; }
        public int? Workoff { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }
        public double? CombinePayHistory { get; set; }
        public double? INDVInstallment { get; set; }
        public double? GSPInstallment { get; set; }
        public double? TotalCurrentAmt { get; set; }
        public string? ResponseDetails { get; set; }
        public string? ReqDetails { get; set; }
        public string? AadharID { get; set; }
        public string? pancard { get; set; }
        public string? Gender { get; set; }
        public string CreatedBy { get; set; }

        public string? ShowScoreValue { get; set; }
    }
}
