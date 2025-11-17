using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class LedgerTransactionVM
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Particulars { get; set; }
        public string VoucherNo { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public int? Days { get; set; }
        public decimal Interest { get; set; }
        public decimal CumulativeInterest { get; set; }
        public decimal InvestAmount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string InstCode { get; set; }
        public int? InstallmentNumber { get; set; }
        public DateTime? InstallmentDueDate { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public decimal? InstallmentPrincipal { get; set; }
        public decimal? InstallmentInterest { get; set; }
        public decimal? InstallmentBalance { get; set; }
        public decimal? Installmentdebit { get; set; }
        public decimal? Installmentcredit { get; set; }
        public decimal? OverdueBalance { get; set; }
        public string InstallmentStatus { get; set; }
        public string BranchCode { get; set; }
        public string Remarks { get; set; }
    }
}
