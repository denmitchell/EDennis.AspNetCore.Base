using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.Samples.ScopedLogging.ColorsApi.Models
{
    public class ColorDbContext : DbContext
    {
        public DbSet<Color> Color { get; set; }

        public ColorDbContext(DbContextOptions<ColorDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Color>()
                    .ToTable("Color")
                    .HasData(
                        new Color { Id = 1, Name = "Black", Red = 0, Green = 0, Blue = 0 },
                        new Color { Id = 2, Name = "White", Red = 255, Green = 255, Blue = 255 },
                        new Color { Id = 3, Name = "Grey", Red = 127, Green = 127, Blue = 127 },
                        new Color { Id = 4, Name = "Red", Red = 255, Green = 0, Blue = 0 },
                        new Color { Id = 5, Name = "Green", Red = 0, Green = 255, Blue = 0 },
                        new Color { Id = 6, Name = "Blue", Red = 0, Green = 0, Blue = 255 },
                        new Color { Id = 7, Name = "Yellow", Red = 255, Green = 255 }
                        );

        }
    }
}

