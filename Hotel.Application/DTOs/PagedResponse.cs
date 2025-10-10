using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs
{
    public class PagedResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long Total { get; set; }
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}
