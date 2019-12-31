using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;

namespace Colors.Models {


    public class ColorDbContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<ColorDbContext> { }

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
