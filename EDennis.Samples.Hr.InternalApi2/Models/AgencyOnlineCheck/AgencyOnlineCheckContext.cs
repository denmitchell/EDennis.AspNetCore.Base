using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class AgencyOnlineCheckContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<AgencyOnlineCheckContext> { }
    public class AgencyOnlineCheckHistoryContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<AgencyOnlineCheckHistoryContext> { }


    public class AgencyOnlineCheckContext : AgencyOnlineCheckContextBase {
        public AgencyOnlineCheckContext(
            DbContextOptions<AgencyOnlineCheckContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<AgencyOnlineCheck>()
                .ToTable("AgencyOnlineCheck")
                .HasKey(e => e.Id);

            if (Database.IsInMemory()) {
                modelBuilder.Entity<AgencyOnlineCheck>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<AgencyOnlineCheck>>();

                modelBuilder.Entity<AgencyOnlineCheck>()
                    .HasData(AgencyOnlineCheckContextDataFactory.AgencyOnlineCheckRecordsFromRetriever);
            }
        }
    }


    public class AgencyOnlineCheckHistoryContext : AgencyOnlineCheckContextBase {
        public AgencyOnlineCheckHistoryContext(
            DbContextOptions<AgencyOnlineCheckHistoryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<AgencyOnlineCheck>()
                .ToTable("AgencyOnlineCheck", "dbo_history")
                .HasKey(e => new { e.Id, e.SysStart });

            if (Database.IsInMemory()) {

                modelBuilder.Entity<AgencyOnlineCheck>()
                    .HasData(AgencyOnlineCheckHistoryContextDataFactory.AgencyOnlineCheckHistoryRecordsFromRetriever);
            }
        }
    }


    public abstract class AgencyOnlineCheckContextBase : DbContext {

        public AgencyOnlineCheckContextBase(DbContextOptions options) : base(options) { }

        public DbSet<AgencyOnlineCheck> AgencyOnlineChecks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {


            modelBuilder.Entity<AgencyOnlineCheck>()
                .Property(e => e.DateCompleted)
                .HasColumnType("date");

            modelBuilder.Entity<AgencyOnlineCheck>()
                .Property(e => e.Status)
                .HasMaxLength(100);

        }
    }
}
