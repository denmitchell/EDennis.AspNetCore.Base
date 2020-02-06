using Microsoft.EntityFrameworkCore;

using ConfigCore.Models;

namespace ConfigurationApi.Lib.Models {
    public class ConfigurationDbContext : DbContext {

        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options)
            : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Project> Settings { get; set; }
        public DbSet<ProjectSetting> ProjectSettings { get; set; }
        public DbSet<ProjectSettingView> ProjectSettingView { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.HasSequence<int>("seqProject");
            modelBuilder.HasSequence<int>("seqSetting");

            modelBuilder.Entity<Project>(e => {
                e.ToTable("Project");
            });

            modelBuilder.Entity<Setting>(e => {
                e.ToTable("Setting");
            });

            modelBuilder.Entity<ProjectSetting>(e => {

                e.ToTable("ProjectSetting");

                e.Property(e => e.SysStart)
                    .HasDefaultValueSql("(getdate())")
                    .ValueGeneratedOnAddOrUpdate();

                e.Property(e => e.SysEnd)
                    .HasDefaultValueSql("(CONVERT(datetime2, '9999-12-31 23:59:59.9999999'))")
                    .ValueGeneratedOnAddOrUpdate();

            });

            modelBuilder.Entity<ProjectSettingView>(e => {
                e.HasNoKey()
                    .ToView("vwProjectSetting");
            });

        }
    }
}
