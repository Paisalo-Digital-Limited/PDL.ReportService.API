using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class IciciExcelFileVM
    {
        public DateTime? Date { get; set; }          
        public string? BankRRN { get; set; }
        public string? MerchantTranId { get; set; }
        public string? PayerVA { get; set; }
        public string? PayerAccountType { get; set; }
        public decimal? PayerAmount { get; set; }
        public string? TerminalId { get; set; }
        public string? SubMerchantId { get; set; }
        public string? SeqNo { get; set; }
    }
}
