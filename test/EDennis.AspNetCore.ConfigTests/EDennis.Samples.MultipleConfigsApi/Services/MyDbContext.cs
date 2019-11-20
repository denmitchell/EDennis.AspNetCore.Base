using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace EDennis.Samples.MultipleConfigsApi.Services {
    public class MyDbContext : DbContext {
        public MyDbContext(DbContextOptionsProvider<MyDbContext> provider) :
            base(provider.DbContextOptions) { }

        public DbSet<MyEntity1> MyEntity1s { get; set; }
        public DbSet<MyEntity1> MyEntity2s { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<MyEntity1>()
                .HasData(new MyEntity1[] {
                    new MyEntity1 { MyIntProp=1, MyStringProp="A", SysUser="moe"},
                    new MyEntity1 { MyIntProp=2, MyStringProp="B", SysUser="larry"},
                    new MyEntity1 { MyIntProp=3, MyStringProp="C", SysUser="curly"}
                });
            modelBuilder.Entity<MyEntity2>()
                .HasData(new MyEntity2[] {
                    new MyEntity2 { MyIntProp=1, MyStringProp="D", SysUser="posh"},
                    new MyEntity2 { MyIntProp=2, MyStringProp="E", SysUser="baby"},
                    new MyEntity2 { MyIntProp=3, MyStringProp="F", SysUser="ginger"}
                });
        }

    }
}
