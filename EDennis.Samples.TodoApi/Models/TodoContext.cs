using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace EDennis.Samples.TodoApi.Models {

    public class TodoContextDesignTimeFactory :
        SqlTemporalContextDesignTimeFactory<TodoContext> { }

    public class TodoContext : DbContextBase {

        public TodoContext(DbContextOptions options) : base(options) { }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {


            #region Task
            modelBuilder.Entity<Task>()
                .ToTable("Task")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Task>()
                .Property(e => e.Id)
                .UseSqlServerIdentityColumn();

            if (Database.IsInMemory()) {
                modelBuilder.Entity<Task>()
                    .Property(e => e.Id)
                    .HasValueGenerator<MaxPlusOneValueGenerator>();
            }


            modelBuilder.Entity<Task>()
                .Property(e => e.Title)
                .HasMaxLength(50);

            modelBuilder.Entity<Task>()
                .Property(e => e.DueDate)
                .HasColumnType("datetime");

            modelBuilder.Entity<Task>()
                .Property(e => e.SysStart)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(getdate())")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Task>()
                .Property(e => e.SysEnd)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("(convert(datetime2,'9999-12-31 23:59:59.9999999'))")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Task>()
                .HasData(TodoContextDataFactory.TaskRecords);
            #endregion

        }

    }
}