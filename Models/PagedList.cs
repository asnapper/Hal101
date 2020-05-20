using System.Collections.Generic;

namespace Asnapper.Hal101.Models
{
    public class PagedList<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}