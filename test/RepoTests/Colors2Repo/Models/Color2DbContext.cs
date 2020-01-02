using System;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Colors2.Models
{

    public class ColorDbContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<Color2DbContext>{ }


    public partial class Color2DbContext : ResettableDbContext<Color2DbContext>
    {

        public Color2DbContext(DbContextOptionsProvider<Color2DbContext> provider)
            : base(provider) {
        }

        public virtual DbSet<Rgb> Rgb { get; set; }
        public virtual DbSet<Hsl> Hsl { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.HasSequence<int>("seqRgb");

            modelBuilder.Entity<Rgb>(entity => {

                entity.ToTable("Rgb");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("NEXT VALUE FOR seqRgb");

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
                        .HasData(ColorsDbContextDataFactory.RgbRecords);
                }


            });

            modelBuilder.Entity<Hsl>(entity => {
                entity
                .HasNoKey().ToView("vwHsl");
            });

        }
    }
}
