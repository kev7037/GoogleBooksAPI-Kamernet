using GoogleBooksAPI.Models.DbHelper;
using System;

namespace GoogleBooksAPI.Models.SearchModels
{
    public class SearchQueryResultModel : BaseEntity
    {
        public Guid SearchId { get; set; }
        public Guid VolumeInfoId { get; set; }
    }
}
