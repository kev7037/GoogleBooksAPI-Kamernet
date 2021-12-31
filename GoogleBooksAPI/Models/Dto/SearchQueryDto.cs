using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleBooksAPI.Models.Dto
{
    public class SearchQueryDto
    {
        public string Intitle { get; set; }
        public string Inauthor { get; set; }
        public string Inpublisher { get; set; }
        public string Indescription { get; set; }
        public int StartIndex { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 20;
    }
}