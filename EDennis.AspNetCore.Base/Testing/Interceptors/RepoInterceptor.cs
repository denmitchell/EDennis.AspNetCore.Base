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
    public class RepoInterceptor<TRepo, TEntity, TContext> : Interceptor
        where TEntity : class, new()
        where TContext : DbContext
        where TRepo : SqlRepo<TEntity, TContext> {

        public RepoInterceptor(RequestDelegate next) : base(next) { }

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider, IConfiguration config) {

            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                var header = GetTestingHeader(context);

                if (header.Key == null) {
                    context.Request.Headers.Add(HDR_USE_CLONE, DEFAULT_NAMED_INSTANCE);
                    header = new KeyValuePair<string, string>(HDR_USE_CLONE, DEFAULT_NAMED_INSTANCE);
                }

                var operation = header.Key;
                var instanceName = header.Value;

                var repo = provider.GetRequiredService(typeof(TRepo)) as TRepo;
                var cache = provider.GetRequiredService(typeof(TestDbContextCache<TContext>))
                        as TestDbContextCache<TContext>;

                var baseDatabaseName = TestDbContextManager<TContext>.BaseDatabaseName(config);
                var contextName = typeof(TContext).Name;

                if (operation == HDR_USE_READONLY)
                    GetReadonlyDatabase(repo, baseDatabaseName);
                else if (operation == HDR_USE_INMEMORY)
                    GetOrAddInMemoryDatabase(repo, cache, baseDatabaseName, instanceName);
                else if (operation == HDR_USE_CLONE) {
                    var cloneConnections = provider.GetRequiredService(typeof(CloneConnections))
                        as CloneConnections;
                    GetDatabaseClone(repo, cache, cloneConnections, contextName, instanceName);
                } else if (operation == HDR_DROP_INMEMORY)
                    DropInMemory(cache, instanceName);
                else if (operation == HDR_RETURN_CLONE) {
                    var cloneConnections = provider.GetRequiredService(typeof(CloneConnections))
                        as CloneConnections;
                    ReturnClone(cache, cloneConnections, instanceName);
                }

            }

            await _next(context);

        }

        private void GetReadonlyDatabase(TRepo repo, string baseDatabaseName) {
            var dbContext = TestDbContextManager<TContext>.GetReadonlyDatabase(baseDatabaseName);
            repo.Context = dbContext;
        }

        private void GetOrAddInMemoryDatabase(TRepo repo, TestDbContextCache<TContext> cache,
            string baseDatabaseName, string instanceName) {
            if (cache.ContainsKey(instanceName))
                repo.Context = cache[instanceName];
            else {
                var dbContext = TestDbContextManager<TContext>.CreateInMemoryDatabase(baseDatabaseName, instanceName);
                repo.Context = dbContext;
                repo.Context.Database.EnsureCreated();
                cache.Add(instanceName, repo.Context);
            }
        }


        private void GetDatabaseClone(TRepo repo, TestDbContextCache<TContext> cache,
            CloneConnections cloneConnections, string contextName, string instanceName) {
            var cloneIndex = int.Parse(instanceName);
            //if (cache.ContainsKey(instanceName))
            //    repo.Context = cache[instanceName];
            //else {
                var dbContext = TestDbContextManager<TContext>.GetDatabaseClone(cloneConnections, contextName, cloneIndex);
                repo.Context = dbContext;
            //    cache.Add(instanceName, repo.Context);
            //}
        }


        private void DropInMemory(TestDbContextCache<TContext> cache, string instanceName) {
            if (cache.ContainsKey(instanceName)) {
                var context = cache[instanceName];
                TestDbContextManager<TContext>.DropInMemoryDatabase(context);
                cache.Remove(instanceName);
            }
        }


        private void ReturnClone(TestDbContextCache<TContext> cache, 
            CloneConnections cloneConnections, string instanceName) {
            //if (cache.ContainsKey(instanceName)) {
            //    var context = cache[instanceName];
                var contextName = typeof(TContext).Name;
                var cloneIndex = int.Parse(instanceName);
                var cxnTrans = cloneConnections[contextName][cloneIndex];
                TestDbContextManager<TContext>.ReturnDatabaseClone(cxnTrans);
                //cache.Remove(instanceName);
            //}
        }

    }

    public static class IApplicationBuilderExtensions_RepoInterceptorMiddleware {
        public static IApplicationBuilder UseRepoInterceptor<TRepo, TEntity, TContext>(this IApplicationBuilder app)
        where TEntity : class, new()
        where TContext : DbContext
        where TRepo : SqlRepo<TEntity, TContext> {
            app.UseMiddleware<RepoInterceptor<TRepo, TEntity, TContext>>();
            return app;
        }
    }


}
