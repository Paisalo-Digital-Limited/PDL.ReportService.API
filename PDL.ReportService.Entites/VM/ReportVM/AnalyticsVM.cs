using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class AnalyticsVM
    {
        public string SubjectId { get; set; }
        public string Verification { get; set; }
        public string SoleTrader { get; set; }
        public string TransactionScore { get; set; }
        public string ScoreArea { get; set; }
        public string ScoreTranche { get; set; }
        public string MonthlyAnalysisJson { get; set; }
        public string PeriodAnalysisJson { get; set; }
        public string InsightsJson { get; set; }
    }
}
