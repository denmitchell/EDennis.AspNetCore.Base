using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    //AspNetCore.Base config
    public class HrContextDesignTimeFactory :
        SqlTemporalContextDesignTimeFactory<HrContext> { }

    public class HrContext : DbContext {

        public HrContext(DbContextOptions<HrContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbQuery<ManagerPosition> ManagerPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            #region Employee
            modelBuilder.Entity<Employee>()
                .ToTable("Employee")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Id)
                .UseSqlServerIdentityColumn();

            modelBuilder.Entity<Employee>()
                .Property(e => e.FirstName)
                .HasMaxLength(30);

            modelBuilder.Entity<Employee>()
                .Property(e => e.SysStart)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(getdate())")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Employee>()
                .Property(e => e.SysEnd)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(convert(datetime2,'9999-12-31 23:59:59.9999999'))")
                .ValueGeneratedOnAddOrUpdate();


            //AspNetCore.Base config
            if (Database.IsInMemory()) {

                modelBuilder.Entity<Employee>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<Employee>>();

                modelBuilder.Entity<Employee>()
                    .HasData(HrContextDataFactory.EmployeeRecordsFromRetriever);
            }

            #endregion
            #region Position

            modelBuilder.Entity<Position>()
                .ToTable("Position")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Position>()
                .Property(e => e.Id)
                .UseSqlServerIdentityColumn();

            modelBuilder.Entity<Position>()
                .Property(e => e.Title)
                .HasMaxLength(60);

            modelBuilder.Entity<Position>()
                .Property(e => e.SysStart)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(getdate())")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Position>()
                .Property(e => e.SysEnd)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(convert(datetime2,'9999-12-31 23:59:59.9999999'))")
                .ValueGeneratedOnAddOrUpdate();

            //AspNetCore.Base config
            if (Database.IsInMemory()) {

                modelBuilder.Entity<Position>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator<Position>>();

                modelBuilder.Entity<Position>()
                    .HasData(HrContextDataFactory.PositionRecordsFromRetriever);
            }

            #endregion
            #region EmployeePosition

            modelBuilder.Entity<EmployeePosition>()
                .ToTable("EmployeePosition")
                .HasKey(e => new { e.EmployeeId, e.PositionId });

            modelBuilder.Entity<EmployeePosition>()
                .Property(e => e.SysStart)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(getdate())")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<EmployeePosition>()
                .Property(e => e.SysEnd)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(convert(datetime2,'9999-12-31 23:59:59.9999999'))")
                .ValueGeneratedOnAddOrUpdate();

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


            //AspNetCore.Base config
            if (Database.IsInMemory()) {

                modelBuilder.Entity<EmployeePosition>()
                    .HasData(HrContextDataFactory.EmployeePositionRecordsFromRetriever);
            }

            #endregion
            #region ManagerPosition


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
            #endregion

        }
    }
}
