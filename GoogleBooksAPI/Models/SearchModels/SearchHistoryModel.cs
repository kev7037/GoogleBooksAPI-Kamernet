using GoogleBooksAPI.Models.DbHelper;
using GoogleBooksAPI.Models.GoogleAPI;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace GoogleBooksAPI.Models.SearchModels
{
    public class SearchHistoryModel
    {
        [Key]
        public Guid Entity_Id { get; set; }
        public string SearchQuery { get; set; }
        public DateTime ModifiedDateTime { get; set; } = DateTime.UtcNow;
        public List<VolumeInfo> VoloumeInfo { get; set; } = new List<VolumeInfo>();
    }
}
