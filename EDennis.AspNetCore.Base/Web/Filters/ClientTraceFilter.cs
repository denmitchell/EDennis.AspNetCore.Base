using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {

    public class ClientTraceFilter {

        RequestDelegate _next;

        public ClientTraceFilter(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, 
            IServiceProvider provider, IHostingEnvironment hostingEnvironment) {

            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                var scopeProperties = provider.GetRequiredService(typeof(ScopeProperties)) as ScopeProperties;

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