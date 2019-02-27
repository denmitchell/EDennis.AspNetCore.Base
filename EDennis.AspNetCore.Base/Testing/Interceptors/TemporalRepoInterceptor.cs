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
                    context.Request.Headers.Add(HDR_USE_INMEMORY, DEFAULT_NAMED_INSTANCE);
                    header = new KeyValuePair<string, string>(HDR_USE_INMEMORY, DEFAULT_NAMED_INSTANCE);
                }

                _logger.LogInformation($"TemporalRepoInterceptor processing header {header.Key}: {header.Value}");


                var operation = header.Key;
                var baseInstanceName = header.Value;
                var histInstanceName = header.Value + HISTORY_INSTANCE_SUFFIX;

                var repo = provider.GetRequiredService(typeof(TRepo)) as TRepo;

                var cache = provider.GetRequiredService(typeof(TestDbContextCache<TContext>))
                        as TestDbContextCache<TContext>;
                var historyCache = provider.GetRequiredService(typeof(TestDbContextCache<THistoryContext>))
                        as TestDbContextCache<THistoryContext>;

                var baseDatabaseName = TestDbContextManager<TContext>.BaseDatabaseName(config);
                var histDatabaseName = TestDbContextManager<THistoryContext>.BaseDatabaseName(config);

                if (operation == HDR_USE_READONLY)
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
            TestDbContextCache<TContext> cache,
            string instanceName, string baseDatabaseName) {
            if (cache.ContainsKey(instanceName)) {
                _logger.LogInformation($"Using existing in-memory database {baseDatabaseName}, instance = {instanceName}");
                repo.Context = cache[instanceName];
            } else {
                _logger.LogInformation($"Creating in-memory database {baseDatabaseName}, instance = {instanceName}");
                var dbContext = TestDbContextManager<TContext>.CreateInMemoryDatabase(baseDatabaseName, instanceName);
                repo.Context = dbContext;
                repo.Context.Database.EnsureCreated();
                cache.Add(instanceName, repo.Context);
            }
        }



        private void GetOrAddInMemoryHistoryDatabase(TRepo repo,
            TestDbContextCache<THistoryContext> cache,
            string instanceName, string baseDatabaseName) {
            if (cache.ContainsKey(instanceName)) {
                _logger.LogInformation($"Using existing in-memory history database {baseDatabaseName}, instance = {instanceName}");
                repo.HistoryContext = cache[instanceName];
            } else {
                _logger.LogInformation($"Creating in-memory history database {baseDatabaseName}, instance = {instanceName}");
                var dbContext = TestDbContextManager<THistoryContext>.CreateInMemoryDatabase(baseDatabaseName, instanceName);
                repo.HistoryContext = dbContext;
                repo.HistoryContext.Database.EnsureCreated();
                cache.Add(instanceName, repo.HistoryContext);
            }
        }


        private void DropInMemory(TestDbContextCache<TContext> cache,
            string instanceName) {
            if (cache.ContainsKey(instanceName)) {
                _logger.LogInformation($"Dropping in-memory history instance {instanceName} for {typeof(TContext).Name}");
                var context = cache[instanceName];
                TestDbContextManager<TContext>.DropInMemoryDatabase(context);
                cache.Remove(instanceName);
            }
        }



        private void DropInMemoryHistory(TestDbContextCache<THistoryContext> cache,
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
