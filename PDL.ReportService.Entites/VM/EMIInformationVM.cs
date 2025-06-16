using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class EMIInformationVM
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? DOB { get; set; }
        public string UID { get; set; }
        public string PanCard { get; set; }
        public string VoterCard { get; set; }
        public string Address { get; set; }
        public string TotalTenureofCase { get; set; }
        public int TotalNoOfEMIDue { get; set; }
        public Int64 TotalAmountDue { get; set; }
        public int TotalNoofEMIReceive { get; set; }
        public Int64 TotalAmountReceived { get; set; }
        public Int64 OverdueAmount { get; set; }
        public Int64 CurrentBalance { get; set; }
        public DateTime? LastPaymentRcvdDate { get; set; }
        public Int64 LastRcvdAmt { get; set; }
        public string SMClosedOrNot { get; set; }
    }
}
