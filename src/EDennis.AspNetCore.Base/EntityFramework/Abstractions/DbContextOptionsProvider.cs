using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// 
    /// Configure DbContext as such:
    /// <code>
    /// public AppDbContext(DbContextOptionsProvider<AppDbContext> optionsProvider) 
    ///        : base(optionsProvider.DbContextOptions) { }
    /// </code>
    /// 
    /// Configure Startup.ConfigureServices as such:
    /// <code>
    /// services.AddScoped<DbContextOptionsProvider<AppDbContext>>();
    /// services.AddDbContext<AppDbContext>(options =>
    ///     options.UseSqlite($"Data Source={HostingEnvironment.ContentRootPath}/hr.db")
    /// );
    /// </code>
    /// 
    ///    or as such ...
    /// <code>
    /// services.AddDbContext<AppDbContext>(options =>
    ///     options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=hr3;Trusted_Connection=True;MultipleActiveResultSets=true")
    /// );
    /// </code>
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DbContextOptionsProvider<TContext>
        where TContext : DbContext {
        public DbContextOptions<TContext> DbContextOptions { get; set; }
        public bool DisableAutoTransactions { get; set; }
        public IDbTransaction Transaction { get; set; }
        public DbContextOptionsProvider(DbContextOptions<TContext> options) { DbContextOptions = options; }
    }

}
