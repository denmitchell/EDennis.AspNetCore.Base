using System;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EDennis.Samples.Colors2Repo.Models
{

    public class ColorDbContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<ColorsDbContext>{ }


    public partial class ColorsDbContext : ResettableDbContext<ColorsDbContext>
    {

        public ColorsDbContext(DbContextOptionsProvider<ColorsDbContext> provider)
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
