using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public class ScopedConfigurationMiddleware {

        protected readonly RequestDelegate _next;
        protected readonly IOptionsMonitor<ScopedConfigurationSettings> _settings;
        protected readonly IConfiguration _config;

        public ScopedConfigurationMiddleware(RequestDelegate next,
            IOptionsMonitor<ScopedConfigurationSettings> settings,
            IConfiguration config) {
            _next = next;
            _settings = settings;
            _config = config;

        }

        public async Task InvokeAsync(HttpContext context) {

            var req = context.Request;
            var enabled = (_settings.CurrentValue?.Enabled ?? new bool?(false)).Value;

            if (req.Path.StartsWithSegments(new PathString("/swagger"))) {
                await _next(context);
            } else {

                var method = req.Method;

                if (method == Constants.CONFIG_METHOD 
                    || req.ContainsPathHeaderOrQueryKey(Constants.CONFIG_QUERY_KEY, out string _)) {

                    if (!enabled)
                        return;

                    req.EnableBuffering();
                    var body = MiddlewareUtils.StreamToString(req.Body);

                    var config = new ConfigurationBuilder()
                        .AddJsonStream(MiddlewareUtils.StringToStream(body))
                        .Build();


                    var configDict = config.Flatten();
                    foreach (var key in configDict.Keys)
                        _config[key] = configDict[key];

                    return;

                }

            }
            await _next(context);
        }


    }



    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseScopedConfiguration(this IApplicationBuilder app)
        {
            app.UseMiddleware<ScopedConfigurationMiddleware>();
            return app;
        }
    }

}