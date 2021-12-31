using GoogleBooksAPI.Models.DbHelper;

namespace GoogleBooksAPI.Models.GoogleAPI
{
    public class Item : BaseEntity
    {
        public string Kind { get; set; }
        public string Id { get; set; }
        public string SelfLink { get; set; }
        public virtual VolumeInfo VolumeInfo { get; set; }

        #region commented - Not Stored Properties - Not Necessary to Project Domain
        //public Guid SearchGuid { get; set; }
        //public virtual SaleInfo SaleInfo { get; set; }
        //public virtual AccessInfo AccessInfo { get; set; }
        //public virtual SearchInfo SearchInfo { get; set; } 
        #endregion
    }
}