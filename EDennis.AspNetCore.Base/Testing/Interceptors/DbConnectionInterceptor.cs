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
            IConfiguration appConfig,

            ILogger<DbConnectionInterceptor<TContext>> logger) {


            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {


                _logger = logger;
                _logger.LogInformation("RepoInterceptor handling request: {RequestPath}", context.Request.Path);


                var scopeProperties = serviceProvider.GetRequiredService<ScopeProperties>();

                string testConfigUnparsed;
                try {
                    testConfigUnparsed = scopeProperties.Headers[TESTING_HDR].ToString();
                } catch (Exception) {
                    throw new ApplicationException($"ScopeProperties is missing {TESTING_HDR} key.  Please ensure that ScopeProperties is configured in Startup and ScopePropertiesMiddleware precedes this interceptor in the request pipeline (in Startup.Configure).");
                }
                _logger.LogInformation("RepoInterceptor processing header {Header}",$"{TESTING_HDR}:{testConfigUnparsed}");


                var parser = new TestConfigParser();
                var testConfig = parser.Parse(testConfigUnparsed);

                var profileConfig = parser.GetProfileConfiguration(testConfig, appConfig);
                var profile = profileConfig.Profile;

                //get the database name and cache
                var baseDatabaseName = TestDbContextManager<TContext>.BaseDatabaseName(appConfig);
                var cache = serviceProvider.GetRequiredService(typeof(DbConnectionCache<TContext>))
                        as DbConnectionCache<TContext>;

                //get the newly created DbContextOptions provider and builder for the current scope
                var dbContextOptionsProvider = serviceProvider.GetRequiredService<DbContextOptionsProvider<TContext>>();
                var dbContextOptionsBuilder = serviceProvider.GetRequiredService<DbContextOptionsBuilder<TContext>>();



                var findResult = testConfig.Find(cache.Keys);

                //if asking for a cached DbContextOptions instance, retrieve and use it, regardless of type
                if (findResult.ToggleComparisonResult == ToggleComparisonResult.Same) {
                    dbContextOptionsProvider.DbContextOptions = cache[findResult.MatchingInstanceName].DbContextOptions;
                } else if (findResult.ToggleComparisonResult == ToggleComparisonResult.Reset
                    || findResult.ToggleComparisonResult == ToggleComparisonResult.Different) {
                    if (testConfig.ConnectionType != ConnectionType.InMemory) {
                        if (testConfig.ConnectionType == ConnectionType.Rollback) {
                            if (cache[findResult.MatchingInstanceName].IDbConnection != null
                                && cache[findResult.MatchingInstanceName].IDbConnection.State == ConnectionState.Open) {
                                if (cache[findResult.MatchingInstanceName].IDbTransaction != null)
                                    cache[findResult.MatchingInstanceName].IDbTransaction.Rollback();
                                cache[findResult.MatchingInstanceName].IDbConnection.Close();
                            }
                        }
                    }
                    cache.Remove(findResult.MatchingInstanceName);
                }

                if (findResult.MatchingInstanceName == null || findResult.ToggleComparisonResult == ToggleComparisonResult.Different) {
                    if (testConfig.ConnectionType == ConnectionType.InMemory) {
                        dbContextOptionsProvider.DbContextOptions =
                            new DbContextOptionsBuilder<TContext>()
                            .UseInMemoryDatabase(testConfig.InstanceName)
                            .Options;
                    } else if (testConfig.ConnectionType == ConnectionType.Rollback) {
                        var connectionString = profile.ConnectionStrings[typeof(TContext).Name];
                        var provider = DatabaseProviderExtensions.InferProvider(connectionString);
                        IDbConnection connection;
                        IDbTransaction transaction;
                        
                        if (provider == DatabaseProvider.SqlServer) {
                            connection = new SqlConnection(connectionString);
                            connection.Open();
                            transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                            dbContextOptionsProvider.DbContextOptions =
                                new DbContextOptionsBuilder<TContext>()
                                .UseSqlServer(connection as SqlConnection)
                                .Options;
                        } else {
                            if (!Regex.IsMatch(connectionString, "cache\\s*=\\s*shared", RegexOptions.IgnoreCase))
                                connectionString = (connectionString + ";cache=shared").Replace(";;", ";");
                            connection = new SqliteConnection(profile.ConnectionStrings[typeof(TContext).Name]);
                            connection.Open();
                            transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                            dbContextOptionsProvider.DbContextOptions =
                                new DbContextOptionsBuilder<TContext>()
                                .UseSqlite(connection as SqliteConnection)
                                .Options;
                        }
                        cache[testConfig.InstanceName] = new DbConnection<TContext> {
                            DbContextOptions = dbContextOptionsProvider.DbContextOptions,
                            IDbConnection = connection,
                            IDbTransaction = transaction
                        };
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