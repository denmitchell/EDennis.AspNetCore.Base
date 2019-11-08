using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public class ApiLauncherMiddleware {
        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<AppSettings> _appSettings;
        private readonly IConfiguration _config;
        public ApiLauncherMiddleware(RequestDelegate next, 
            IOptionsMonitor<AppSettings> appSettings, IConfiguration config ) {
            _next = next;
            _appSettings = appSettings;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context) {
            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                var apis = _appSettings.CurrentValue.Apis;
                foreach(var key in apis.Keys) {
                    var api = apis[key];
                    if (api.ApiLauncher) {

                    }
                }
            }
            await _next(context);
        }
    }
}
