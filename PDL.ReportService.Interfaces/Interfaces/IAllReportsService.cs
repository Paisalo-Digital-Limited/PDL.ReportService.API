using PDL.ReportService.Entites.VM;
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
    }
}
