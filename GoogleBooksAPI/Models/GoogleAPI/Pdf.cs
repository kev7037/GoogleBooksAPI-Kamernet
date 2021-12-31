using GoogleBooksAPI.Models.DbHelper;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class Pdf : BaseEntity
    {
        public bool IsAvailable { get; set; }
        public string AcsTokenLink { get; set; }
    }
}