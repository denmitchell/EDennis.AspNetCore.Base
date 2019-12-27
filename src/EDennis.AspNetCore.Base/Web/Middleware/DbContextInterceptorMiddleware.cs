using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {
    public class DbContextInterceptorMiddleware<TContext>
        where TContext : DbContext {

        protected readonly RequestDelegate _next;
        protected readonly IOptionsMonitor<DbContextInterceptorSettings<TContext>> _settings;
        protected readonly DbConnectionCache<TContext> _cache;
        protected readonly ILogger<DbContextInterceptorMiddleware<TContext>> _logger;

        public bool Bypass { get; } = false;

        public DbContextInterceptorMiddleware(RequestDelegate next,
            IOptionsMonitor<DbContextInterceptorSettings<TContext>> settings,
            DbConnectionCache<TContext> cache,
            ILogger<DbContextInterceptorMiddleware<TContext>> logger,
            IWebHostEnvironment env) {
            _next = next;
            _settings = settings;
            _cache = cache;
            _logger = logger;
            if (env.EnvironmentName == "Production")
                Bypass = true;
        }


        public async Task InvokeAsync(HttpContext context,
            DbContextOptionsProvider<TContext> dbContextOptionsProvider) {


            var req = context.Request;
            var enabled = (_settings.CurrentValue?.Enabled ?? new bool?(false)).Value;

            if (Bypass || !enabled || !req.Path.StartsWithSegments(new PathString("/swagger"))) {


                var method = req.Method;
                var instance = req.Query[Constants.TESTING_INSTANCE_KEY][0];

                //Handle reset
                //NOTE: Resets are sent as standalone requests with a special "Reset" Http Method
                //          or a special "X-Testing-Reset" Request Header or query key
                //      and a query string: instance=some_instance_name
                if (method == Constants.RESET_METHOD 
                    || context.Request.ContainsHeaderOrQueryKey(Constants.TESTING_RESET_KEY, out string _)) {
                    if (_cache.ContainsKey(instance)) {
                        if (_settings.CurrentValue.IsInMemory) {
                            _cache[instance] = DbConnectionManager.GetInMemoryDbConnection<TContext>();
                            _logger.LogInformation("Db Interceptor resetting {DbContext}-{Instance}", typeof(TContext).Name, instance);
                        } else {
                            _cache[instance].IDbTransaction.Rollback();
                            _logger.LogInformation("Db Interceptor rolling back {DbContext}-{Instance}", typeof(TContext).Name, instance);
                        }
                        DbConnectionManager.Reset<TContext>(_cache[instance].IDbConnection, _settings.CurrentValue);
                    }
                    return;
                }

                _logger.LogInformation("Db Interceptor handling request: {RequestPath}", context.Request.Path);

                var instanceName = instance ?? MiddlewareUtils.ResolveUser(context, _settings.CurrentValue.InstanceNameSource, "DbContextInterceptor InstanceName");

                var cachedCxn = _cache.GetOrAdd(instanceName,(key)
                    => DbConnectionManager.GetDbConnection(_settings.CurrentValue));
                
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