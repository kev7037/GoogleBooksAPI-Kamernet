using GoogleBooksAPI.Models.DbHelper;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class ImageLinks : BaseEntity
    {
        public string SmallThumbnail { get; set; }
        public string Thumbnail { get; set; }
    }
}