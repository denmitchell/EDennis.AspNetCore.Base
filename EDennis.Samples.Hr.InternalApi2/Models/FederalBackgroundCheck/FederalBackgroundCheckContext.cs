using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    //AspNetCore.Base config
    public class FederalBackgroundCheckContextDesignTimeFactory :
        SqlTemporalContextDesignTimeFactory<FederalBackgroundCheckContext> { }

    public class FederalBackgroundCheckContext : DbContext {

        public FederalBackgroundCheckContext(DbContextOptions options) : base(options) { }

        public DbQuery<FederalBackgroundCheckView> FederalBackgroundCheckViewRecords { get; set; }
        public DbSet<FederalBackgroundCheck> FederalBackgroundChecks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            #region FederalBackgroundCheck

            modelBuilder.Entity<FederalBackgroundCheck>()
                .ToTable("FederalBackgroundCheck")
                .HasKey(e=>e.Id);

            modelBuilder.Entity<FederalBackgroundCheck>()
                .Property(e => e.Id)
                .UseSqlServerIdentityColumn();

            modelBuilder.Entity<FederalBackgroundCheck>()
                .Property(e => e.DateCompleted)
                .HasColumnType("date");

            modelBuilder.Entity<FederalBackgroundCheck>()
                .Property(e => e.Status)
                .HasMaxLength(100);

            //AspNetCore.Base config
            if (Database.IsInMemory()) {
                modelBuilder.Entity<FederalBackgroundCheck>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<FederalBackgroundCheck>>();

                modelBuilder.Entity<FederalBackgroundCheck>()
                    .HasData(FederalBackgroundCheckContextDataFactory.FederalBackgroundCheckRecordsFromRetriever);
            }


            //defining query for FederalBackgroundCheckView
            modelBuilder.Query<FederalBackgroundCheckView>()
                .ToQuery(
                    () => FederalBackgroundChecks.Select(
                        rec => 
                            new FederalBackgroundCheckView {
                                Id = rec.Id,
                                EmployeeId = rec.EmployeeId,
                                DateCompleted = rec.DateCompleted,
                                Status = rec.Status
                            } 
                        ) );

            #endregion

        }
    }
}
