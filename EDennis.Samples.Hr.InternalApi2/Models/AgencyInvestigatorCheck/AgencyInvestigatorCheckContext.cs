using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;


//for WriteableTemporalRepo

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class AgencyInvestigatorCheckContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<AgencyInvestigatorCheckContext> { }


    public class AgencyInvestigatorCheckHistoryContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<AgencyInvestigatorCheckHistoryContext> { }


    public class AgencyInvestigatorCheckContext : AgencyInvestigatorCheckContextBase {
        public AgencyInvestigatorCheckContext(
            DbContextOptions<AgencyInvestigatorCheckContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<AgencyInvestigatorCheck>()
                .ToTable("AgencyInvestigatorCheck")
                .HasKey(e => e.Id);

            if (Database.IsInMemory()) {
                modelBuilder.Entity<AgencyInvestigatorCheck>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<AgencyInvestigatorCheck>>();

                modelBuilder.Entity<AgencyInvestigatorCheck>()
                    .HasData(AgencyInvestigatorCheckContextDataFactory.AgencyInvestigatorCheckRecordsFromRetriever);
            }
        }
    }


    public class AgencyInvestigatorCheckHistoryContext : AgencyInvestigatorCheckContextBase {
        public AgencyInvestigatorCheckHistoryContext(
            DbContextOptions<AgencyInvestigatorCheckHistoryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<AgencyInvestigatorCheck>()
                .ToTable("AgencyInvestigatorCheck","dbo_history")
                .HasKey(e => new { e.Id, e.SysStart } );

            //not need in this project, but included for documentation purposes 
            modelBuilder.IgnoreNavigationProperties();


            if (Database.IsInMemory()) {

                modelBuilder.Entity<AgencyInvestigatorCheck>()
                    .HasData(AgencyInvestigatorCheckHistoryContextDataFactory.AgencyInvestigatorCheckHistoryRecordsFromRetriever);
            }
        }
    }


    public abstract class AgencyInvestigatorCheckContextBase : DbContext {

        public AgencyInvestigatorCheckContextBase(DbContextOptions options) : base(options) { }

        public DbSet<AgencyInvestigatorCheck> AgencyInvestigatorChecks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {


            modelBuilder.Entity<AgencyInvestigatorCheck>()
                .Property(e => e.DateCompleted)
                .HasColumnType("date");

            modelBuilder.Entity<AgencyInvestigatorCheck>()
                .Property(e => e.Status)
                .HasMaxLength(100);

        }
    }
}
