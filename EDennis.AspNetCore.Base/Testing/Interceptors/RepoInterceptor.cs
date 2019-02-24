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
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : WriteableRepo<TEntity, TContext> {

        public RepoInterceptor(RequestDelegate next) : base(next) { }

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider, IConfiguration config) {

            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                var header = GetTestingHeader(context);

                if (header.Key == null) {
                    context.Request.Headers.Add(HDR_USE_INMEMORY, DEFAULT_NAMED_INSTANCE);
                    header = new KeyValuePair<string, string>(HDR_USE_INMEMORY, DEFAULT_NAMED_INSTANCE);
                }

                var operation = header.Key;
                var baseInstanceName = header.Value;

                var repo = provider.GetRequiredService(typeof(TRepo)) as TRepo;
                var cache = provider.GetRequiredService(typeof(TestDbContextCache<TContext>))
                        as TestDbContextCache<TContext>;

                var baseDatabaseName = TestDbContextManager<TContext>.BaseDatabaseName(config);

                if (operation == HDR_USE_READONLY)
                    throw new ApplicationException("HDR_USE_READONLY not appropriate for Writeable Repo.");
                else if (operation == HDR_USE_INMEMORY) {
                    GetOrAddInMemoryDatabase(repo, cache, baseInstanceName, baseDatabaseName);
                } else if (operation == HDR_DROP_INMEMORY)
                    DropInMemory(cache, baseInstanceName);

            }

            await _next(context);

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

        private void DropInMemory(TestDbContextCache<TContext> cache, string instanceName) {
            if (cache.ContainsKey(instanceName)) {
                var context = cache[instanceName];
                TestDbContextManager<TContext>.DropInMemoryDatabase(context);
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
