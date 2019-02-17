using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;

namespace EDennis.AspNetCore.Base.Security {

    public class IdentityServerContextDesignTimeFactory
        : SqlTemporalContextDesignTimeFactory<IdentityServerContext> { }

    /// <summary>
    /// This class is designed to support IdentityServerMockDataLoader.
    /// *** Special Note: this class is not yet tested.
    /// </summary>
    public class IdentityServerContext : DbContext {
        public IdentityServerContext(DbContextOptions options) :
            base(options) {
        }

        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            var modelTypes = new Type[] {
                typeof(ApiResource), typeof(ApiResourceClaim), typeof(ApiResourceProperty), typeof(ApiScope),
                typeof(ApiScopeClaim), typeof(ApiSecret), typeof(Client), typeof(ClientClaim),
                typeof(ClientCorsOrigin), typeof(ClientGrantType), typeof(ClientIdPRestriction),
                typeof(ClientPostLogoutRedirectUri), typeof(ClientProperty), typeof(ClientRedirectUri),
                typeof(ClientScope), typeof(ClientSecret), typeof(DeviceFlowCodes), typeof(IdentityClaim),
                typeof(IdentityResource), typeof(IdentityResourceProperty), typeof(PersistedGrant),
                typeof(Property), typeof(Secret), typeof(UserClaim)
            };

            foreach (var type in modelTypes) {
                modelBuilder.Entity(type)
                    .Property("SysStart")
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("(getdate())")
                    .ValueGeneratedOnAddOrUpdate();
                modelBuilder.Entity(type)
                    .Property("SysEnd")
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("(getdate())")
                    .ValueGeneratedOnAddOrUpdate();

                if (type == typeof(ApiResource) || type == typeof(Client)
                    || type == typeof(IdentityResource)) {
                    modelBuilder.Entity(type)
                        .Property("Updated")
                        .HasComputedColumnSql("case when convert(datetime,[SysStart]) = [Created] then null else convert(datetime,[SysStart]) end");
                }
            }

        }

    }
}
