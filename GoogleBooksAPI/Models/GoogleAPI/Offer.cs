using GoogleBooksAPI.Models.DbHelper;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class Offer : BaseEntity
    {
        public int FinskyOfferType { get; set; }
        public virtual ListPrice ListPrice { get; set; }
        public virtual RetailPrice RetailPrice { get; set; }
        public bool Giftable { get; set; }
    }
}
