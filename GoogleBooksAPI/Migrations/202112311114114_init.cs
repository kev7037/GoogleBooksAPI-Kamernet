namespace GoogleBooksAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Author",
                c => new
                    {
                        Entity_Id = c.Guid(nullable: false, identity: true),
                        Authors = c.String(),
                    })
                .PrimaryKey(t => t.Entity_Id);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Entity_Id = c.Guid(nullable: false, identity: true),
                        Categories = c.String(),
                    })
                .PrimaryKey(t => t.Entity_Id);
            
            CreateTable(
                "dbo.ImageLinks",
                c => new
                    {
                        Entity_Id = c.Guid(nullable: false, identity: true),
                        SmallThumbnail = c.String(),
                        Thumbnail = c.String(),
                    })
                .PrimaryKey(t => t.Entity_Id);
            
            CreateTable(
                "dbo.SearchHistoryModel",
                c => new
                    {
                        Entity_Id = c.Guid(nullable: false),
                        SearchQuery = c.String(),
                        ModifiedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Entity_Id);
            
            CreateTable(
                "dbo.VolumeInfo",
                c => new
                    {
                        Entity_Id = c.Guid(nullable: false, identity: true),
                        Item_id = c.String(),
                        SearchGuid = c.Guid(nullable: false),
                        Title = c.String(),
                        AuthorList = c.String(),
                        Publisher = c.String(),
                        PublishedDate = c.String(),
                        Description = c.String(),
                        PageCount = c.Int(nullable: false),
                        PrintType = c.String(),
                        CategoryList = c.String(),
                        AverageRating = c.Single(nullable: false),
                        RatingsCount = c.Int(nullable: false),
                        MaturityRating = c.String(),
                        AllowAnonLogging = c.Boolean(nullable: false),
                        ContentVersion = c.String(),
                        Language = c.String(),
                        PreviewLink = c.String(),
                        InfoLink = c.String(),
                        CanonicalVolumeLink = c.String(),
                        Subtitle = c.String(),
                        ImageLinks_Entity_Id = c.Guid(),
                        ReadingModes_Entity_Id = c.Guid(),
                        SearchHistoryModel_Entity_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Entity_Id)
                .ForeignKey("dbo.ImageLinks", t => t.ImageLinks_Entity_Id)
                .ForeignKey("dbo.ReadingModes", t => t.ReadingModes_Entity_Id)
                .ForeignKey("dbo.SearchHistoryModel", t => t.SearchHistoryModel_Entity_Id)
                .Index(t => t.ImageLinks_Entity_Id)
                .Index(t => t.ReadingModes_Entity_Id)
                .Index(t => t.SearchHistoryModel_Entity_Id);
            
            CreateTable(
                "dbo.ReadingModes",
                c => new
                    {
                        Entity_Id = c.Guid(nullable: false, identity: true),
                        Text = c.Boolean(nullable: false),
                        Image = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Entity_Id);
            
            CreateTable(
                "dbo.SearchQueryResultModel",
                c => new
                    {
                        Entity_Id = c.Guid(nullable: false, identity: true),
                        SearchId = c.Guid(nullable: false),
                        VolumeInfoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Entity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VolumeInfo", "SearchHistoryModel_Entity_Id", "dbo.SearchHistoryModel");
            DropForeignKey("dbo.VolumeInfo", "ReadingModes_Entity_Id", "dbo.ReadingModes");
            DropForeignKey("dbo.VolumeInfo", "ImageLinks_Entity_Id", "dbo.ImageLinks");
            DropIndex("dbo.VolumeInfo", new[] { "SearchHistoryModel_Entity_Id" });
            DropIndex("dbo.VolumeInfo", new[] { "ReadingModes_Entity_Id" });
            DropIndex("dbo.VolumeInfo", new[] { "ImageLinks_Entity_Id" });
            DropTable("dbo.SearchQueryResultModel");
            DropTable("dbo.ReadingModes");
            DropTable("dbo.VolumeInfo");
            DropTable("dbo.SearchHistoryModel");
            DropTable("dbo.ImageLinks");
            DropTable("dbo.Category");
            DropTable("dbo.Author");
        }
    }
}
