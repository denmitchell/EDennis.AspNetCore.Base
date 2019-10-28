using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        public DbConnectionInterceptor(RequestDelegate next){
            _next = next;
        }

        ILogger _logger;

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider,
            IConfiguration appConfig, IScopeProperties scopeProperties,
            ILogger<DbConnectionInterceptor<TContext>> logger) {


            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {


                _logger = logger;
                _logger.LogInformation("RepoInterceptor handling request: {RequestPath}", context.Request.Path);


                var instruction = scopeProperties.Instruction;
                var profile = scopeProperties.ActiveProfile;

                //get the database name and cache
                var baseDatabaseName = TestDbContextManager<TContext>.BaseDatabaseName(appConfig);
                var cache = serviceProvider.GetRequiredService(typeof(DbConnectionCache<TContext>))
                        as DbConnectionCache<TContext>;

                //get the newly created DbContextOptions provider and builder for the current scope
                var dbContextOptionsProvider = serviceProvider.GetRequiredService<DbContextOptionsProvider<TContext>>();
                var dbContextOptionsBuilder = serviceProvider.GetRequiredService<DbContextOptionsBuilder<TContext>>();



                var findResult = instruction.Find(cache.Keys);
                var cachedCxn = cache[findResult.MatchingInstanceName];

                //if asking for a cached DbContextOptions instance, retrieve and use it, regardless of type
                if (findResult.ToggleComparisonResult == ToggleComparisonResult.Same) {
                    dbContextOptionsProvider.DbContextOptions = cachedCxn.DbContextOptions;
                } else if (findResult.ToggleComparisonResult == ToggleComparisonResult.Reset
                    || findResult.ToggleComparisonResult == ToggleComparisonResult.Different) {
                    if (instruction.ConnectionType != ConnectionType.InMemory) {
                        if (instruction.ConnectionType == ConnectionType.Rollback) {
                            if (cachedCxn.IDbConnection != null
                                && cachedCxn.IDbConnection.State == ConnectionState.Open) {
                                if (cachedCxn.IDbTransaction != null)
                                    cachedCxn.IDbTransaction.Rollback();
                                cachedCxn.IDbConnection.Close();
                            }
                        }
                    }
                    cache.Remove(findResult.MatchingInstanceName);
                }

                var manager = new DbContextOptionsManager<TContext>();
                if (findResult.MatchingInstanceName == null || findResult.ToggleComparisonResult == ToggleComparisonResult.Different) {
                    if (instruction.ConnectionType == ConnectionType.InMemory) {
                        manager = manager.BuildOptions(instruction.InstanceName)
                            .UpdateCache(instruction.InstanceName,cache)
                            .UpdateProvider(dbContextOptionsProvider);
                    } else if (instruction.ConnectionType == ConnectionType.Rollback) {
                        var connectionString = profile.ConnectionStrings[typeof(TContext).Name];
                        var provider = DatabaseProviderExtensions.InferProvider(connectionString);
                        if (provider == DatabaseProvider.SqlServer) { 
                            manager = manager.BuildOptions<SqlConnection>(connectionString, instruction.IsolationLevel)
                                .UpdateCache(instruction.InstanceName, cache)
                                .UpdateProvider(dbContextOptionsProvider);
                        } else {
                            manager = manager.BuildOptions<SqliteConnection>(connectionString, instruction.IsolationLevel)
                                .UpdateCache(instruction.InstanceName, cache)
                                .UpdateProvider(dbContextOptionsProvider);
                        }
                    }
                }

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