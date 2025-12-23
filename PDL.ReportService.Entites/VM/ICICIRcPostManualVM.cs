using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class ICICIRcPostManualVM
    {
        public long InstRcvID { get; set; }
        public long IMEI { get; set; }
        public string CaseCode { get; set; }
        public int RcptNo { get; set; }
        public int InstRcvAmt { get; set; }
        public string InstRcvDateTimeUTC { get; set; }
        public string? Flag { get; set; }
        public DateTime? CreationDate { get; set; }
        public byte BatchNo { get; set; }
        public DateTime? BatchDate { get; set; }
        public string FoCode { get; set; }
        public string DataBaseName { get; set; }
        public string Creator { get; set; }
        public string CustName { get; set; }
        public string PartyCd { get; set; }
        public string PayFlag { get; set; }
        public string? SmsMobNo { get; set; }
        public int InterestAmt { get; set; }
        public string CollPoint { get; set; }
        public string PaymentMode { get; set; }
        public string? CollBranchCode { get; set; }
        public string? TxnId { get; set; }
        [IgnoreDataMemberAttribute]
        public string? UserId { get; set; }
        public DateTime? VDATE { get; set; }
        public string? CSOID { get; set; }
        public string? BankRRN { get; set; }
    }
}
