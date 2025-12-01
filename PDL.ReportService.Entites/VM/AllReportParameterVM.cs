using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class AllReportParameterVM
    {
        public int Id { get; set; }
        public string? ReportName { get; set; }
        public string? DatabaseName { get; set; }
        public string? Creator { get; set; }
        public string? BranchCodeFrom { get; set; }
        public string? BranchCodeTo { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? SchemeType { get; set; }
        public string? FO1 { get; set; }
        public string? FO2 { get; set; }
        public string? ToCode { get; set; }
        public string? Area { get; set; }
        public string? Tag { get; set; }
        public string? FICode { get; set; }
        public string? Code { get; set; }
        public string? Ahead { get; set; }
        public string? Name { get; set; }
        public decimal? FinAmount { get; set; }
        public string? FromCode { get; set; }
        public string? Trench { get; set; }
        public string? MobNo { get; set; }
        public string? Endpoints { get; set; }
        public int? ModuleType { get; set; }
        public string? Mode { get; set; }
    }
}
