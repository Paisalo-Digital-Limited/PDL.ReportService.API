using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class BranchDashBoardVM
    {
        public Int64 Total_FirstEsign_Count { get; set; }
        public Int64 Total_Sanctioned_Count { get; set; }
        public Int64 Total_SecondEsign_Count { get; set; }
        public Int64 Total_Disbursed_Count { get; set; }
        public Int64 Total_Count { get; set; }
        public Int64 Total_PostSanction_Count { get; set; }
        public Int64 Total_ReadyForAudit_Count { get; set; }
        public Int64 Total_ReadyForNeft_Count { get; set; }
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
        public string? Bank_Ac { get; set; }
        public string? Bank_IFCS { get; set; }
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
    public class TotalDemandAndCollection
    {

        public decimal? TotalDemand { get; set; }
        public decimal? TotalCollection { get; set; }
        public decimal? AdvanceCollection { get; set; }
        public decimal? OD { get; set; }
        public decimal? TotalEfficiency { get; set; }
    }
    public class GetCollectionCountVM
    {

        public string CreatorName { get; set; }
        public Int64 FICode { get; set; }
        public string Branch_code { get; set; }
        public string SmCode { get; set; }
        public string FullName { get; set; }
        public decimal? CR { get; set; }
        public string? VNO { get; set; }
        public string? VDATE { get; set; }
    }
    public class GetDemandCountVM
    {

        public string CreatorName { get; set; }
        public Int64 FICode { get; set; }
        public string Branch_code { get; set; }
        public string SmCode { get; set; }
        public string FullName { get; set; }
        public decimal? INSTALL { get; set; }
        public decimal? AMT { get; set; }
        public string? PVN_RCP_DT { get; set; }
    }
}
