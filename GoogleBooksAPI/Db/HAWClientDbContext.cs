using System.Data.Entity.ModelConfiguration.Conventions;
using GoogleBooksAPI.Models.SearchModels;
using GoogleBooksAPI.Models.GoogleAPI;
using System.Data.Entity;

namespace GoogleBooksAPI.Db
{
    public class HAWClientDbContext : DbContext
    {
        public HAWClientDbContext() : base("HAWClientConnectionString") { }

        public static HAWClientDbContext Create() => new HAWClientDbContext();

        public DbSet<SearchHistoryModel> SearchHistories { get; set; }
        public DbSet<VolumeInfo> VolumeInfos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<ImageLinks> ImageLinks { get; set; }
        public DbSet<SearchQueryResultModel> SearchQueryResults { get; set; }

        #region Commented - Google API Entities not necessary to Project domain
        //public DbSet<Root> Roots { get; set; }
        //public DbSet<Item> Items { get; set; }
        //public DbSet<SaleInfo> SaleInfos { get; set; }
        //public DbSet<AccessInfo> AccessInfos { get; set; }
        //public DbSet<SearchInfo> SearchInfos { get; set; }
        //public DbSet<IndustryIdentifier> IndustryIdentifiers { get; set; }
        //public DbSet<ReadingModes> ReadingModes { get; set; }
        //public DbSet<PanelizationSummary> PanelizationSummaries { get; set; }
        //public DbSet<ListPrice> ListPrices { get; set; }
        //public DbSet<RetailPrice> RetailPrices { get; set; }
        //public DbSet<Offer> Offers { get; set; }
        //public DbSet<Epub> Epubs { get; set; }
        //public DbSet<Pdf> Pdfs { get; set; } 
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
