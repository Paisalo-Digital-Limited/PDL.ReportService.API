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

        public CaseHistoryResponse GetCaseHistoryBySmCodes(List<string> smCodes, string dbName, bool isLive, int PageNumber, int PageSize)
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

        public List<SmWithoutChqVM> GetLoansWithoutInstallments(string dDbName, string dbName, bool isLive, int PageNumber, int PageSize, out int totalCount)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetLoansWithoutInstallments(dDbName, dbName, isLive, PageNumber, PageSize, out totalCount);
            }
        }

        public List<EMIInformationVM> GetEMIInformation(string smCode, string dbtype, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetEMIInformation(smCode, dbtype, dbName, isLive, PageNumber, PageSize);

            }
        }

        public List<LoanWithoutDisbVoucherVM> GetLoansWithoutDisbursements(string dDbName, string dbName, bool isLive, int PageNumber, int PageSize, out int TotalCount)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetLoansWithoutDisbursements(dDbName, dbName, isLive, PageNumber, PageSize, out TotalCount);
            }
        }
        public List<DuplicateDIBVoucherVM> GetDuplicateDIBVouchers(string dbtype, string dbName, bool isLive, int PageNumber, int PageSize)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetDuplicateDIBVouchers(dbtype, dbName, isLive, PageNumber, PageSize);
            }
        }
        public List<RcTransactionVM> GetRcDisbursementTransactionReport(string dDbName, string dbName, bool isLive, int PageNumber, int PageSize, out int TotalCount, DateTime fromDate, DateTime toDate, string creator)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetRcDisbursementTransactionReport(dDbName, dbName, isLive, PageNumber, PageSize, out TotalCount, fromDate, toDate, creator);
            }
        }
        public List<CSOReportVM> GetCSOReport(int creatorId, string branchCode, string dbName, bool isLive, int pageNumber, int pageSize)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetCSOReport(creatorId, branchCode, dbName, isLive, pageNumber, pageSize);
            }
        }
        public List<LedgerReportVM> GetLedgerReport(string smCode,string dbName, bool isLive, int pageNumber, int pageSize)
        {
            using (ReportsBLL reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.GetLedgerReport(smCode,dbName, isLive, pageNumber, pageSize);
            }
        }
        public async Task<data> GetAccountAggregatorReportAsync(long? fiCode, int? creatorId, string? smCode, string activeUser, bool isLive, string dbName)
        {
            using (var reportsBLL = new ReportsBLL(_configuration))
            {
                return await reportsBLL.GetAccountAggregatorReportAsync(fiCode, creatorId, smCode, activeUser, isLive, dbName);
            }
        }
        public dynamic SMCodeValidation(SMCodeValidationVM file, string dbname, bool isLive)
        {
            using (var reportsBLL = new ReportsBLL(_configuration))
            {
                return reportsBLL.SMCodeValidation(file, dbname, isLive);
            }
        }


        public async Task<PaginationResponse<OverduePenalties>> GetOverdueRecordsAsync(PaginationRequest<OverduePenalties> paginationRequest, string dbname, bool isLive)
        {
            using (var reportsBLL = new ReportsBLL(_configuration))
            {
                return await reportsBLL.GetOverdueRecordsAsync(paginationRequest,  dbname,  isLive);
            }
        }

        public async Task<List<OverduePenalties>> ExportOverdueExcel(string creatorId, string branchCode, string groupCode, string startDate, string endDate, string dbname, bool isLive)
        {
            using (var reportsBLL = new ReportsBLL(_configuration))
            {
                return await reportsBLL.ExportOverdueExcel( creatorId,  branchCode,  groupCode,  startDate,  endDate, dbname, isLive);
            }
        }

        public async Task<List<QRMandateReportsVM>> GetQRMandateReportsAsync(string SmCode, string mode, DateTime fromDate, DateTime toDate, string dbName, bool isLive)
        {
            using (AllReportsBLL bll = new AllReportsBLL(_configuration))
            {
                return await bll.GetQRMandateReportsAsync(SmCode, mode, fromDate, toDate, dbName, isLive);
            }
        }

        public async Task<InstallementCollectionStatusVM> GetInstallmentCollectionReportsAsync(string SmCode, string dbName, bool isLive)
        {
            using (AllReportsBLL bll = new AllReportsBLL(_configuration))
            {
                return await bll.GetInstallmentCollectionReportsAsync(SmCode, dbName, isLive);
            }
        }
    }
}