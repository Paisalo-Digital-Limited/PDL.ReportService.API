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
        CaseHistoryResponse GetCaseHistoryBySmCodes(List<string> smCodes, string dbName, bool isLive, int PageNumber, int PageSize);
        List<CsoCollectionReportModelVM> GetCsoCollectionReport(DateTime fromDate, DateTime toDate, string csoCode, string dbtype, string dbName, bool isLive, int PageNumber, int PageSize);
        List<CsoCollectionReportModelVM> GetCsoCollectionReportAllCases(DateTime fromDate, DateTime toDate,string dbtype, string dbName, bool isLive, int PageNumber, int PageSize);
        List<BBPSPaymentReportVM> GetBBPSPaymentReport(DateTime fromDate, DateTime toDate, string? smCode, string dbName, bool isLive, int PageNumber, int PageSize);
        List<SmWithoutChqVM> GetLoansWithoutInstallments(string dDbName, string dbName, bool isLive, int PageNumber, int PageSize, out int totalCount);
        List<EMIInformationVM> GetEMIInformation(string smCode, string dbtype, string dbName, bool isLive, int PageNumber, int PageSize);
        List<LoanWithoutDisbVoucherVM> GetLoansWithoutDisbursements(string dDbName, string dbName, bool isLive, int PageNumber, int PageSize, out int TotalCount);
        List<DuplicateDIBVoucherVM> GetDuplicateDIBVouchers(string dbtype, string dbName, bool isLive, int PageNumber, int PageSize);
        List<RcTransactionVM> GetRcDisbursementTransactionReport(string dDbName, string dbName, bool isLive, int PageNumber, int PageSize, out int TotalCount, DateTime fromDate, DateTime toDate, string creator);
        List<CSOReportVM> GetCSOReport(int creatorId, string branchCode, string dbName, bool isLive, int pageNumber, int pageSize);
        List<LedgerReportVM> GetLedgerReport(string smCode, string dbName, bool isLive, int pageNumber, int pageSize);
        Task<data> GetAccountAggregatorReportAsync(long? fiCode, int? creatorId, string? smCode, string activeUser, bool isLive, string dbName);
        dynamic SMCodeValidation(SMCodeValidationVM file, string dbname, bool isLive);
        Task<PaginationResponse<OverduePenalties>> GetOverdueRecordsAsync(PaginationRequest<OverduePenalties> paginationRequest, string dbname, bool isLive);
        Task<List<OverduePenalties>> ExportOverdueExcel(string creatorId, string branchCode, string groupCode, string startDate, string endDate, string dbname, bool isLive);
        List<CibilDataVM> GetCibilReport(string searchDate, string dbName, bool isLive);
        List<InsuranceDataVM> GetInsuranceReport(string fromDate, string toDate,string dbName, bool isLive);
    }
}