using EDennis.AspNetCore.Base.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {
    public class ScopedLoggerMiddleware {

        protected readonly RequestDelegate _next;

        public ScopedLoggerMiddleware(RequestDelegate next) {
            _next = next;
        }

        ILogger _logger;

        public async Task InvokeAsync(HttpContext context,
            //IOptionsMonitor<UserLoggerSettings> settings,
            //IScopedLogger scopedLogger,
            IScopedLoggerAssignments loggerAssignments,
            ILogger<ScopedLoggerMiddleware> logger) {


            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                var query = context.Request.Query;
                var headers = context.Request.Headers;

                _logger = logger;

                //handle new logger assignment or clearing of existing assignment
                if (context.Request.ContainsHeaderOrQueryKey(
                        Constants.SET_SCOPEDLOGGER_KEY, out string setValue)) {
                    var v = setValue.Split('|');
                    var userKey = v[0];
                    var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), v[1]);

                    _logger.LogInformation("UserLogger middleware assigning {User} to {LogLevel}", userKey, logLevel);
                    loggerAssignments.Assignments.AddOrUpdate(userKey, logLevel, (u, l) => l);

                }else if (context.Request.ContainsHeaderOrQueryKey(
                        Constants.CLEAR_SCOPEDLOGGER_KEY, out string user1)) {

                    _logger.LogInformation("UserLogger middleware clearing {User}", user1);
                    loggerAssignments.Assignments.TryRemove(user1, out LogLevel _);
                }


            }

            await _next(context);

        }

    }

    public static partial class RequestExtensions {
        public static bool ContainsHeaderOrQueryKey(this HttpRequest request, string key, out string value) {
            if (request.Headers.Keys.Any(k=>k.Equals(key,StringComparison.OrdinalIgnoreCase))) {
                value = request.Headers[key];
                return true;
            } else if (request.Query.Keys.Any(k => k.Equals(key, StringComparison.OrdinalIgnoreCase))) {
                value = request.Query[key];
                return true;
            } else {
                value = null;
                return false;
            }
        }
    }


    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseUserLogger(this IApplicationBuilder app) { 
            app.UseMiddleware<ScopedLoggerMiddleware>();
            return app;
        }
    }

}