using System.Data.Entity.Migrations;

namespace GoogleBooksAPI.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<GoogleBooksAPI.Db.HAWClientDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GoogleBooksAPI.Db.HAWClientDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
