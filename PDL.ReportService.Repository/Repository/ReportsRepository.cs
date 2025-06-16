using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM.ReportVM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Repository.Repository
{
    public class ReportsRepository : BaseBLL, IReports
    {
        private IConfiguration _configuration;

        public ReportsRepository(IConfiguration configuration)
        {
           _configuration = configuration;
        }

        public List<CaseHistoryVM> GetCaseHistoryBySmCodes(List<string> smCodes, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetCaseHistoryBySmCodes(smCodes, dbName, isLive, PageNumber, PageSize );
            }
        }
    }
}