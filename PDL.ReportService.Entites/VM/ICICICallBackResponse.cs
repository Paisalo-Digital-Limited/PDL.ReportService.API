using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class ICICICallBackResponse
    {
        // Merchant / Transaction identifiers
        public string MerchantId { get; set; }
        public string SubMerchantId { get; set; }
        public string TerminalId { get; set; }
        public string BankRRN { get; set; }
        public string MerchantTranId { get; set; }

        // Payer details
        public string PayerName { get; set; }
        public string PayerMobile { get; set; }
        public string PayerVA { get; set; }
        public string PayerAccountType { get; set; }
        public decimal? PayerAmount { get; set; }

        // Transaction status & dates
        public string TxnStatus { get; set; }
        public DateTime? TxnInitDate { get; set; }          // yyyyMMddHHmmss
        public DateTime? TxnCompletionDate { get; set; }    // yyyyMMddHHmmss
        public DateTime? CreationDate { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        // Response info
        public string ResponseCode { get; set; }
        public string RespCodeDescription { get; set; }

        // UPI specific
        public string PayeeVPA { get; set; }
        public string? UPIVersion { get; set; }

        // Extra / Audit
        public string? SequenceNum { get; set; }
        public string? ResponseJSON { get; set; }
        public string? UMN { get; set; }
    }
}
