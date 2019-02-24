using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider, IConfiguration config) {

            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                var header = GetTestingHeader(context);

                if (header.Key == null) {
                    context.Request.Headers.Add(HDR_USE_INMEMORY, DEFAULT_NAMED_INSTANCE);
                    header = new KeyValuePair<string, string>(HDR_USE_INMEMORY, DEFAULT_NAMED_INSTANCE);
                }

                var operation = header.Key;
                var baseInstanceName = header.Value;
                var histInstanceName = header.Value + "-hist";

                var repo = provider.GetRequiredService(typeof(TRepo)) as TRepo;
                var cache = provider.GetRequiredService(typeof(TestDbContextCache<TContext>))
                        as TestDbContextCache<TContext>;

                var baseDatabaseName = TestDbContextManager<TContext>.BaseDatabaseName(config);
                var histDatabaseName = TestDbContextManager<THistoryContext>.BaseDatabaseName(config);

                var dbDict = new Dictionary<string, string> {
                    { baseDatabaseName, baseInstanceName },
                    { histDatabaseName, histInstanceName }
                };

                if (operation == HDR_USE_READONLY)
                    throw new ApplicationException("HDR_USE_READONLY not appropriate for Writeable Repo.");
                else if (operation == HDR_USE_INMEMORY) {
                    GetOrAddInMemoryDatabases(repo, cache, dbDict);
                } else if (operation == HDR_DROP_INMEMORY)
                    DropInMemory(cache, dbDict);

            }

            await _next(context);

        }

        private void GetOrAddInMemoryDatabases(TRepo repo, TestDbContextCache<TContext> cache,
            Dictionary<string,string> dbDict) {
            foreach (var entry in dbDict) {
                if (cache.ContainsKey(entry.Value))
                    repo.Context = cache[entry.Value];
                else {
                    var dbContext = TestDbContextManager<TContext>.CreateInMemoryDatabase(entry.Key, entry.Value);
                    repo.Context = dbContext;
                    repo.Context.Database.EnsureCreated();
                    cache.Add(entry.Value, repo.Context);
                }
            }
        }



        private void DropInMemory(TestDbContextCache<TContext> cache,
            Dictionary<string, string> dbDict) {
            foreach (var entry in dbDict) {
                if (cache.ContainsKey(entry.Value)) {
                    var context = cache[entry.Value];
                    TestDbContextManager<TContext>.DropInMemoryDatabase(context);
                    cache.Remove(entry.Value);
                }
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
