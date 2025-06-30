using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class AccountAggregatorReportVM
    {
        public List<BorrowerInfoVM> BorrowerInfo { get; set; }
        public List<TransactionsVM> Transactions { get; set; }
        public AnalyticsVM Analytics { get; set; }
    }
}