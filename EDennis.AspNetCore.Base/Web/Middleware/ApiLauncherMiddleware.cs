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
        private readonly IOptionsMonitor<AppSettings> _appSettings;
        private readonly IConfiguration _config;
        private readonly LauncherSettingsDictionary _projectPorts;
        private readonly ILogger _logger;

        private static bool _called = false;

        public ApiLauncherMiddleware(RequestDelegate next, 
            IOptionsMonitor<AppSettings> appSettings, IConfiguration config,
            LauncherSettingsDictionary projectPorts,
            ILogger<ApiLauncherMiddleware<TStartup>> logger) {
            _next = next;
            _appSettings = appSettings;
            _config = config;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context) {
            //ignore, if called already or swagger meta-data processing
            if (!_called && !context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                var apis = _appSettings.CurrentValue.Apis;

                Type classType = typeof(Launcher<>);
                Type[] typeParams = new Type[] { typeof(TStartup) };
                Type constructedType = classType.MakeGenericType(typeParams);

                foreach (var key in apis.Keys) {
                    var api = apis[key];
                    var projectName = api.ProjectName;
                    if (api.ApiLauncher) {
                        var launcher = (Launcher<TStartup>)Activator.CreateInstance(constructedType);
                        await launcher.RunAsync(_projectPorts, _logger);
                        var projectPortInfo = 
                        _config[$"Apis:{key}:HttpsPort"] = _projectPorts[projectName];
                        ,
      "Host": "localhost",
      "Version": 1.0,
      "HttpsPort": 6542,
      "HttpPort": 6543, "]
                    }
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
