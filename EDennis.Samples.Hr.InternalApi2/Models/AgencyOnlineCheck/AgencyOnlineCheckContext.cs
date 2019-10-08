using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;

//for WriteableRepo

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class AgencyOnlineCheckContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<AgencyOnlineCheckContext> { }


    public class AgencyOnlineCheckContext : DbContext {
        public AgencyOnlineCheckContext(
            DbContextOptions<AgencyOnlineCheckContext> options) : base(options) { }

        public DbSet<AgencyOnlineCheck> AgencyOnlineChecks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<AgencyOnlineCheck>()
                .ToTable("AgencyOnlineCheck")
                .HasKey(e => e.Id);

            modelBuilder.Entity<AgencyOnlineCheck>()
                .Property(e => e.DateCompleted)
                .HasColumnType("date");

            modelBuilder.Entity<AgencyOnlineCheck>()
                .Property(e => e.Status)
                .HasMaxLength(100);

            if (Database.IsInMemory()) {
                modelBuilder.Entity<AgencyOnlineCheck>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<AgencyOnlineCheck>>();

                modelBuilder.Entity<AgencyOnlineCheck>()
                    .HasData(AgencyOnlineCheckContextDataFactory.AgencyOnlineCheckRecordsFromRetriever);
            }
        }
    }

}
