using EDennis.AspNetCore.Base.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {
    public class ScopedTraceLoggerMiddleware {

        protected readonly RequestDelegate _next;
        protected readonly ILogger<ScopedTraceLoggerMiddleware> _logger;
        protected readonly IOptionsMonitor<ScopedTraceLoggerSettings> _settings;
        protected readonly Func<IScopeProperties, string> getScopedTraceLoggerKey;
        public ScopedTraceLoggerMiddleware(RequestDelegate next,
            IOptionsMonitor<ScopedTraceLoggerSettings> settings,
            ILogger<ScopedTraceLoggerMiddleware> logger) {
            _next = next;
            _logger = logger;
            _settings = settings;
            getScopedTraceLoggerKey = GetScopedTraceLoggerKey(); //cache function for faster retrieval.
        }

        public async Task InvokeAsync(HttpContext context, IScopeProperties scopeProperties) {


            var req = context.Request;
            var enabled = (_settings.CurrentValue?.Enabled ?? new bool?(false)).Value;

            if (!enabled || req.Path.StartsWithSegments(new PathString("/swagger"))) {
                await _next(context);
            } else {

                scopeProperties.ScopedTraceLoggerKey = getScopedTraceLoggerKey(scopeProperties);

                var query = req.Query;
                var headers = req.Headers;

                //handle new registrations and un-registrations
                if (req.ContainsPathHeaderOrQueryKey(
                        Constants.SET_SCOPEDLOGGER_KEY, out string keyToRegister)) {
                    ScopedTraceLoggerAttribute.RegisteredKeys.AddOrUpdate(keyToRegister.ToString(), true, (key, value) => true);

                    _logger.LogInformation("ScopedTraceLogging middleware registering {Key}", keyToRegister);

                } else if (req.ContainsPathHeaderOrQueryKey(
                        Constants.CLEAR_SCOPEDLOGGER_KEY, out string keyToUnregister)) {
                    ScopedTraceLoggerAttribute.RegisteredKeys.TryRemove(keyToUnregister.ToString(), out bool _);

                    _logger.LogInformation("ScopedTraceLogging middleware un-registering {Key}", keyToUnregister);
                }

                await _next(context);

            }


        }

        public Func<IScopeProperties,string> GetScopedTraceLoggerKey() {
            return _settings.CurrentValue.AssignmentKeySource switch
            {
                AssignmentKeySource.User => (sp) => sp.User,
                AssignmentKeySource.Header => (sp) => sp.Headers.FirstOrDefault(c => c.Key == _settings.CurrentValue.AssignmentKeyName).Value,
                AssignmentKeySource.Claim => (sp) => sp.Claims.FirstOrDefault(c => c.Type == _settings.CurrentValue.AssignmentKeyName)?.Value,
                AssignmentKeySource.OtherProperty => (sp) => sp.OtherProperties.FirstOrDefault(c => c.Key == _settings.CurrentValue.AssignmentKeyName).Value.ToString(),
                _ => (sp) => sp.User,
            };
        }

    }

    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseScopedTraceLogger(this IApplicationBuilder app) { 
            app.UseMiddleware<ScopedTraceLoggerMiddleware>();
            return app;
        }
    }

}