using GoogleBooksAPI.Models.DbHelper;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class ReadingModes : BaseEntity
    {
        public bool Text { get; set; }
        public bool Image { get; set; }
    }
}