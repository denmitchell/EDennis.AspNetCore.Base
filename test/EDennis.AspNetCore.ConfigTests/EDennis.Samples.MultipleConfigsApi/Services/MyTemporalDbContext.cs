using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace EDennis.Samples.MultipleConfigsApi.Services {

    public class MyTemporalHistoryDbContext : DbContext {
        public MyTemporalHistoryDbContext(DbContextOptionsProvider<MyTemporalHistoryDbContext> provider) :
            base(provider.DbContextOptions) { }

        public DbSet<MyTemporalHistoryEntity1> MyTemporalHistoryEntity1s { get; set; }
        public DbSet<MyTemporalHistoryEntity1> MyTemporalHistoryEntity2s { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<MyTemporalHistoryEntity1>()
                .HasData(new MyTemporalHistoryEntity1[] {
                    new MyTemporalHistoryEntity1 { MyIntProp=1, MyStringProp="A", SysUser="moe"},
                    new MyTemporalHistoryEntity1 { MyIntProp=2, MyStringProp="B", SysUser="larry"},
                    new MyTemporalHistoryEntity1 { MyIntProp=3, MyStringProp="C", SysUser="curly"}
                });
            modelBuilder.Entity<MyTemporalHistoryEntity2>()
                .HasData(new MyTemporalHistoryEntity2[] {
                    new MyTemporalHistoryEntity2 { MyIntProp=1, MyStringProp="D", SysUser="posh"},
                    new MyTemporalHistoryEntity2 { MyIntProp=2, MyStringProp="E", SysUser="baby"},
                    new MyTemporalHistoryEntity2 { MyIntProp=3, MyStringProp="F", SysUser="ginger"}
                });
        }


    }


    public class MyTemporalDbContext : DbContext {
        public MyTemporalDbContext(DbContextOptionsProvider<MyTemporalDbContext> provider) :
            base(provider.DbContextOptions) { }

        public DbSet<MyTemporalEntity1> MyTemporalEntity1s { get; set; }
        public DbSet<MyTemporalEntity1> MyTemporalEntity2s { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<MyTemporalEntity1>()
                .HasData(new MyTemporalHistoryEntity1[] {
                    new MyTemporalHistoryEntity1 { MyIntProp=1, MyStringProp="A", SysUser="moe"},
                    new MyTemporalHistoryEntity1 { MyIntProp=2, MyStringProp="B", SysUser="larry"},
                    new MyTemporalHistoryEntity1 { MyIntProp=3, MyStringProp="C", SysUser="curly"}
                });
            modelBuilder.Entity<MyTemporalHistoryEntity2>()
                .HasData(new MyTemporalHistoryEntity2[] {
                    new MyTemporalHistoryEntity2 { MyIntProp=1, MyStringProp="D", SysUser="posh"},
                    new MyTemporalHistoryEntity2 { MyIntProp=2, MyStringProp="E", SysUser="baby"},
                    new MyTemporalHistoryEntity2 { MyIntProp=3, MyStringProp="F", SysUser="ginger"}
                });
        }

    }
}
