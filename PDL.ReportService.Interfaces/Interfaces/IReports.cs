using Microsoft.AspNetCore.Http;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Entites.VM.ReportVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Interfaces.Interfaces
{
    public interface IReports
    {
        List<CaseHistoryVM> GetCaseHistoryBySmCodes(List<string> smCodes, string dbName, bool isLive, int PageNumber, int PageSize);
        List<CsoCollectionReportModelVM> GetCsoCollectionReport(DateTime fromDate, DateTime toDate, string csoCode, string dbtype, string dbName, bool isLive);
        List<CsoCollectionReportModelVM> GetCsoCollectionReportAllCases(DateTime fromDate, DateTime toDate,string dbtype, string dbName, bool isLive);
        List<BBPSPaymentReportVM> GetBBPSPaymentReport(DateTime fromDate, DateTime toDate, string? smCode, string dbName, bool isLive);
    }
}