using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {
    public class UserLoggerMiddleware {

        protected readonly RequestDelegate _next;

        public UserLoggerMiddleware(RequestDelegate next) {
            _next = next;
        }

        ILogger _logger;

        public async Task InvokeAsync(HttpContext context,
            IOptionsMonitor<UserLoggerSettings> settings,
            IScopedLogger scopedLogger,
            IScopedLoggerAssignments loggerAssignments,
            ILogger<UserLoggerMiddleware> logger) {


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


                //update ScopedLogger for the user, if the user has an assigned logger/level
                var user = MiddlewareUtils.ResolveUser(context, settings.CurrentValue.UserSource, "UserLogger.User");
                if (loggerAssignments.Assignments.TryGetValue(user, out LogLevel newLevel)) {
                    _logger.LogInformation("UserLogger setting logging level to {LogLevel} for {User}", newLevel, user);
                    scopedLogger.LogLevel = newLevel;
                }

            }

            await _next(context);

        }


    }

    public static partial class RequestExtensions {
        public static bool ContainsHeaderOrQueryKey(this HttpRequest request, string key, out string value) {
            if (request.Headers.ContainsKey(key)) {
                value = request.Headers[key];
                return true;
            } else if (request.Query.ContainsKey(key)) {
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
            app.UseMiddleware<UserLoggerMiddleware>();
            return app;
        }
    }

}