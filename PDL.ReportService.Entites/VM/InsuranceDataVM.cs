using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class InsuranceDataVM
    {
        public string BranchName { get; set; }
        public DateTime? LoanAppliRecDt { get; set; }
        public string LoanAcctNo { get; set; }
        public double? LoanAmount { get; set; }
        public double? LoanTenure { get; set; }
        public string LoanType { get; set; }

        public string Title { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string PinCode { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }

        public string InsuredFirstName { get; set; }
        public string InsuredLastName { get; set; }
        public string InsuredDOB { get; set; }
        public string InsuredRelationShip { get; set; }

        public string Insured2FirstName { get; set; }
        public string Insured2LastName { get; set; }
        public string Insured2DOB { get; set; }
        public string Insured2RelationShip { get; set; }

        public string NomineeName { get; set; }
        public string NomineeDOB { get; set; }
        public string Relation { get; set; }

        public double? SumAssured { get; set; }
        public double? TransactionAmount { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionDetails_UTR { get; set; }
    }

}
