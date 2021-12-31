using GoogleBooksAPI.Models.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleBooksAPI.Models.Dto
{
    public class VolumeInfoDto_dapper : BaseEntity
    {
        public string SmallThumbnail { get; set; }
        public string Title { get; set; }
        public string CategoryList { get; set; }
        public string AuthorList { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }

        public string Thumbnail { get; set; }
        public string PreviewLink { get; set; }
        public string InfoLink { get; set; }
        public int PageCount { get; set; }
        public float AverageRating { get; set; }
        public string Language { get; set; }
    }
}