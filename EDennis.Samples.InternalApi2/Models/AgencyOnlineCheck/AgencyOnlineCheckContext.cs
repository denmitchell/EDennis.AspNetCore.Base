using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EDennis.Samples.InternalApi2.Models {

    public class AgencyOnlineCheckContextDesignTimeFactory :
        SqlTemporalContextDesignTimeFactory<AgencyOnlineCheckContext> { }

    public class AgencyOnlineCheckContext : DbContextBase {

        public AgencyOnlineCheckContext(DbContextOptions options) : base(options) { }

        public DbSet<AgencyOnlineCheck> AgencyOnlineChecks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            #region AgencyOnlineCheck

            modelBuilder.Entity<AgencyOnlineCheck>()
                .ToTable("AgencyOnlineCheck")
                .HasKey(e => e.Id);

            modelBuilder.Entity<AgencyOnlineCheck>()
                .Property(e => e.Id)
                .UseSqlServerIdentityColumn();

            if (Database.IsInMemory()) {
                modelBuilder.Entity<AgencyOnlineCheck>()
                    .Property(e => e.Id)
                    .HasValueGenerator<ResettableValueGenerator>();
            }

            modelBuilder.Entity<AgencyOnlineCheck>()
                .Property(e => e.DateCompleted)
                .HasColumnType("date");

            modelBuilder.Entity<AgencyOnlineCheck>()
                .Property(e => e.Status)
                .HasMaxLength(100);

            modelBuilder.Entity<AgencyOnlineCheck>()
                .Property(e => e.SysStart)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(getdate())")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<AgencyOnlineCheck>()
                .Property(e => e.SysEnd)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(convert(datetime2,'9999-12-31 23:59:59.9999999'))")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<AgencyOnlineCheck>()
                .HasData(AgencyOnlineCheckContextDataFactory.AgencyOnlineCheckRecords);

            #endregion

        }

    }
}
