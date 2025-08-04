using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class CaseHistoryVM
    {
        public string Code { get; set; }              
        public string Custname { get; set; }          
        public string Religion { get; set; }
        public string PhoneNo { get; set; }
        public decimal? Income { get; set; }
        public int? CrifScore { get; set; }
        public decimal? OverdueAmt { get; set; }
        public decimal? TotalCurrentAmt { get; set; }
        public string Pincode { get; set; }
        public string Address { get; set; }          
        public string Creator { get; set; }          
        public decimal? IncomePA { get; set; }
        public int? ActiveAccount { get; set; }
        public decimal? ActiveAmount { get; set; }
        public long? FiCode { get; set; }
    }
}