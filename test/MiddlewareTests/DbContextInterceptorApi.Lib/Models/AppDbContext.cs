using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi {

    /// <summary>
    /// To Add Migration :
    /// with DefaultProject = EDennis.Samples.DbContextConfigsApi ...
    ///     PM > Add-Migration Initial -Context AppDbContext -Project DbContextInterceptorApi.Lib -StartupProject DbContextInterceptorApi
    /// To Update Database:
    ///     PM > Update-Database -Context AppDbContext -Project DbContextInterceptorApi.Lib -StartupProject DbContextInterceptorApi
    /// </summary>
    public class DbContextDesignTimeFactory1 : DbContextDesignTimeFactory<AppDbContext> { }
    public class AppDbContext : ResettableDbContext<AppDbContext> {
        public DbSet<Person> Person { get; set; }
        public DbSet<Position> Position { get; set; }

        public AppDbContext(DbContextOptionsProvider<AppDbContext> provider) :
            base(provider) { }

        protected override void OnModelCreating(ModelBuilder builder) {

            builder.HasSequence<int>("seqPerson");
            builder.HasSequence<int>("seqPosition");

            builder.Entity<Person>()
                .Property(e => e.Id)
                .HasDefaultValueSql("NEXT VALUE FOR seqPerson");
            
            builder.Entity<Person>()
                    .ToTable("Person")
                    .HasData(
                        new Person { Id = -999001, Name = "Mike", SysUser = "jack@hill.org" },
                        new Person { Id = -999002, Name = "Carol", SysUser = "jill@hill.org" },
                        new Person { Id = -999003, Name = "Greg", SysUser = "jack@hill.org" }
                        );

            builder.Entity<Position>()
                .Property(e => e.Id)
                .HasDefaultValueSql("NEXT VALUE FOR seqPosition");

            builder.Entity<Position>()
                    .ToTable("Position")
                    .HasData(
                        new Position { Id = -999001, Title = "President", SysUser = "jill@hill.org" },
                        new Position { Id = -999002, Title = "Vice-president", SysUser = "jack@hill.org" }
                        );

        }
    }
}
