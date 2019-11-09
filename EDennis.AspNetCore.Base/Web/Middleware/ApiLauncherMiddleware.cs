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
        private readonly ILogger _logger;

        private static bool _called = false;

        public ApiLauncherMiddleware(RequestDelegate next,
            IOptionsMonitor<AppSettings> appSettings, IConfiguration config,
            ILogger<ApiLauncherMiddleware<TStartup>> logger) {
            _next = next;
            _api = appSettings.CurrentValue.GetApiSettingsFacade<TStartup>(config);
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context) {
            //ignore, if called already or swagger meta-data processing
            if (!_called && !context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                Type classType = typeof(Launcher<>);
                Type[] typeParams = new Type[] { typeof(TStartup) };
                Type constructedType = classType.MakeGenericType(typeParams);

                if (_api.NeedsLaunched) {
                    var launcher = (Launcher<TStartup>)Activator.CreateInstance(constructedType);
                    await launcher.RunAsync(_api, _logger);
                }
            }
            await _next(context);
            _called = true;
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
