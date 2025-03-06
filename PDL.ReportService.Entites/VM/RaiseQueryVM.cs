using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class RaiseQueryVM
    {
        public int Fi_ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Query { get; set; }
        public string? ErrorImage { get; set; }
        public DateTime? Createdon { get; set; }
        public IFormFile? Imag { get; set; }
    }
}