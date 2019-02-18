using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi2.Models {

    //AspNetCore.Base config
    public class StateBackgroundCheckContextDesignTimeFactory :
        SqlTemporalContextDesignTimeFactory<StateBackgroundCheckContext> { }

    public class StateBackgroundCheckContext : DbContext {

        public StateBackgroundCheckContext(DbContextOptions options) : base(options) { }

        public DbSet<StateBackgroundCheck> StateBackgroundChecks { get; set; }
        public DbQuery<StateBackgroundCheckView> StateBackgroundCheckViewRecords { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            #region StateBackgroundCheck

            modelBuilder.Entity<StateBackgroundCheck>()
                .ToTable("StateBackgroundCheck")
                .HasKey(e => e.Id);

            modelBuilder.Entity<StateBackgroundCheck>()
                .Property(e => e.Id)
                .UseSqlServerIdentityColumn();

            modelBuilder.Entity<StateBackgroundCheck>()
                .Property(e => e.DateCompleted)
                .HasColumnType("date");

            modelBuilder.Entity<StateBackgroundCheck>()
                .Property(e => e.Status)
                .HasMaxLength(100);


            //AspNetCore.Base config
            if (Database.IsInMemory()) {
                modelBuilder.Entity<StateBackgroundCheck>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<StateBackgroundCheck>>();

                modelBuilder.Entity<StateBackgroundCheck>()
                    .HasData(StateBackgroundCheckContextDataFactory.StateBackgroundCheckRecordsFromRetriever);
            }


            //defining query for FederalBackgroundCheckView
            modelBuilder.Query<StateBackgroundCheckView>()
                .ToQuery(
                    () => StateBackgroundChecks.Select(
                        rec =>
                            new StateBackgroundCheckView {
                                Id = rec.Id,
                                EmployeeId = rec.EmployeeId,
                                DateCompleted = rec.DateCompleted,
                                Status = rec.Status
                            }
                        ));

            #endregion

        }
    }
}
