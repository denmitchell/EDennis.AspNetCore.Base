using EDennis.AspNetCore.Base.Testing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {
    public class ExceptionLoggerMiddleware {

        protected readonly RequestDelegate _next;
        protected readonly ILogger<ExceptionLoggerMiddleware> _logger;
        protected readonly IOptionsMonitor<ExceptionLoggerSettings> _settings;
        protected readonly IWebHostEnvironment _env;

        public ExceptionLoggerMiddleware(RequestDelegate next,
            ILogger<ExceptionLoggerMiddleware> logger,
            IOptionsMonitor<ExceptionLoggerSettings> settings,
            IWebHostEnvironment env) {
            _next = next;
            _logger = logger;
            _settings = settings;
            _env = env;

        }

        public async Task InvokeAsync(HttpContext context,
            IServiceProvider serviceProvider) {

            var req = context.Request;
            var settings = _settings.CurrentValue;

            var middlewareEnabled = settings.Enabled;

            if (!middlewareEnabled || req.Path.StartsWithSegments(new PathString("/swagger"))) {
                await _next(context);
            } else {
                try {
                    await _next(context);
                } catch (Exception ex) {

                    var scopeProperties = serviceProvider.GetRequiredService<IScopeProperties>();

                    List<KeyValuePair<string, object>> logScope = new List<KeyValuePair<string, object>> {
                        KeyValuePair.Create("User", (object)scopeProperties.User),
                        KeyValuePair.Create("RequestPath", (object)context.Request.Path.ToString()),
                        KeyValuePair.Create("StackTrace", (object)ex.StackTrace)
                    };

                    using var _ = _logger.BeginScope(logScope);
                    _logger.LogError(ex, "For {User}, attempting {RequestPath}, Exception: {Message}", 
                        scopeProperties.User, context.Request.Path.ToString(), ex.Message);
                    throw;
                }

            }
        }

    }


    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseExceptionLogger(this IApplicationBuilder app) {
            app.UseMiddleware<ExceptionLoggerMiddleware>();
            return app;
        }
    }

}