using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    //AspNetCore.Base config
    public class AgencyInvestigatorCheckContextDesignTimeFactory :
        SqlTemporalContextDesignTimeFactory<AgencyInvestigatorCheckContext> { }

    public class AgencyInvestigatorCheckContext : DbContext {

        public AgencyInvestigatorCheckContext(DbContextOptions options) : base(options) { }

        public DbSet<AgencyInvestigatorCheck> AgencyInvestigatorChecks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            #region AgencyInvestigatorCheck

            modelBuilder.Entity<AgencyInvestigatorCheck>()
                .ToTable("AgencyInvestigatorCheck")
                .HasKey(e => e.Id);

            modelBuilder.Entity<AgencyInvestigatorCheck>()
                .Property(e => e.Id)
                .UseSqlServerIdentityColumn();

            modelBuilder.Entity<AgencyInvestigatorCheck>()
                .Property(e => e.DateCompleted)
                .HasColumnType("date");

            modelBuilder.Entity<AgencyInvestigatorCheck>()
                .Property(e => e.Status)
                .HasMaxLength(100);

            modelBuilder.Entity<AgencyInvestigatorCheck>()
                .Property(e => e.SysStart)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(getdate())")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<AgencyInvestigatorCheck>()
                .Property(e => e.SysEnd)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(convert(datetime2,'9999-12-31 23:59:59.9999999'))")
                .ValueGeneratedOnAddOrUpdate();

            //AspNetCore.Base config
            if (Database.IsInMemory()) {
                modelBuilder.Entity<AgencyInvestigatorCheck>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<AgencyInvestigatorCheck>>();

                modelBuilder.Entity<AgencyInvestigatorCheck>()
                    .HasData(AgencyInvestigatorCheckContextDataFactory.AgencyInvestigatorCheckRecordsFromRetriever);
            }


            #endregion

        }
    }
}
