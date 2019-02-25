using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;

//for ReadonlyTemporalRepo

namespace EDennis.Samples.Hr.InternalApi2.Models {

    public class FederalBackgroundCheckContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<FederalBackgroundCheckContext> { }

    public class FederalBackgroundCheckHistoryContextDesignTimeFactory :
        MigrationsExtensionsDbContextDesignTimeFactory<FederalBackgroundCheckHistoryContext> { }


    public class FederalBackgroundCheckContext : FederalBackgroundCheckContextBase {
        public FederalBackgroundCheckContext(
            DbContextOptions<FederalBackgroundCheckContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Query<FederalBackgroundCheckView>()
                .ToView("FederalBackgroundCheckView");
        }
    }


    public class FederalBackgroundCheckHistoryContext : FederalBackgroundCheckContextBase {
        public FederalBackgroundCheckHistoryContext(
            DbContextOptions<FederalBackgroundCheckHistoryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Query<FederalBackgroundCheckView>()
                .ToView("FederalBackgroundCheckHistoryView");
        }
    }


    public abstract class FederalBackgroundCheckContextBase : DbContext {

        public FederalBackgroundCheckContextBase(DbContextOptions options)
            : base(options) { }

        public DbQuery<FederalBackgroundCheckView> FederalBackgroundChecks { get; set; }
    }
}
