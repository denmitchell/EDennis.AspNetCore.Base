using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;


namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class StateBackgroundCheckContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<StateBackgroundCheckContext> { }


    public class StateBackgroundCheckContext : DbContext {
        public StateBackgroundCheckContext(
            DbContextOptions<StateBackgroundCheckContext> options) : base(options) { }

        public DbSet<StateBackgroundCheck> StateBackgroundChecks { get; set; }
        public DbQuery<StateBackgroundCheckView> StateBackgroundCheckViewRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<StateBackgroundCheck>()
                .ToTable("StateBackgroundCheck")
                .HasKey(e => e.Id);

            modelBuilder.Entity<StateBackgroundCheck>()
                .Property(e => e.DateCompleted)
                .HasColumnType("date");

            modelBuilder.Entity<StateBackgroundCheck>()
                .Property(e => e.Status)
                .HasMaxLength(100);
        }
    }

}
