using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class HrContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<HrContext> { }

    public class HrHistoryContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<HrHistoryContext> { }


    public class HrContext : HrContextBase {

        public HrContext(DbContextOptions<HrContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .ToTable("Employee", "dbo")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Position>()
                .ToTable("Position", "dbo")
                .HasKey(e => e.Id);

            modelBuilder.Entity<EmployeePosition>()
                .ToTable("EmployeePosition", "dbo")
                .HasKey(e => new { e.EmployeeId, e.PositionId });



            modelBuilder.Entity<EmployeePosition>()
                .HasOne(e => e.Employee)
                .WithMany(r => r.EmployeePositions)
                .HasForeignKey(e => e.EmployeeId)
                .HasConstraintName("fk_EmployeePosition_Employee")
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<EmployeePosition>()
                .HasOne(e => e.Position)
                .WithMany(r => r.EmployeePositions)
                .HasForeignKey(e => e.PositionId)
                .HasConstraintName("fk_EmployeePosition_Position")
                .OnDelete(DeleteBehavior.Restrict);


            if (Database.IsInMemory()) {

                modelBuilder.Entity<Employee>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<Employee>>();
                modelBuilder.Entity<Position>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<Position>>();

                modelBuilder.Entity<Employee>()
                    .HasData(HrContextDataFactory.EmployeeRecordsFromRetriever);
                modelBuilder.Entity<Position>()
                    .HasData(HrContextDataFactory.PositionRecordsFromRetriever);
                modelBuilder.Entity<EmployeePosition>()
                    .HasData(HrContextDataFactory.EmployeePositionRecordsFromRetriever);
            }
        }
    }


    public class HrHistoryContext : HrContextBase {

        public HrHistoryContext(DbContextOptions<HrHistoryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .ToTable("Employee", "dbo_history")
                .HasKey(e => new { e.Id, e.SysStart });

            modelBuilder.Entity<Position>()
                .ToTable("Position", "dbo_history")
                .HasKey(e => new { e.Id, e.SysStart });

            modelBuilder.Entity<EmployeePosition>()
                .ToTable("EmployeePosition", "dbo_history")
                .HasKey(e => new { e.EmployeeId, e.PositionId, e.SysStart });

            foreach(var entityType in modelBuilder.Model.GetEntityTypes()) {
                foreach(var fk in entityType.GetForeignKeys()) {
                    entityType.RemoveForeignKey(
                        fk.Properties,
                        fk.PrincipalKey,
                        entityType);
                }
            }

            if (Database.IsInMemory()) {

                modelBuilder.Entity<Employee>()
                    .HasData(HrContextDataFactory.EmployeeHistoryRecordsFromRetriever);
                modelBuilder.Entity<Position>()
                    .HasData(HrContextDataFactory.PositionHistoryRecordsFromRetriever);
                modelBuilder.Entity<EmployeePosition>()
                    .HasData(HrContextDataFactory.EmployeePositionHistoryRecordsFromRetriever);
            }
        }
    }



    public abstract class HrContextBase : DbContext {

        public HrContextBase(DbContextOptions options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbQuery<ManagerPosition> ManagerPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {


            modelBuilder.Entity<Employee>()
                .Property(e => e.FirstName)
                .HasMaxLength(30);

            modelBuilder.Entity<Position>()
                .Property(e => e.Title)
                .HasMaxLength(60);


            modelBuilder.Query<ManagerPosition>()
                .ToQuery(() =>
                    from e in Employees
                    join ep in EmployeePositions
                       on e.Id equals ep.EmployeeId
                    join p in Positions
                       on ep.PositionId equals p.Id
                    select new ManagerPosition {
                        EmployeeFirstName = e.FirstName,
                        PositionTitle = p.Title
                    }
                );

        }
    }
}
