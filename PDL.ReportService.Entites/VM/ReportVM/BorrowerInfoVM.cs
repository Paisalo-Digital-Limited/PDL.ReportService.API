using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class BorrowerInfoVM
    {
        public string FICode { get; set; }
        public string SMCode { get; set; }
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PAN { get; set; }
        public string BranchCode { get; set; }
        public string IFSC { get; set; }
        public string Status { get; set; }
    }
}