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
    public class RepoInterceptor<TRepo, TEntity, TContext> : Interceptor
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : WriteableRepo<TEntity, TContext> {

        public RepoInterceptor(RequestDelegate next) : base(next) { }

        ILogger _logger;

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider, 
            IConfiguration config,
            ILogger<RepoInterceptor<TRepo, TEntity, TContext>> logger) {

            _logger = logger;

            _logger.LogInformation($"RepoInterceptor handling request: {context.Request.Path}");


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

                _logger.LogInformation($"RepoInterceptor processing header {header.Key}: {header.Value}");

                var operation = header.Key;
                var baseInstanceName = header.Value;

                var repo = provider.GetRequiredService(typeof(TRepo)) as TRepo;
                var cache = provider.GetRequiredService(typeof(TestDbContextOptionsCache<TContext>))
                        as TestDbContextOptionsCache<TContext>;

                var baseDatabaseName = TestDbContextManager<TContext>.BaseDatabaseName(config);

                if (operation == HDR_USE_RELATIONAL) {
                    _logger.LogInformation($"Using Relational database");
                } else if (operation == HDR_USE_READONLY)
                    throw new ApplicationException("HDR_USE_READONLY not appropriate for Writeable Repo.");
                else if (operation == HDR_USE_INMEMORY) {
                    GetOrAddInMemoryDatabase(repo, cache, baseInstanceName, baseDatabaseName);
                } else if (operation == HDR_DROP_INMEMORY)
                    DropInMemory(cache, baseInstanceName);

            }

            await _next(context);

        }

        private void GetOrAddInMemoryDatabase(TRepo repo, TestDbContextOptionsCache<TContext> cache,
            string instanceName, string baseDatabaseName) {
            if (cache.ContainsKey(instanceName)) {
                TestDbContextManager<TContext>.CreateInMemoryDatabase(cache[instanceName], out TContext context);
                repo.Context = context;
                _logger.LogInformation($"Using existing in-memory database {baseDatabaseName}, instance = {instanceName}");
            } else {
                _logger.LogInformation($"Creating in-memory database {baseDatabaseName}, instance = {instanceName}");
                TestDbContextManager<TContext>.CreateInMemoryDatabase(baseDatabaseName, instanceName, 
                    out DbContextOptions<TContext> options, out TContext context);
                repo.Context = context;
                repo.Context.Database.EnsureCreated();
                cache.Add(instanceName, options);
            }
        }

        private void DropInMemory(TestDbContextOptionsCache<TContext> cache, string instanceName) {
            if (cache.ContainsKey(instanceName)) {
                _logger.LogInformation($"Dropping in-memory history instance {instanceName} for {typeof(TContext).Name}");
                var options = cache[instanceName];
                TestDbContextManager<TContext>.DropInMemoryDatabase(options);
                cache.Remove(instanceName);
            }
        }




    }

    public static partial class IApplicationBuilderExtensions_RepoInterceptorMiddleware {
        public static IApplicationBuilder UseRepoInterceptor<TRepo, TEntity, TContext>(this IApplicationBuilder app)
                where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : WriteableRepo<TEntity, TContext> {
            app.UseMiddleware<RepoInterceptor<TRepo, TEntity, TContext>>();
            return app;
        }
    }


}
