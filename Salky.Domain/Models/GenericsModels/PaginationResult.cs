using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Domain.Models.GenericsModels
{


    public class PaginationResult<T> : Pagination
    {
        public List<T> Results { get; set; } = new();

        public PaginationResult() { }
        public PaginationResult(int currentPage, int pageSize, long totalCount) : base( currentPage, totalCount,pageSize) 
        { }

        public PaginationResult(int currentPage, int pageSize, long totalCount,List<T> results) : this(currentPage,pageSize,totalCount)
        {
            this.SetResults(results);
        }
        public void SetResults(List<T> results)
        {
            Results = results;
        }

        public static async Task<PaginationResult<T>> CreateNewAsync(IQueryable<T> query, int currentPage, int PageSize)
        {
            var paginationRes = new PaginationResult<T>(currentPage, PageSize, query.Count());
            var result = await query
                .Skip((paginationRes.CurrentPage - 1) * paginationRes.PageSize)
                .Take(paginationRes.PageSize)
                .ToListAsync();
            paginationRes.SetResults(result);
            return paginationRes;
        }

      

        public PaginationResult<F> CastTo<F>(Func<T, F> converter)
        {
            var result = new PaginationResult<F>(CurrentPage, PageSize, TotalCount);
            result.SetResults(Results.Select(x => converter(x)).ToList());
            return result;
        }



    }
}
