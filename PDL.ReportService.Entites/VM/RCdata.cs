using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class RCdata
    {
        
            public long Fi_ID { get; set; }          // Finance or File ID
            public string VNO { get; set; }         // Voucher Number
            public DateTime? VDATE { get; set; }    // Voucher Date
            public string VDESC { get; set; }       // Voucher Description / Particulars
            public string AHEAD { get; set; }       // Account Head
            public decimal CR { get; set; }         // Credit Amount
            public decimal DR { get; set; }         // Debit Amount
            public string CODE { get; set; }        // Account / Transaction Code
            public string REMARKS { get; set; }     // Remarks or comments
            public decimal InstllDR { get; set; }     // Remarks or comments
                // Remarks or comments
        

    }

    public class EMIdata
    {
        public DateTime INS_DUE_DT { get; set; }
        public int INSTALL { get; set; }
        public string CODE { get; set; }
        public long Fi_ID { get; set; }
        public decimal AMT { get; set; }
        public decimal INST_PRINC { get; set; }
        //public decimal Rate { get; set; }
        public decimal PrincipalAmount { get; set; }
    }

    public class PerEmiResult
    {
        public int EmiNo { get; set; }
        public DateTime DueDate { get; set; }
        public decimal EmiAmount { get; set; }
        public decimal PaidForThisEmi { get; set; }
        public decimal OverdueAmount { get; set; }
        public int OverdueDays { get; set; }
        public decimal CumulativeOverdue { get; set; }
    }

}
