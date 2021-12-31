using GoogleBooksAPI.Models.DbHelper;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class PanelizationSummary : BaseEntity
    {
        public bool ContainsEpubBubbles { get; set; }
        public bool ContainsImageBubbles { get; set; }
    }
}