using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Shared
{
    public class PaginatedList<T> : Result<T>
    {
        public PaginatedList()
        {
            
        }
        public new List<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public PaginatedList(List<T> data) => Data = data;
        public PaginatedList(
            bool succeeded,
            List<T> data = default!,
            string? messages = null,
            int count = 0,
            int pageNumber = 1,
            int pageSize = 10)
        {
            Data = data;
            CurrentPage = pageNumber;
            IsSuccess = succeeded;
            Messages = messages!;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }
      
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;


        public static PaginatedList<T> Create(List<T> data, int count, int pageNumber, int pageSize)
        {
            return new PaginatedList<T>(true, data, null, count, pageNumber, pageSize);
        }

    }
}
