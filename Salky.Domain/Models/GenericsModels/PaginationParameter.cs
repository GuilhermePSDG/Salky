using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Domain.Models.GenericsModels
{
    public class PaginationParameter : Pagination
    {
        public string? Query { get; set; } = string.Empty;
    }
}
