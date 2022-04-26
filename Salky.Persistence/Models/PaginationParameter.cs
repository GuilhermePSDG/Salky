using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Persistence.Models
{
    public class PaginationParameter : Pagination
    {
        public string? Query { get; set; } = string.Empty;
    }
}
