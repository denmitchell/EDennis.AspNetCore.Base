using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.Samples.DefaultPoliciesApi.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> Person { get; set; }
        public DbSet<Position> Position { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Person>()
                    .ToTable("Person")
                    .HasData(
                        new Person { Id = 1, Name = "Moe" },
                        new Person { Id = 2, Name = "Larry" },
                        new Person { Id = 3, Name = "Curly" }
                        );
                      
            builder.Entity<Position>()
                .ToTable("Position")
                    .HasData(
                        new Position { Id = 1, Title = "Manager" },
                        new Position { Id = 2, Title = "Employee" }
                        );

            Debug.WriteLine(Database.GetDbConnection().ConnectionString);
            Debug.WriteLine($"Database.IsInMemory():{Database.IsInMemory()}");
            Debug.WriteLine($"Database.IsSqlite():{Database.IsSqlite()}");
            Debug.WriteLine($"Database.IsSqlServer():{Database.IsSqlServer()}");
        }
    }
}
