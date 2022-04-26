using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Salky.Persistence.Models
{
    public class Pagination
    {
        public Pagination()
        {

        }
        public Pagination(int currentPage, int totalCount, int pageSize)
        {
            CurrentPage = currentPage;
            TotalCount = totalCount;
            PageSize = pageSize;
        }

        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        [NotMapped, JsonIgnore]
        private int _PageSize;
        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = value > 50 ? 50 : value;
        }
    }
}
