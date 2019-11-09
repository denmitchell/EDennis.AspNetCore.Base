using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Testing {
    public class DbInterceptorMiddleware<TContext>
        where TContext : DbContext {

        protected readonly RequestDelegate _next;

        public DbInterceptorMiddleware(RequestDelegate next) {
            _next = next;
        }

        ILogger _logger;

        public async Task InvokeAsync(HttpContext context,
            IScopeProperties scopeProperties, IOptionsMonitor<AppSettings> appSettings,
            DbContextOptionsProvider<TContext> dbContextOptionsProvider,
            DbConnectionCache<TContext> cache,
            ILogger<DbInterceptorMiddleware<TContext>> logger) {

            DbContextSettings dbContextSettings = appSettings.CurrentValue.DbContexts[typeof(TContext).Name];


            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {


                _logger = logger;
                _logger.LogInformation("RepoInterceptor handling request: {RequestPath}", context.Request.Path);


                var newConnection = scopeProperties.NewConnection;

                bool added = false;

                var cachedCxn = cache.GetOrAdd(scopeProperties.User,(key)=> {
                    added = true;
                    return DbConnectionManager.GetDbConnection<TContext>(dbContextSettings);
                });

                if (!added && scopeProperties.NewConnection)
                    if (dbContextSettings.DatabaseProvider == DatabaseProvider.InMemory)
                        cache[scopeProperties.User] = DbConnectionManager.GetInMemoryDbConnection<TContext>();
                    else if (dbContextSettings.TransactionType == TransactionType.Rollback)
                        cachedCxn.IDbTransaction.Rollback();

                dbContextOptionsProvider.DbContextOptions = cachedCxn.DbContextOptions;

            }

            await _next(context);

        }


    }


    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseDbInterceptor<TContext>(this IApplicationBuilder app)
        where TContext : DbContext {
            app.UseMiddleware<DbInterceptorMiddleware<TContext>>();
            return app;
        }
    }

}