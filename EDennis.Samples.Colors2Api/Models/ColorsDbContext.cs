using System;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EDennis.Samples.Colors2Api.Models
{

    public class ColorDbContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<ColorsDbContext>{ }


    public partial class ColorsDbContext : DbContext
    {

        public ColorsDbContext(DbContextOptions<ColorsDbContext> options)
            : base(options) {
        }

        public virtual DbSet<Rgb> Rgb { get; set; }
        public virtual DbQuery<Hsl> Hsl { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Rgb>(entity => {

                entity.ToTable("Rgb");

                entity.HasKey(e => e.Id)
                    .HasName("pkRgb");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SysUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                if (Database.IsInMemory()) {

                    modelBuilder.Entity<Rgb>()
                        .Property(e => e.Id)
                        .HasValueGenerator<MaxPlusOneValueGenerator<Rgb>>();

                    modelBuilder.Entity<Rgb>()
                        .HasData(ColorsDbContextDataFactory.dbo_RgbRecords);
                }


            });

            modelBuilder.Query<Hsl>(entity => {
                entity.ToView("vwHsl");
            });

        }
    }
}
