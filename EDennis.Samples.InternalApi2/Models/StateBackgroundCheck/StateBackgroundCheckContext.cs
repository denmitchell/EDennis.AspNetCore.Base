using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace EDennis.Samples.InternalApi2.Models {

    public class StateBackgroundCheckContextDesignTimeFactory :
        SqlTemporalContextDesignTimeFactory<StateBackgroundCheckContext> { }

    public class StateBackgroundCheckContext : DbContextBase {

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

            if (Database.IsInMemory()) {
                modelBuilder.Entity<StateBackgroundCheck>()
                    .Property(e => e.Id)
                    .HasValueGenerator<ResettableValueGenerator>();
            }

            modelBuilder.Entity<StateBackgroundCheck>()
                .Property(e => e.DateCompleted)
                .HasColumnType("date");

            modelBuilder.Entity<StateBackgroundCheck>()
                .Property(e => e.Status)
                .HasMaxLength(100);

            modelBuilder.Entity<StateBackgroundCheck>()
                .HasData(StateBackgroundCheckContextDataFactory.StateBackgroundCheckRecords);

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
