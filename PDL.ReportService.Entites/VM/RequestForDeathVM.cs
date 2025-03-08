using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class RequestForDeathVM
    {
        public string? SmCode { get; set; }
        public string? Type { get; set; }
        public string? Query { get; set; }
        public List<IFormFile?> DeathFiles { get; set; }
    }
}
