using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Testing {
    public class TemporalRepoInterceptor<TRepo, TEntity, TContext, THistoryContext> : Interceptor
        where TEntity : class, IEFCoreTemporalModel, new()
        where TContext : DbContext
        where THistoryContext : DbContext
        where TRepo : WriteableTemporalRepo<TEntity, TContext, THistoryContext> {

        public TemporalRepoInterceptor(RequestDelegate next) : base(next) { }

        ILogger _logger;

        public async Task InvokeAsync(HttpContext context,
            IServiceProvider provider, IConfiguration config,
            ILogger<TemporalRepoInterceptor<TRepo, TEntity, TContext, THistoryContext>> logger) {

            _logger = logger;

            _logger.LogInformation($"TemporalRepoInterceptor handling request: {context.Request.Path}");


            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {


                var header = GetTestingHeader(context);

                if (header.Key == null) {

                    var defaultInstanceName = DEFAULT_NAMED_INSTANCE;
                    try { 
                        if (context.Session != null)
                            defaultInstanceName = context.Session.Id;
                    } catch { }

                    context.Request.Headers.Add(HDR_USE_INMEMORY, defaultInstanceName);
                    header = new KeyValuePair<string, string>(HDR_USE_INMEMORY, defaultInstanceName);
                }

                _logger.LogInformation($"TemporalRepoInterceptor processing header {header.Key}: {header.Value}");


                var operation = header.Key;
                var baseInstanceName = header.Value;
                var histInstanceName = header.Value + HISTORY_INSTANCE_SUFFIX;

                var repo = provider.GetRequiredService(typeof(TRepo)) as TRepo;

                var cache = provider.GetRequiredService(typeof(TestDbContextOptionsCache<TContext>))
                        as TestDbContextOptionsCache<TContext>;
                var historyCache = provider.GetRequiredService(typeof(TestDbContextOptionsCache<THistoryContext>))
                        as TestDbContextOptionsCache<THistoryContext>;

                var baseDatabaseName = TestDbContextManager<TContext>.BaseDatabaseName(config);
                var histDatabaseName = TestDbContextManager<THistoryContext>.BaseDatabaseName(config);

                if (operation == HDR_USE_RELATIONAL) {
                    _logger.LogInformation($"Using Relational database");
                } else if (operation == HDR_USE_READONLY)
                    throw new ApplicationException("HDR_USE_READONLY not appropriate for Writeable Repo.");
                else if (operation == HDR_USE_INMEMORY) {
                    GetOrAddInMemoryDatabase(repo, cache, baseInstanceName, baseDatabaseName);
                    GetOrAddInMemoryHistoryDatabase(repo, historyCache, histInstanceName, histDatabaseName);
                } else if (operation == HDR_DROP_INMEMORY) {
                    DropInMemory(cache, baseInstanceName);
                    DropInMemoryHistory(historyCache, histInstanceName);
                }
            }

            await _next(context);

        }

        private void GetOrAddInMemoryDatabase(TRepo repo,
            TestDbContextOptionsCache<TContext> cache,
            string instanceName, string baseDatabaseName) {
            if (cache.ContainsKey(instanceName)) {
                _logger.LogInformation($"Using existing in-memory database {baseDatabaseName}, instance = {instanceName}");
                TestDbContextManager<TContext>.CreateInMemoryDatabase(cache[instanceName], out TContext context);
                repo.Context = context;
            } else {
                _logger.LogInformation($"Creating in-memory database {baseDatabaseName}, instance = {instanceName}");
                TestDbContextManager<TContext>.CreateInMemoryDatabase(baseDatabaseName, instanceName,
                    out DbContextOptions<TContext> options, out TContext context);
                repo.Context = context;
                repo.Context.Database.EnsureCreated();
                cache.Add(instanceName, options);
            }
        }



        private void GetOrAddInMemoryHistoryDatabase(TRepo repo,
            TestDbContextOptionsCache<THistoryContext> cache,
            string instanceName, string baseDatabaseName) {
            if (cache.ContainsKey(instanceName)) {
                _logger.LogInformation($"Using existing in-memory history database {baseDatabaseName}, instance = {instanceName}");
                TestDbContextManager<THistoryContext>.CreateInMemoryDatabase(cache[instanceName], out THistoryContext context);
                repo.HistoryContext = context;
            } else {
                _logger.LogInformation($"Creating in-memory history database {baseDatabaseName}, instance = {instanceName}");
                TestDbContextManager<THistoryContext>.CreateInMemoryDatabase(baseDatabaseName, instanceName,
                    out DbContextOptions<THistoryContext> options, out THistoryContext context);
                repo.HistoryContext = context;
                repo.HistoryContext.Database.EnsureCreated();
                cache.Add(instanceName, options);
            }
        }


        private void DropInMemory(TestDbContextOptionsCache<TContext> cache,
            string instanceName) {
            if (cache.ContainsKey(instanceName)) {
                _logger.LogInformation($"Dropping in-memory history instance {instanceName} for {typeof(TContext).Name}");
                var context = cache[instanceName];
                TestDbContextManager<TContext>.DropInMemoryDatabase(context);
                cache.Remove(instanceName);
            }
        }



        private void DropInMemoryHistory(TestDbContextOptionsCache<THistoryContext> cache,
            string instanceName) {
            if (cache.ContainsKey(instanceName)) {
                _logger.LogInformation($"Dropping in-memory history database instance {instanceName} for {typeof(THistoryContext).Name}");
                var context = cache[instanceName];
                TestDbContextManager<THistoryContext>.DropInMemoryDatabase(context);
                cache.Remove(instanceName);
            }



        }
    }

    public static partial class IApplicationBuilderExtensions_RepoInterceptorMiddleware {
        public static IApplicationBuilder UseTemporalRepoInterceptor<TRepo, TEntity, TContext, THistoryContext>(this IApplicationBuilder app)
                where TEntity : class, IEFCoreTemporalModel, new()
        where TContext : DbContext
        where THistoryContext : DbContext
        where TRepo : WriteableTemporalRepo<TEntity, TContext, THistoryContext> {
            app.UseMiddleware<TemporalRepoInterceptor<TRepo, TEntity, TContext, THistoryContext>>();
            return app;
        }
    }


}
