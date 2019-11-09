using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
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
        private readonly IOptionsMonitor<AppSettings> _appSettings;

        public ScopePropertiesMiddleware(RequestDelegate next, 
            IOptionsMonitor<AppSettings> appSettings) {
            _next = next;
            _appSettings = appSettings;
        }

        public async Task InvokeAsync(HttpContext context, IScopeProperties scopeProperties) {


            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                if (context.Request.Headers.ContainsKey(Constants.ROLLBACK_HEADER_KEY))
                    scopeProperties.NewConnection = true;
                else if (context.Request.Query.ContainsKey(Constants.ROLLBACK_QUERY_KEY))
                    scopeProperties.NewConnection = true;


                var appSettings = _appSettings.CurrentValue;

                //update the Scope Properties User with identity, claim or header data
                scopeProperties.User = appSettings.ScopeProperties.UserSource switch
                {
                    UserSource.JwtNameClaim => GetClaimValue(context, JwtClaimTypes.Name),
                    UserSource.OasisNameClaim => GetClaimValue(context, ClaimTypes.Name),
                    UserSource.OasisEmailClaim => GetClaimValue(context, ClaimTypes.Email),
                    UserSource.JwtPreferredUserNameClaim => GetClaimValue(context, JwtClaimTypes.PreferredUserName),
                    UserSource.JwtSubjectClaim => GetClaimValue(context, JwtClaimTypes.Subject),
                    UserSource.JwtEmailClaim => GetClaimValue(context, JwtClaimTypes.Email),
                    UserSource.JwtPhoneClaim => GetClaimValue(context, JwtClaimTypes.PhoneNumber),
                    UserSource.JwtClientIdClaim => GetClaimValue(context, JwtClaimTypes.ClientId),
                    UserSource.SessionId => context.Session?.Id,
                    UserSource.XUserHeader => GetHeaderValue(context, Constants.USER_HEADER),
                    _ => null
                };



                //copy headers to ScopeProperties headers, based upon the provided match expession
                context.Request.Headers
                    .Where(h=> appSettings.ScopeProperties.Headers.Any(s=> s == h.Key))
                    .ToList()
                    .ForEach(h => scopeProperties.Headers
                    .Add(h.Key, h.Value.ToArray()));


                //append the host path to a ScopeProperties header, if configured 
                if (appSettings.ScopeProperties.AppendHostPath)
                        scopeProperties.Headers.Add(Constants.HOSTPATH_HEADER,
                            $"{context.Request.Headers[Constants.HOSTPATH_HEADER].ToString()}>" +
                            $"{context.Request.Headers["Host"]}");


                //add user claims, if configured
                if (context.User?.Claims != null) {
                    scopeProperties.Claims = context.User.Claims
                        .Where(c => appSettings.ScopeProperties.ClaimTypes.Any(s => s == c.Type))
                        .ToArray();
                }

            }
            await _next(context);

        }


        private string GetClaimValue(HttpContext context, string claimType)
            => context.User?.Claims?.FirstOrDefault(x 
                => x.Type.Equals(claimType,StringComparison.OrdinalIgnoreCase))?.Value;


        private string GetHeaderValue(HttpContext context, string headerKey)
            => context.Request?.Headers?.FirstOrDefault(x
                => x.Key.Equals(headerKey, StringComparison.OrdinalIgnoreCase)).Value.ToString();


    }

    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseScopeProperties(this IApplicationBuilder app) {
            app.UseMiddleware<ScopePropertiesMiddleware>();
            return app;
        }
    }


}