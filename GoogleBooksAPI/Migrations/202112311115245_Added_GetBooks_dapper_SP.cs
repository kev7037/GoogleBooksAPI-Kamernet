namespace GoogleBooksAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_GetBooks_dapper_SP : DbMigration
    {
        public override void Up()
        {

            CreateStoredProcedure("stp_books_get",
                p => new
                {
                    pTitle = p.String(),
                    pAuthor = p.String(),
                    pPublisher = p.String(),
                    pDescription = p.String(),
                    pPageNumber = p.Int(),
                    pTotalRows = new System.Data.Entity.Migrations.Model.ParameterModel(System.Data.Entity.Core.Metadata.Edm.PrimitiveTypeKind.Int32)
                    {
                        Name = "pTotalRows",
                        IsOutParameter = true
                    }
                },
                body: @"

                    SELECT
					            @pTotalRows = COUNT(v.[Entity_Id])
		            FROM 
					            VolumeInfo v

		            WHERE		(@pTitle		IS NULL OR v.Title		    like N'%' + @pTitle		    + '%')
		            AND			(@pAuthor		IS NULL OR v.AuthorList     like N'%' + @pAuthor		+ '%')
		            AND			(@pDescription	IS NULL OR v.[Description]  like N'%' + @pDescription	+ '%')
		            AND			(@pPublisher	IS NULL OR v.Publisher	    Like N'%' + @pPublisher	    + '%')


                    SELECT
					            v.[Entity_Id], 
					            v.Title, 
					            v.Subtitle,
					            v.AuthorList, 
					            v.Publisher, 
					            v.PublishedDate, 
					            v.[Description],  
					            v.CategoryList,
					            v.[Language],
                                v.PreviewLink,
					            v.ImageLinks_Entity_Id AS ImageLinks,
					            i.[Thumbnail],
					            i.[SmallThumbnail]
		            FROM 
					            VolumeInfo v
		            LEFT JOIN	ImageLinks i on i.[Entity_Id] = v.ImageLinks_Entity_Id

		            WHERE		(@pTitle		IS NULL OR v.Title		    like N'%' + @pTitle		    + '%')
		            AND			(@pAuthor		IS NULL OR v.AuthorList     like N'%' + @pAuthor		+ '%')
		            AND			(@pDescription	IS NULL OR v.[Description]  like N'%' + @pDescription	+ '%')
		            AND			(@pPublisher	IS NULL OR v.Publisher	    Like N'%' + @pPublisher	    + '%')
                    
                    ORDER BY    v.Title 
	                OFFSET ((@pPageNumber - 1) * 40) ROWS FETCH NEXT 40 ROWS ONLY
                ");
        }

        public override void Down()
        {
            DropStoredProcedure("stp_books_get");
        }
    }
}
