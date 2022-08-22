using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Domain.Models.GenericsModels
{


    public class PaginationResult<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<T> Results;

        public PaginationResult(IQueryable<T> source, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            Results = (source.Skip(PageIndex * PageSize).Take(PageSize)).ToList();
        }
        public PaginationResult()
        {

        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex + 1 < TotalPages);
            }
        }


        public PaginationResult<F> CastTo<F>(Func<T, F> converter)
        {
            var result = new PaginationResult<F>();
            result.PageIndex = PageIndex;
            result.PageSize = PageSize;
            result.TotalCount = TotalCount;
            result.TotalPages = TotalPages;
            result.Results = this.Results.Select(x => converter(x)).ToList();
            return result;
        }



    }
}
