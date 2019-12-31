using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public DbContextInterceptorMiddleware(RequestDelegate next,
            IOptionsMonitor<DbContextInterceptorSettings<TContext>> settings,
            DbConnectionCache<TContext> cache,
            ILogger<DbContextInterceptorMiddleware<TContext>> logger) {
            _next = next;
            _settings = settings;
            _cache = cache;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context,
            DbContextOptionsProvider<TContext> dbContextOptionsProvider) {


            var req = context.Request;
            var enabled = (_settings.CurrentValue?.Enabled ?? new bool?(false)).Value;

            if (!enabled || req.Path.StartsWithSegments(new PathString("/swagger"))) {
                await _next(context);
            } else {


                var method = req.Method;

                //get instance name
                string instance;
                if (req.Query.ContainsKey(Constants.TESTING_INSTANCE_KEY))
                    instance = req.Query[Constants.TESTING_INSTANCE_KEY][0];
                else
                    instance = MiddlewareUtils.ResolveUser(context, _settings.CurrentValue.InstanceNameSource, "DbContextInterceptor InstanceName");


                _logger.LogInformation("DbContextInterceptor handling request: {RequestPath}", context.Request.Path);


                //add or retrieve cached connection
                var cachedCxn = _cache.GetOrAdd(instance, _settings.CurrentValue, dbContextOptionsProvider);

                await _next(context);


                //Handle reset
                //NOTE: Resets are sent with the last request that is part of the same transaction
                //      or as part of a standalone/throw-away GET request
                if (context.Request.ContainsHeaderOrQueryKey(Constants.TESTING_RESET_KEY, out _)) 
                    DbConnectionManager.Reset(_cache, instance, _settings.CurrentValue, _logger);
                

            }
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