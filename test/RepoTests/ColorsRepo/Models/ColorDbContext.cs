using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.MigrationsExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System;

namespace Colors.Models {


    /// <summary>
    /// To Add Migration :
    /// with DefaultProject = ColorsRepo ...
    ///     PM > Add-Migration Initial -Context ColorDbContext -Project ColorsRepo -StartupProject ColorsRepo
    /// To Update Database:
    ///     PM > Update-Database -Context ColorDbContext -Project ColorsRepo -StartupProject ColorsRepo
    /// </summary>
    public class ColorDbContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<ColorDbContext> { }

    /// <summary>
    /// To Add Migration :
    /// with DefaultProject = ColorsRepo ...
    ///     PM > Add-Migration Initial -Context ColorHistoryDbContext -Project ColorsRepo -StartupProject ColorsRepo
    /// To Update Database:
    ///     PM > Update-Database -Context ColorHistoryDbContext -Project ColorsRepo -StartupProject ColorsRepo
    /// </summary>
    public class ColorHistoryDbContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<ColorHistoryDbContext> { }


    public class ColorHistoryDbContext : ResettableDbContext<ColorHistoryDbContext> {

        public ColorHistoryDbContext(DbContextOptionsProvider<ColorHistoryDbContext> options)
            : base(options) { }



        public virtual DbSet<ColorHistory> ColorHistories { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ColorHistory>(entity => {

                entity.ToTable("Color", "dbo_history");
                entity.HasKey(e => new { e.Id, e.SysStart });
                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                if (Database.IsInMemory())
                    entity.HasData(ColorHistoryDbContextDataFactory.ColorHistoryRecordsFromRetriever);
               
            });

        }

    }

    public class ColorDbContext : ResettableDbContext<ColorDbContext> {
        public ColorDbContext(DbContextOptionsProvider<ColorDbContext> provider)
            : base(provider) { }

        public virtual DbSet<Color> Colors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

            if (!optionsBuilder.IsConfigured) {

                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
                var config = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json", true)
                    .AddJsonFile($"appsettings.{env}.json", true)
                    .Build();

                DbContextSettings<ColorDbContext> settings = new DbContextSettings<ColorDbContext>();
                config.GetSection($"DbContexts:{typeof(ColorDbContext).Name}").Bind(settings);

                optionsBuilder.UseSqlServer(settings.ConnectionString)
                       .ReplaceService<IMigrationsSqlGenerator, MigrationsExtensionsSqlGenerator>();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasSequence<int>("seqColor");

            modelBuilder.Entity<Color>(entity => {

                entity.ToTable("Color", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                if (Database.IsInMemory()) {
                    entity.HasData(ColorDbContextDataFactory.ColorRecordsFromRetriever);
                    entity.Property(e => e.Id)
                        .HasValueGenerator<MaxPlusOneValueGenerator<Color>>();
                } else {
                    entity.Property(e => e.Id)
                        .HasDefaultValueSql("NEXT VALUE FOR seqColor");
                }

            });

        }

    }

}
