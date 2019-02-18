using System;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using EDennis.AspNetCore.Base.Testing;

namespace EDennis.Samples.Colors.InternalApi.Models {

    
    public class ColorDbContextDesignTimeFactory :
        SqlTemporalContextDesignTimeFactory<ColorDbContext> { }


    public partial class ColorDbContext : DbContext {

        //AspNetCore.Base config
        public ColorDbContext(DbContextOptions<ColorDbContext> options)
            : base(options) { }

        public virtual DbSet<Color> Colors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Color>(entity => {
                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Color>().Property(e => e.SysStart)
                .HasDefaultValueSql("(getdate())")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Color>().Property(e => e.SysEnd)
                .HasDefaultValueSql("(CONVERT(datetime2, '9999-12-31 23:59:59.9999999'))")
                .ValueGeneratedOnAddOrUpdate();

            //AspNetCore.Base config
            if (Database.IsInMemory()) {

                modelBuilder.Entity<Color>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<Color>>();

                modelBuilder.Entity<Color>()
                    //.HasData(ColorDbContextDataFactory.ColorRecords);
                    .HasData(ColorDbContextDataFactory.ColorRecordsFromRetriever);
            }

        }
    }
}
