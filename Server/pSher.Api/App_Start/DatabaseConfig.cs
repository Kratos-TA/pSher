namespace PSher.Api
{
    using System.Data.Entity;
    using Data;
    using PSher.Data.Migrations;

    public class DatabaseConfig
    {
        public static void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PSherDbContext, Configuration>());
            PSherDbContext.Create().Database.Initialize(true);
        }
    }
}