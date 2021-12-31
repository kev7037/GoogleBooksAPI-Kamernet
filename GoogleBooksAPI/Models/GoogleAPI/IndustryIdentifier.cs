using GoogleBooksAPI.Models.DbHelper;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class IndustryIdentifier : BaseEntity
    {
        public string Type { get; set; }
        public string Identifier { get; set; }
    }
}