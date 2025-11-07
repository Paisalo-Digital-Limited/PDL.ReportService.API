using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class PaginationResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    public class PaginationRequest<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
        public int? MinOverdueDays { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public object Filters { get; set; } // dynamic filter object
    }
}
