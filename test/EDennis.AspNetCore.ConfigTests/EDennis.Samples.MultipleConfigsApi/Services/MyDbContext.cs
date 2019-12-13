using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace EDennis.Samples.MultipleConfigsApi.Services {
    public class MyDbContext : DbContext {
        public MyDbContext(DbContextOptionsProvider<MyDbContext> provider) :
            base(provider.DbContextOptions) { }

        public DbSet<MyEntity> MyEntity1s { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<MyEntity>()
                .HasData(new MyEntity[] {
                    new MyEntity { MyIntProp=1, MyStringProp="A", SysUser="moe"},
                    new MyEntity { MyIntProp=2, MyStringProp="B", SysUser="larry"},
                    new MyEntity { MyIntProp=3, MyStringProp="C", SysUser="curly"}
                });
        }

    }
}
