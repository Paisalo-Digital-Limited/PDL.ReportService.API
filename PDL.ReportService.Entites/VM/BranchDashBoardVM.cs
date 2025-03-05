using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class BranchDashBoardVM
    {
        public int Total_FirstEsign_Count { get; set; }
        public int Total_Sanctioned_Count { get; set; }
        public int Total_SecondEsign_Count { get; set; }
        public int Total_Disbursed_Count { get; set; }
        public int Total_Count { get; set; }
    }
    public class BranchDashBoardDataModel
    {
        public long Fi_Id { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string CreatorName { get; set; }
        public string FICode { get; set; }
        public string SmCode { get; set; }
        public string Current_City { get; set; }
        public string Group_code { get; set; }
        public decimal? Income { get; set; }
        public decimal? Expense { get; set; }
        public int? LoanDuration { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Approved { get; set; }
        public string SchCode { get; set; }
        public decimal? SanctionedAmt { get; set; }
        public DateTime? DtFin { get; set; }
        public decimal? Loan_amount { get; set; }
        public decimal? InstAmt { get; set; }
        public decimal? Invest { get; set; }
        public DateTime? DtPos { get; set; }
    }
    public class GetFirstEsign
    {
        public string BorrSignStatus { get; set; }
        public DateTime? Creation_Date { get; set; }
        
    }
    public class GetSecoundEsign
    {
        public string Download_One_Pager_Status { get; set; }
        public string Eligible_CSO_Id { get; set; }
        public string Esign_Applicable_Status { get; set; }
    }
}
