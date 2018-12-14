using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.InternalApi1.Models {

    public class HrContextDesignTimeFactory :
        SqlTemporalContextDesignTimeFactory<HrContext> { }


    public class HrContext : DbContextBase {

        public HrContext(DbContextOptions options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbQuery<ManagerPosition> ManagerPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            #region Employee

            modelBuilder.HasSequence<int>("seqEmployee")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<Employee>()
                .ToTable("Employee")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Id)
                .HasDefaultValueSql("next value for seqEmployee")
                .ValueGeneratedOnAdd();

            if (Database.IsInMemory()) {
                modelBuilder.Entity<Employee>()
                    .Property(e => e.Id)
                    .HasValueGenerator<ResettableValueGenerator>();
            }


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

            modelBuilder.Entity<Employee>()
                .HasData(HrContextDataFactory.EmployeeRecords);

            #endregion
            #region Position

            modelBuilder.HasSequence<int>("seqPosition")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<Position>()
                .ToTable("Position")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Position>()
                .Property(e => e.Id)
                .HasDefaultValueSql("next value for seqPosition")
                .ValueGeneratedOnAdd();

            if (Database.IsInMemory()) {
                modelBuilder.Entity<Position>()
                    .Property(e => e.Id)
                    .HasValueGenerator<ResettableValueGenerator>();
            }

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

            modelBuilder.Entity<Position>()
                .HasData(HrContextDataFactory.PositionRecords);

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

            modelBuilder.Entity<EmployeePosition>()
                .HasData(HrContextDataFactory.EmployeePositionRecords);

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
