using GoogleBooksAPI.Models.DbHelper;
using System.Collections.Generic;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class Root : BaseEntity
    {
        public string Kind { get; set; }
        public int TotalItems { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
