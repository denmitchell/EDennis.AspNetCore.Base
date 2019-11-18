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


namespace EDennis.AspNetCore.Base.Web {
    public class DbContextInterceptorMiddleware<TContext>
        where TContext : DbContext {

        protected readonly RequestDelegate _next;

        public DbContextInterceptorMiddleware(RequestDelegate next) {
            _next = next;
        }

        ILogger _logger;

        public async Task InvokeAsync(HttpContext context,
            IOptionsMonitor<DbContextInterceptorSettings<TContext>> settings,
            DbContextOptionsProvider<TContext> dbContextOptionsProvider,
            DbConnectionCache<TContext> cache,
            ILogger<DbContextInterceptorMiddleware<TContext>> logger) {


            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {


                _logger = logger;
                _logger.LogInformation("RepoInterceptor handling request: {RequestPath}", context.Request.Path);


                bool newConnection = false;

                if (context.Request.Headers.ContainsKey(Constants.ROLLBACK_REQUEST_KEY))
                    newConnection = true;
                else if (context.Request.Query.ContainsKey(Constants.ROLLBACK_QUERY_KEY))
                    newConnection = true;

                var instanceName = MiddlewareUtils.ResolveUser(context, settings.CurrentValue.InstanceNameSource, "DbContextInterceptor InstanceName");


                bool added = false;

                var cachedCxn = cache.GetOrAdd(instanceName,(key)=> {
                    added = true;
                    return DbConnectionManager.GetDbConnection(settings.CurrentValue);
                });

                if (!added && newConnection)
                    if (settings.CurrentValue.IsInMemory)
                        cache[instanceName] = DbConnectionManager.GetInMemoryDbConnection<TContext>();
                    else 
                        cachedCxn.IDbTransaction.Rollback();

                dbContextOptionsProvider.DbContextOptions = cachedCxn.DbContextOptionsBuilder.Options;

            }

            await _next(context);

        }


    }


    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseDbContextInterceptor<TContext>(this IApplicationBuilder app)
        where TContext : DbContext {
            app.UseMiddleware<DbContextInterceptorMiddleware<TContext>>();
            return app;
        }
    }

}