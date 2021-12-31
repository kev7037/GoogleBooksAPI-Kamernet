using GoogleBooksAPI.Models.DbHelper;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class RetailPrice : BaseEntity
    {
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public int AmountInMicros { get; set; }
    }
}
