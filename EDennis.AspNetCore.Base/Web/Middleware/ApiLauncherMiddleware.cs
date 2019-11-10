using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Web {
    public class ApiLauncherMiddleware<TStartup>
        where TStartup : class {

        private readonly RequestDelegate _next;
        private readonly ApiSettingsFacade _api;
        private readonly ILauncher<TStartup> _launcher;
        private readonly ILogger _logger;

        private static bool _called = false;

        public ApiLauncherMiddleware(RequestDelegate next,
            IOptionsMonitor<AppSettings> appSettings,
            ILauncher<TStartup> launcher,
            IConfiguration config,
            ILogger<ApiLauncherMiddleware<TStartup>> logger) {
            _next = next;
            _api = appSettings.CurrentValue.GetApiSettingsFacade<TStartup>(config);
            _launcher = launcher;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context) {

            //ignore, if called already or swagger meta-data processing
            if (!_called && !context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                if (_api.NeedsLaunched)
                    await _launcher.RunAsync(_api, _logger);
                _called = true;
            }
            await _next(context);
        }
    }

    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseApiLauncher<TStartup>(this IApplicationBuilder app)
            where TStartup : class {
            app.UseMiddleware<ApiLauncherMiddleware<TStartup>>();
            return app;
        }
    }


}
