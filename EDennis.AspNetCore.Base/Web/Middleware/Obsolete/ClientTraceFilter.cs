using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web
{

    [Obsolete("Use ScopePropertiesMiddleware with AppendHostPath option")]
    public class ClientTraceFilter {

        readonly RequestDelegate _next;

        public ClientTraceFilter(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, 
            IServiceProvider provider, IWebHostEnvironment hostingEnvironment) {

            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                var scopeProperties = provider.GetRequiredService(typeof(ScopeProperties22)) as ScopeProperties22;

                context.Request.Headers.TryGetValue("X-ClientTrace", out StringValues clientTraceEntries);

                if (!StringValues.IsNullOrEmpty(clientTraceEntries))
                    clientTraceEntries = StringValues.Concat(clientTraceEntries, hostingEnvironment.ApplicationName);
                else
                    clientTraceEntries = new StringValues(hostingEnvironment.ApplicationName);

                scopeProperties.OtherProperties.Add("X-ClientTrace", clientTraceEntries);

            }
            await _next(context);

        }
    }

    public static class IApplicationBuilderExtensionsForClientTraceFilter {
        public static IApplicationBuilder UseClientTrace(this IApplicationBuilder app) {
            app.UseMiddleware<ClientTraceFilter>();
            return app;
        }
    }


}