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
    public class DbConnectionInterceptor<TContext>
        where TContext : DbContext {

        public const string TESTING_HDR = "X-Testing-Config";

        //TODO: Have ScopePropertiesMiddleware translate this claim header into above TESTING_HDR
        //TODO: Also have ScopePropertiesMiddleware translate X-Testing-Config claim into above TESTING_HDR -- simpler                
        public const string TESTING_CLAIM_HDR = "X-Claim-X-Testing-Config";

        protected readonly RequestDelegate _next;

        public DbConnectionInterceptor(RequestDelegate next) {
            _next = next;
        }

        ILogger _logger;

        public async Task InvokeAsync(HttpContext context,
            IScopeProperties scopeProperties, IOptionsMonitor<AppSettings> appSettings,
            DbContextOptionsProvider<TContext> dbContextOptionsProvider,
            DbConnectionCache<TContext> cache,
            ILogger<DbConnectionInterceptor<TContext>> logger) {

            EFContext efContextSettings = appSettings.CurrentValue.EFContexts[typeof(TContext).Name];


            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {


                _logger = logger;
                _logger.LogInformation("RepoInterceptor handling request: {RequestPath}", context.Request.Path);


                var newConnection = scopeProperties.NewConnection;

                bool added = false;

                var cachedCxn = cache.GetOrAdd(scopeProperties.User,(key)=> {
                    added = true;
                    return DbConnectionManager.GetDbConnection<TContext>(efContextSettings);
                });

                if (!added && scopeProperties.NewConnection)
                    if (efContextSettings.ProviderName.Equals("inmemory", StringComparison.OrdinalIgnoreCase))
                        cache[scopeProperties.User] = DbConnectionManager.GetInMemoryDbConnection<TContext>();
                    else if (efContextSettings.TransactionType == TransactionType.Rollback)
                        cachedCxn.IDbTransaction.Rollback();

                dbContextOptionsProvider.DbContextOptions = cachedCxn.DbContextOptions;

            }

            await _next(context);

        }


    }


    public static partial class IApplicationBuilderExtensions_RepoInterceptorMiddleware {
        public static IApplicationBuilder UseDbContextInterceptor<TContext>(this IApplicationBuilder app)
        where TContext : DbContext {
            app.UseMiddleware<DbConnectionInterceptor<TContext>>();
            return app;
        }
    }

}