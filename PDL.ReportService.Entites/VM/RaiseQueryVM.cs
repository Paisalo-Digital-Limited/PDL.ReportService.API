﻿using Microsoft.AspNetCore.Http;
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
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Query { get; set; }
        public string? Reply { get; set; }
        public string? ImagPath { get; set; }
        public DateTime? Createdon { get; set; }
        public IFormFile? Imag { get; set; }

    }
}