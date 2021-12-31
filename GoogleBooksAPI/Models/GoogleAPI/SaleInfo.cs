using GoogleBooksAPI.Models.DbHelper;
using System.Collections.Generic;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class SaleInfo : BaseEntity
    {
        public string Country { get; set; }
        public string Saleability { get; set; }
        public bool IsEbook { get; set; }
        public virtual ListPrice ListPrice { get; set; }
        public virtual RetailPrice RetailPrice { get; set; }
        public string BuyLink { get; set; }
        public virtual ICollection<Offer> Offers { get; set; }
    }
}
