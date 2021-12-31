using GoogleBooksAPI.Models.DbHelper;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class AccessInfo : BaseEntity
    {
        public string Country { get; set; }
        public string Viewability { get; set; }
        public bool Embeddable { get; set; }
        public bool PublicDomain { get; set; }
        public string TextToSpeechPermission { get; set; }
        public virtual Epub Epub { get; set; }
        public virtual Pdf Pdf { get; set; }
        public string WebReaderLink { get; set; }
        public string AccessViewStatus { get; set; }
        public bool QuoteSharingAllowed { get; set; }
    }
}