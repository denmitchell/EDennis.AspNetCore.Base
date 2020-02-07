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
                e.ToTable("Project")
                    .HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<Setting>(e => {
                e.ToTable("Setting")
                    .HasKey(e => new { e.Id });
            });

            modelBuilder.Entity<ProjectSetting>(e => {

                e.ToTable("ProjectSetting")
                    .HasKey(e=> new { e.ProjectId, e.SettingId });

                e.Property(e => e.SysStart)
                    .HasDefaultValueSql("(getdate())")
                    .ValueGeneratedOnAddOrUpdate();

                e.Property(e => e.SysEnd)
                    .HasDefaultValueSql("(CONVERT(datetime2, '9999-12-31 23:59:59.9999999'))")
                    .ValueGeneratedOnAddOrUpdate();

                e.HasOne(e => e.Project)
                    .WithMany(f => f.ProjectSettings)
                    .HasForeignKey(e=>e.ProjectId)
                    .HasConstraintName("fk_ProjectSetting_Project")
                    .OnDelete(DeleteBehavior.ClientCascade);

                e.HasOne(e => e.Setting)
                    .WithMany(f => f.ProjectSettings)
                    .HasForeignKey(e=>e.SettingId)
                    .HasConstraintName("fk_ProjectSetting_Setting")
                    .OnDelete(DeleteBehavior.ClientCascade);
            });


            modelBuilder.Entity<ProjectSettingView>(e => {
                e.HasNoKey()
                    .ToView("vwProjectSetting");
            });

        }
    }
}
