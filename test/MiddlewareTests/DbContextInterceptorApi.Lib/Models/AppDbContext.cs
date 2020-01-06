﻿using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer;
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
    public class AppDbContext : DbContext {
        public DbSet<Person> Person { get; set; }
        public DbSet<Position> Position { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {

            builder.HasSequence<int>("seqPerson");
            builder.HasSequence<int>("seqPosition");

            builder.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            builder.Entity<Person>(e => {
                e.ToTable("Person");
                e.HasKey(e => e.Id);
                e.Property(e => e.Id)
                    .HasDefaultValueSql("NEXT VALUE FOR seqPerson")
                    .ValueGeneratedOnAdd();
                e.HasData(
                        new Person { Id = -999001, Name = "Mike", SysUser = "jack@hill.org" },
                        new Person { Id = -999002, Name = "Carol", SysUser = "jill@hill.org" },
                        new Person { Id = -999003, Name = "Greg", SysUser = "jack@hill.org" }
                        );
            });

            builder.Entity<Position>(e => {
                e.ToTable("Position");
                e.HasKey(e => e.Id);
                e.Property(e => e.Id)
                    .HasDefaultValueSql("NEXT VALUE FOR seqPosition")
                    .ValueGeneratedOnAdd();
                e.HasData(
                        new Position { Id = -999001, Title = "President", SysUser = "jill@hill.org" },
                        new Position { Id = -999002, Title = "Vice-president", SysUser = "jack@hill.org" }
                        );

            });
        }
    }
}
