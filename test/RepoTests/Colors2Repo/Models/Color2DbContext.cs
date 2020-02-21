using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;

namespace Colors2.Models
{

    public class ColorDbContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<Color2DbContext>{

        public ColorDbContextDesignTimeFactory() {
        }

        public override ConfigurationType ConfigurationType => ConfigurationType.PhysicalFiles;

    }


    public class Color2DbContext : DbContext, ISqlServerDbContext<Color2DbContext>
    {

        public Color2DbContext(DbContextOptions<Color2DbContext> options)
            : base(options) {
        }

        public virtual DbSet<Rgb> Rgb { get; set; }
        public virtual DbSet<Hsl> Hsl { get; set; }
        public StoredProcedureDefs<Color2DbContext> StoredProcedureDefs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.HasSequence<int>("seqRgb");
            modelBuilder.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            modelBuilder.Entity<Rgb>(e => {

                e.ToTable("Rgb");
                e.HasKey(e => e.Id)
                    .HasName("pkRgb");
                e.Property(e => e.Id)
                    .HasDefaultValueSql("NEXT VALUE FOR seqRgb")
                    .ValueGeneratedOnAdd();
                e.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                e.Property(e => e.SysUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                e.Property(e => e.SysStart)
                    .HasDefaultValueSql("(getdate())")
                    .ValueGeneratedOnAddOrUpdate();
                e.Property(e => e.SysEnd)
                    .HasDefaultValueSql("(CONVERT(datetime2, '9999-12-31 23:59:59.9999999'))")
                    .ValueGeneratedOnAddOrUpdate();


                if (Database.IsInMemory()) {

                    modelBuilder.Entity<Rgb>()
                        .Property(e => e.Id)
                        .HasValueGenerator<MaxPlusOneValueGenerator<Rgb>>();

                    modelBuilder.Entity<Rgb>()
                        .HasData(ColorsDbContextDataFactory.RgbRecords);
                }

            });


            modelBuilder.Entity<Hsl>(e => {
                e.HasNoKey().ToView("vwHsl");
            });

        }
    }
}
