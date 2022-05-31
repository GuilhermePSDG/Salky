using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Salky.Domain.Models.GenericsModels
{
    public class Pagination
    {
        public Pagination()
        {

        }
        public Pagination(int currentPage, long totalCount, int pageSize)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            LastPage = (int)(totalCount > 0 && pageSize > 0 ? totalCount / pageSize + (totalCount % pageSize == 0 ? 0 : 1) : 1);
            CurrentPage = currentPage == 0 ? 1 : currentPage == -1 ? LastPage : currentPage;

        }

        public int CurrentPage { get; set; }
        public long TotalCount { get; set; }
        [NotMapped, JsonIgnore]
        private int _PageSize;
        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = value > 100 ? 100 : value;
        }

        public int LastPage { get; set; }
    }
}
