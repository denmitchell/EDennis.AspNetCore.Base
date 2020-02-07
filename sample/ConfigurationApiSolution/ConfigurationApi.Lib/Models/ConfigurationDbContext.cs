using Microsoft.EntityFrameworkCore;

namespace ConfigurationApi.Lib.Models {
    public class ConfigurationDbContext : DbContext {

        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options)
            : base(options) { }

        public DbSet<ProjectSetting> ProjectSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<ProjectSetting>(e => {

                e.ToTable("ProjectSetting")
                    .HasKey(e=> new { e.ProjectName, e.SettingKey });

                e.Property(e => e.SysStart)
                    .HasDefaultValueSql("(getdate())")
                    .ValueGeneratedOnAddOrUpdate();

                e.Property(e => e.SysEnd)
                    .HasDefaultValueSql("(CONVERT(datetime2, '9999-12-31 23:59:59.9999999'))")
                    .ValueGeneratedOnAddOrUpdate();

            });


        }
    }
}
