using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class IciciQrTransactionVM
    {
        public int TotalCount { get; set; }
        public string TxnId { get; set; }
        public string VNo { get; set; }
        public string CustRef { get; set; }
        public decimal Amount { get; set; }
        public string TxnStatus { get; set; }
        public string PayerVpa { get; set; }
        public string PayeeVpa { get; set; }
        public DateTime? TxnDateTime { get; set; }
        public string VirtualVpa { get; set; }
        public string Fname { get; set; }
        public string BranchName { get; set; }
        public string Creator { get; set; }
        public string GroupCode { get; set; }
        public string FISMCODE { get; set; }
        public string QRSMCODE { get; set; }
        public DateTime? CreationDate { get; set; }
        public string AHEAD { get; set; }
        public string SmCodeStatus { get; set; }
        public string QrEntryFlag { get; set; }
        public string PaymentMode { get; set; }
        public bool? IsRcposted { get; set; }
    }
}
