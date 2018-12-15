using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class QueryTest<TContext>  
        where TContext : DbContextBase {

        protected TContext Context;

        public QueryTest() {
            IConfiguration config =
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var _cxns = config.GetSection("ConnectionStrings").GetChildren();

            string ConnectionStringName = typeof(TContext).Name;
            string connectionString = _cxns.Where(x => x.Key == ConnectionStringName).FirstOrDefault().Value;

            var options = new DbContextOptionsBuilder()
                .UseSqlServer(connectionString);

            Context = Activator.CreateInstance(typeof(TContext), new object[] { options }) as TContext;
        }


    }
}
