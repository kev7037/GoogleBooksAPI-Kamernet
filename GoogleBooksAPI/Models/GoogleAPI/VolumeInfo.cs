using GoogleBooksAPI.Models.DbHelper;
using GoogleBooksAPI.Models.SearchModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class VolumeInfo : BaseEntity
    {
        public string Item_id { get; set; }
        public Guid SearchGuid { get; set; }

        public string Title { get; set; }

        [NotMapped]
        public List<string> Authors { get; set; }
        public string AuthorList { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public virtual ReadingModes ReadingModes { get; set; }
        public int PageCount { get; set; }
        public string PrintType { get; set; }
        [NotMapped]
        public List<string> Categories { get; set; }
        public string CategoryList { get; set; }
        public float AverageRating { get; set; }
        public int RatingsCount { get; set; }
        public string MaturityRating { get; set; }
        public bool AllowAnonLogging { get; set; }
        public string ContentVersion { get; set; }
        public virtual ImageLinks ImageLinks { get; set; }
        public string Language { get; set; }
        public string PreviewLink { get; set; }
        public string InfoLink { get; set; }
        public string CanonicalVolumeLink { get; set; }
        public string Subtitle { get; set; }

        #region commented - Not Stored Properties - Not Necessary to Project Domain
        //public virtual ICollection<IndustryIdentifier> IndustryIdentifiers { get; set; }
        //public virtual PanelizationSummary PanelizationSummary { get; set; } 
        #endregion
    }
}
