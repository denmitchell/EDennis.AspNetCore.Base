using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Logging {


    public partial class ScopedLoggingMiddleware<T> {

        public const string HDR_LOGGER_PREFIX = "X-Logging-";
        public const string HDR_USE_SCOPEDLOGGER = HDR_LOGGER_PREFIX + "UseScopedLogger";


        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        public ScopedLoggingMiddleware(RequestDelegate next, IOptions<ScopedLoggingOptions> options) {
            _next = next;
            _loggerFactory = options.Value.LoggerFactory;

        }

        public async Task Invoke(HttpContext context, IServiceProvider provider, IWebHostEnvironment env) {

            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                var values = GetScopedLoggingValues(context);

                if (values != null) {
                    var currentProject = env.ApplicationName;

                    var currentAssembly = GetType().Assembly.FullName;
                    currentAssembly = currentAssembly.Substring(0, currentAssembly.IndexOf(','));

                    if (values.Contains(currentProject)
                        || values.Contains(currentAssembly)
                        || values.Select(x => x.ToLower()).Contains(context.Request.Path.ToString().ToLower())
                        || values.Contains(context.User?.Identity?.Name ?? "3f1b6266-49ad-4b21-8fde-0137a49531b7")
                        || values.Contains("*")
                        ) {

                        ScopedLogger<T> scopedLogger = provider.GetRequiredService<ScopedLogger<T>>();
                        scopedLogger.Enabled = true;

                        ILogger<T> logger = _loggerFactory.CreateLogger<T>();

                        scopedLogger.Logger = logger; 

                    }

                }

                await _next(context);

            }

        }

        /// <summary>
        /// Determines if the given header exists
        /// </summary>
        /// <param name="context">provides access to the Request headers</param>
        /// <returns></returns>
        protected string[] GetScopedLoggingValues(HttpContext context) {

            var values = context.Request.Headers
                                .Where(h => h.Key == HDR_USE_SCOPEDLOGGER)
                                .Select(h => h.Value.ToString())
                                .FirstOrDefault()
                                ?.Split(',');

            return values;
        }


    }

}
