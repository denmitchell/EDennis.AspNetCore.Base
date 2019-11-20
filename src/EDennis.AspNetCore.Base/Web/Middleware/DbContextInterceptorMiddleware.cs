using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {
    public class DbContextInterceptorMiddleware<TContext>
        where TContext : DbContext {

        protected readonly RequestDelegate _next;
        public bool Bypass { get; } = false;

        public DbContextInterceptorMiddleware(RequestDelegate next) {
            _next = next;
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env == "Production")
                Bypass = true;
        }

        ILogger _logger;

        public async Task InvokeAsync(HttpContext context,
            IOptionsMonitor<DbContextInterceptorSettings<TContext>> settings,
            DbContextOptionsProvider<TContext> dbContextOptionsProvider,
            DbConnectionCache<TContext> cache,
            ILogger<DbContextInterceptorMiddleware<TContext>> logger) {


            if (!Bypass && !context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                _logger = logger;

                var method = context.Request.Method;
                var instance = context.Request.Query[Constants.TESTING_INSTANCE_KEY][0];

                //Handle reset
                //NOTE: Resets are sent as standalone requests with a special "Reset" Http Method
                //      and a query string: instance=some_instance_name
                if (method == Constants.RESET_METHOD) {
                    if (cache.ContainsKey(instance)) {
                        if (settings.CurrentValue.IsInMemory) {
                            cache[instance] = DbConnectionManager.GetInMemoryDbConnection<TContext>();
                            _logger.LogInformation("Db Interceptor resetting {DbContext}-{Instance}", typeof(TContext).Name, instance);
                        } else {
                            cache[instance].IDbTransaction.Rollback();
                            _logger.LogInformation("Db Interceptor rolling back {DbContext}-{Instance}", typeof(TContext).Name, instance);
                        }
                        DbConnectionManager.Reset<TContext>(cache[instance].IDbConnection, settings.CurrentValue);
                    }
                    return;
                }

                _logger.LogInformation("Db Interceptor handling request: {RequestPath}", context.Request.Path);

                var instanceName = instance ?? MiddlewareUtils.ResolveUser(context, settings.CurrentValue.InstanceNameSource, "DbContextInterceptor InstanceName");

                var cachedCxn = cache.GetOrAdd(instanceName,(key)
                    => DbConnectionManager.GetDbConnection(settings.CurrentValue));
                
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