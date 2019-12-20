using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi {

    /// <summary>
    /// To Add Migration :
    /// with DefaultProject = EDennis.Samples.DbContextConfigsApi ...
    ///     PM > Add-Migration Initial -Context DbContext1 -Project EDennis.Samples.DbContextConfigsApi.Lib -StartupProject EDennis.Samples.DbContextConfigsApi
    /// To Update Database:
    ///     PM > Update-Database -Context DbContext1 -Project EDennis.Samples.DbContextConfigsApi.Lib -StartupProject EDennis.Samples.DbContextConfigsApi
    /// </summary>
    public class DbContextDesignTimeFactory1 : DbContextDesignTimeFactory<AppDbContext> { }
    public class AppDbContext : DbContext {
        public DbSet<Person> Person { get; set; }
        public DbSet<Position> Position { get; set; }

        public AppDbContext(DbContextOptionsProvider<AppDbContext> provider) :
            base(provider.DbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Person>()
                    .ToTable("Person")
                    .HasData(
                        new Person { Id = 1, Name = "Moe", SysUser = "jack@hill.org" },
                        new Person { Id = 2, Name = "Larry", SysUser = "jill@hill.org" },
                        new Person { Id = 3, Name = "Curly", SysUser = "jack@hill.org" }
                        );

            builder.Entity<Position>()
                .ToTable("Position")
                    .HasData(
                        new Position { Id = 1, Title = "Manager", SysUser = "jill@hill.org" },
                        new Position { Id = 2, Title = "Employee", SysUser = "jack@hill.org" }
                        );

        }
    }
}
