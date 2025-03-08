using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class ReadyForPuchVM
    {
        public long ID { get; set; }
        public string FullName { get; set; }
        public string FICode { get; set; }
        public string Creator { get; set; }
        public string Branch_code { get; set; }
        public string Group_code { get; set; }
        public float Loan_amount { get; set; }
        public string Date { get; set; }
    }
}
