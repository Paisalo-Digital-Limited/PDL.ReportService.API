using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class QRMandateReportsVM
    {
        public string SmCode { get; set; }
        public decimal amount { get; set; }
        public string txnId { get; set; }
        public string TxnStatus { get; set; }
        public DateTime? CreationDate { get; set; }
       
    }

    public class InstallementCollectionStatusVM
    {
        public string custname { get; set; }
        public string mobile { get; set; }
        public string LoanStatus { get; set; }
        public List<Emidata> emis { get; set; }
        public List<EmiCollectiondata> emiCollections { get; set; }
    }

    public class Emidata
    {
        public decimal AMT { get; set; }
        public DateTime? PVN_RCP_DT { get; set; }
    }

    public class EmiCollectiondata
    {
        public decimal CR { get; set; }
        public DateTime? VDATE { get; set; }
        public string? VNO { get; set; }
    }

}
