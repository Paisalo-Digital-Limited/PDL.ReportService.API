using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Entites.VM.ReportVM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.BLL;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Repository.Repository
{
    public class AllReportsRepository : BaseBLL, IAllReportsService
    {
        private IConfiguration _configuration;

        public AllReportsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DataTable AllReportsList(AllReportParameterVM obj, string dbname, bool isLive)
        {
            using (AllReportsBLL bll = new AllReportsBLL(_configuration))
            {
                return bll.AllReportsList(obj, dbname, isLive);
            }
        }
        public DataTable RcPostReportsList(int CreatorID, string? VDate, string? VNO, string? FromDate, string? ToDate, int? PageSize, int? PageNumber, string dbname, bool isLive)
        {
            using (AllReportsBLL bll=new AllReportsBLL(_configuration))
            {
                return bll.RcPostReportsList(CreatorID, VDate, VNO, FromDate, ToDate, PageSize, PageNumber, dbname, isLive);
            }
        }
        public byte[] GenerateLedgerPdf(string SmCode, string dbname, bool isLive)
        {
            using (AllReportsBLL bll = new AllReportsBLL(_configuration))
            {
                var data = bll.GetLedgerData(SmCode, dbname, isLive);
                return bll.GenerateLedgerExcel(data.Rows, data.Header);
            }
        }
       public bool GetSmCode(string SmCode, string dbname, bool isLive)
        {
            using (AllReportsBLL bll=new AllReportsBLL(_configuration))
            {
               return bll.GetSmCode(SmCode,dbname,isLive);
            }
        }
        public DataTable GetICICIQrCallbackResponse(string? FromDate, string? ToDate, int? PageSize, int? PageNumber, string dbname, bool isLive)
        {
            using (AllReportsBLL bll = new AllReportsBLL(_configuration))
            {
                return bll.GetICICIQrCallbackResponse(FromDate, ToDate, PageSize, PageNumber, dbname, isLive);
            }
        }

        public async Task<byte[]> GenerateGeneralLedgerExcel(string SmCode, string dbname, bool isLive)
        {
            using (AllReportsBLL bll = new AllReportsBLL(_configuration))
            {
                return await bll.GenerateGeneralLedgerExcel(SmCode, dbname, isLive);
            }
        }

        public async Task<byte[]> GetTrailBalance(List<string> Ahead ,DateTime startdate,DateTime enddate, string dbname, bool isLive)
        {
            using (AllReportsBLL bll = new AllReportsBLL(_configuration))
            {
                return await bll.GetTrailBalance(Ahead, startdate, enddate, dbname, isLive);
            }
        }

        public async Task<List<RCdata>> GetAllAhead( string dbname, bool isLive)
        {
            using (AllReportsBLL bll = new AllReportsBLL(_configuration))
            {
                return await bll.GetAllAhead(dbname, isLive);
            }
        }


        
    }

}
