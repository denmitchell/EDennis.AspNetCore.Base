﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This middleware is designed to retrieve and store important data for later processing by
    /// ApiClients, Repos, and perhaps other classes.  In particular, the middleware retrieves
    /// and stores:
    /// <list type="bullet">
    /// <item>User name from any of a variety of sources</item>
    /// <item>Other Claims that match a prespecified pattern</item>
    /// <item>Other Headers that match a prespecified pattern</item>
    /// </list>
    /// </summary>
    public class ScopePropertiesMiddleware {

        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<ScopePropertiesSettings> _settings;

        public ScopePropertiesMiddleware(RequestDelegate next, 
            IOptionsMonitor<ScopePropertiesSettings> settings) {
            _next = next;
            _settings = settings;
        }

        public async Task InvokeAsync(HttpContext context, IScopeProperties scopeProperties) {


            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {


                var settings = _settings.CurrentValue;

                //update the Scope Properties User with identity, claim or header data
                scopeProperties.User = MiddlewareUtils.ResolveUser(context, settings.UserSource, "ScopeProperties.User");


                //copy all headers to ScopeProperties headers
                if(settings.CopyHeaders)
                    context.Request.Headers
                        .ToList()
                        .ForEach(h => scopeProperties.Headers
                        .Add(h.Key, h.Value.ToArray()));


                //append the host path to a ScopeProperties header, if configured 
                if (settings.AppendHostPath)
                        scopeProperties.Headers.Add(Constants.HOSTPATH_KEY,
                            $"{context.Request.Headers[Constants.HOSTPATH_KEY].ToString()}>" +
                            $"{context.Request.Headers["Host"]}");


                //add user claims
                if (settings.CopyClaims && context.User?.Claims != null) 
                    scopeProperties.Claims = context.User.Claims.ToArray();
                

            }
            await _next(context);

        }


    }

    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseScopeProperties(this IApplicationBuilder app) {
            app.UseMiddleware<ScopePropertiesMiddleware>();
            return app;
        }
    }


}