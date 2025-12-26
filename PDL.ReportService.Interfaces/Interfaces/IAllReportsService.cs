using PDL.ReportService.Entites.VM;
using PDL.ReportService.Entites.VM.ReportVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PDL.ReportService.Interfaces.Interfaces
{
    public interface IAllReportsService
    {
       DataTable AllReportsList(AllReportParameterVM obj,string dbname,bool isLive);
       DataTable RcPostReportsList(int CreatorID, string? VDate, string? VNO, string? FromDate, string? ToDate, int? PageSize, int? PageNumber,string dbname,bool isLive);
        byte[] GenerateLedgerPdf(string SmCode, string dbname, bool isLive);
        bool GetSmCode(string SmCode, string dbname, bool isLive);
        DataTable GetICICIQrCallbackResponse(string? FromDate, string? ToDate, int? PageSize, int? PageNumber, string dbname, bool isLive);
        Task<byte[]> GenerateGeneralLedgerExcel(string SmCode, string dbname, bool isLive);
        Task<byte[]> GetTrailBalance( List<string>Ahead, DateTime startdate,DateTime enddate, string dbname, bool isLive);
        Task<List<RCdata>> GetAllAhead(string dbname, bool isLive);
        List<ApplicationFormDataModel> GetAppFormData(int Fi_Id, string dbname, bool isLive);
        List<HouseVisitReportModel> GenerateHomeVisitReports(int Fi_Id, string dbname, bool isLive);
        List<SecondEsignVM> GetSecondEsignReportData(int Fi_Id, string dbname, bool isLive);
        DataTable GetNewCasesForAMonth(string? FromDate, string? ToDate, string dbname, bool isLive);
        DataTable GetAheadLeger(string? FromDate, string? ToDate,string Ahead, string dbname, bool isLive);
        List<CrifDataJLGVM> GetCrifDataJLG(string ReportDate, string? StartDate, string? EndDate, string dbname, bool isLive);
        PartyLedgerVMresponse PartyLedger(string SmCode,string dbname,bool isLive);
    }
}
