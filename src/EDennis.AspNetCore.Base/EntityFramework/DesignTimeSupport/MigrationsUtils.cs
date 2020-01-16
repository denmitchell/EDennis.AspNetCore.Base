using EDennis.MigrationsExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public static class MigrationsUtils {

        public static void UpdateDatabase<TContext>(bool dropFirst)
            where TContext : DbContext {

            using var db = GetDbContext<TContext>();
            Console.WriteLine($"Updating database for {typeof(TContext).Name} ...");
            if (dropFirst)
                db.Database.EnsureDeleted();
            db.Database.Migrate();
        }


//        public static void AddMigrations<TContext>(string migrationName, string projectNamespace, string destinationDir)
//            where TContext : DbContext {

//            Console.WriteLine($"Adding migration {migrationName} for {projectNamespace} ...");

//            using var db = GetDbContext<TContext>();
//            IServiceCollection designTimeServiceCollection = new ServiceCollection();
//            designTimeServiceCollection.AddEntityFrameworkDesignTimeServices();
//            designTimeServiceCollection.AddDbContextDesignTimeServices(db);

//#pragma warning disable EF1001 // Internal EF Core API usage.
//            new SqlServerDesignTimeServices().ConfigureDesignTimeServices(designTimeServiceCollection);
//#pragma warning restore EF1001 // Internal EF Core API usage.
//            var designTimeServices = designTimeServiceCollection.BuildServiceProvider();

//            var scaffolder = designTimeServices.GetRequiredService<IMigrationsScaffolder>();
//            var migration = scaffolder.ScaffoldMigration(migrationName, projectNamespace);

//            if (!Directory.Exists(destinationDir))
//                Directory.CreateDirectory(destinationDir);

//            destinationDir += typeof(TContext).Name + "/";

//            if (!Directory.Exists(destinationDir))
//                Directory.CreateDirectory(destinationDir);

//            File.WriteAllText(
//                destinationDir + migration.MigrationId + migration.FileExtension,
//                migration.MigrationCode);
//            File.WriteAllText(
//                destinationDir + migration.MigrationId + ".Designer" + migration.FileExtension,
//                migration.MetadataCode);
//            File.WriteAllText(
//                destinationDir + migration.SnapshotName + migration.FileExtension,
//                migration.SnapshotCode);
//        }

        public static TContext GetDbContext<TContext>()
            where TContext : DbContext {

            //note: if the same driver project is being used with multiple repo projects, each
            //with their own appsettings files, ensure that the files are set to Copy Always,
            //rather than Copy If Newer.
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true)
                .AddJsonFile($"appsettings.{env}.json", true)
                .Build();

            DbContextSettings<TContext> settings = new DbContextSettings<TContext>();
            config.GetSection($"DbContexts:{typeof(TContext).Name}").Bind(settings);

            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseSqlServer(settings.ConnectionString)
                   .ReplaceService<IMigrationsSqlGenerator, MigrationsExtensionsSqlGenerator>();

            return (TContext)Activator.CreateInstance(typeof(TContext), new object[] {builder.Options });
        }



        public static string GetMigrationName() {
            char[] charMap = new char[] {
                '0','1','2','3','4','5','6','7','8','9',
                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                'n','o','p','q','r','s','t','u','v','w','x','y','z'
            };
            var now = DateTime.Now;
            var sb = new StringBuilder();
            sb.Append(charMap[now.Year / 100]);
            sb.Append(charMap[now.Year % 100]);
            sb.Append(charMap[now.Month]);
            sb.Append(charMap[now.Day]);
            sb.Append(charMap[now.Hour]);
            sb.Append(charMap[now.Minute]);
            sb.Append(charMap[now.Second]);
            return sb.ToString();
        }

    }
}
