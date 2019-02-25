using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;

//for ReadonlyRepo

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class StateBackgroundCheckContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<StateBackgroundCheckContext> { }


    public class StateBackgroundCheckContext : DbContext {
        public StateBackgroundCheckContext(
            DbContextOptions<StateBackgroundCheckContext> options) : base(options) { }

        public DbQuery<StateBackgroundCheckView> StateBackgroundChecks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Query<StateBackgroundCheckView>()
                .ToView("StateBackgroundCheckView");
        }
    }

}
