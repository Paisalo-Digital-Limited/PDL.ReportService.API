using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class NOCQueryVM
    {
        public string Query { get; set; }
        public string SmCode { get; set; }
        public IFormFile Image { get; set; }
    }
}
