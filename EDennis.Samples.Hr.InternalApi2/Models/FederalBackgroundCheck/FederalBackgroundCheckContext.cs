using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;


namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class FederalBackgroundCheckContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<FederalBackgroundCheckContext> { }


    public class FederalBackgroundCheckContext : DbContext {
        public FederalBackgroundCheckContext(
            DbContextOptions<FederalBackgroundCheckContext> options) : base(options) { }

        public DbSet<FederalBackgroundCheck> FederalBackgroundChecks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<FederalBackgroundCheck>()
                .ToTable("FederalBackgroundCheck")
                .HasKey(e => e.Id);

            modelBuilder.Entity<FederalBackgroundCheck>()
                .Property(e => e.DateCompleted)
                .HasColumnType("date");

            modelBuilder.Entity<FederalBackgroundCheck>()
                .Property(e => e.Status)
                .HasMaxLength(100);
        }
    }

}
