using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Entites.VM.ReportVM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
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
                return reportsBLL.GetCaseHistoryBySmCodes(smCodes, dbName, isLive, PageNumber, PageSize);
            }
        }
        public List<CsoCollectionReportModelVM> GetCsoCollectionReport(DateTime fromDate, DateTime toDate, string csoCode, string dbtype, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetCsoCollectionReport(fromDate, toDate, csoCode, dbtype, dbName, isLive, PageNumber, PageSize);
            }
        }
        public List<CsoCollectionReportModelVM> GetCsoCollectionReportAllCases(DateTime fromDate, DateTime toDate, string dbtype, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetCsoCollectionReportAllCases(fromDate, toDate, dbtype, dbName, isLive, PageNumber, PageSize);
            }
        }
        public List<BBPSPaymentReportVM> GetBBPSPaymentReport(DateTime fromDate, DateTime toDate, string? smCode, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetBBPSPaymentReport(fromDate, toDate, smCode, dbName, isLive, PageNumber, PageSize);
            }
        }
        public List<EMIInformationVM> GetEMIInformation(string smCode, string dbtype, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetEMIInformation(smCode,dbtype, dbName, isLive, PageNumber, PageSize);
            }
        }
    }
}