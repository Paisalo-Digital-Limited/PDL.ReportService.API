using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class CollectionStatusVM
    {
        public List<Emi> emis { get; set; }
        public List<EmiCollection> emiCollections { get; set; }
    }
    public class Emi
    {
        public decimal AMT { get; set; }
        public DateTime PVN_RCP_DT { get; set; }
    }
    public class EmiCollection
    {
        public decimal CR { get; set; }
        public DateTime VDATE { get; set; }

    }
}
