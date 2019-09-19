using System;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using EDennis.AspNetCore.Base.Testing;

namespace EDennis.Samples.Colors.InternalApi.Models {

    
    public class ColorDbContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<ColorDbContext> { }

    public class ColorHistoryDbContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<ColorHistoryDbContext> { }


    public class ColorHistoryDbContext : ColorDbContextBase {
        public ColorHistoryDbContext(DbContextOptions<ColorHistoryDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Color>()
                .ToTable<Color>("Color", "dbo_history");

            modelBuilder.Entity<Color>()
                .HasKey(e=> new { e.Id, e.SysStart });



            if (Database.IsInMemory()) {

                modelBuilder.Entity<Color>()
                    //.HasData(ColorDbContextDataFactory.ColorHistoryRecords);
                    .HasData(ColorHistoryDbContextDataFactory.ColorHistoryRecordsFromRetriever);
            }

        }

    }

    public class ColorDbContext : ColorDbContextBase {
        public ColorDbContext(DbContextOptions<ColorDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Color>()
                .ToTable<Color>("Color", "dbo")
                .HasKey(e => e.Id);
            

            if (Database.IsInMemory()) {

                modelBuilder.Entity<Color>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<Color>>();

                modelBuilder.Entity<Color>()
                    //.HasData(ColorDbContextDataFactory.dbo_ColorRecords);
                    .HasData(ColorDbContextDataFactory.ColorRecordsFromRetriever);
            }

        }

    }

    public abstract class ColorDbContextBase : DbContext {


        public ColorDbContextBase(DbContextOptions options)
            : base(options) { }

        public virtual DbSet<Color> Colors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Color>()
                .Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);


        }
    }
}
