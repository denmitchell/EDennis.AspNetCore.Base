using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    
    /// <summary>
    /// This middleware will transform headers into user claims per configuration settings.
    /// 
    /// This middleware is designed to be used immediately BEFORE or AFTER UseAuthentication() or
    /// UseAuthorization().  All header/claims configured for PostAuthentication will be ignored if
    /// the User is not authenticated.
    /// 
    /// When used after UseAuthorization, the claims are merely extra claims that can be used for
    /// purposes other than all-or-nothing access to a protected resource (e.g., using a claim's
    /// value to filter data requests.)
    /// </summary>
    public class HeadersToClaimsMiddleware {
        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<AppSettings> _appSettings;
        public HeadersToClaimsMiddleware(RequestDelegate next, IOptionsMonitor<AppSettings> appSettings) {
            _next = next;
            _appSettings = appSettings;
        }

        public async Task InvokeAsync(HttpContext context) {
            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {


                if (context.User != null) {

                    var htc = _appSettings.CurrentValue.Mappings?.HeadersToClaims;
                    var claims = new List<Claim>();
                    bool claimsAdded = false;

                    //pre-authentication -- simply add the claim
                    if (context.User.Identities.Count(i => i.IsAuthenticated) == 0) {
                        var headersToClaims = htc?.PreAuthentication;
                        if (headersToClaims != null)
                            foreach (var key in headersToClaims.Keys) {
                                claims.Add(new Claim(key, headersToClaims[key]));
                                claimsAdded = true;
                            }
                    //post-authentication -- only add the claim if the user doesn't already have it
                    } else {
                        var headersToClaims = htc?.PostAuthentication;
                        if (headersToClaims != null)
                            foreach (var key in headersToClaims.Keys)
                                if (!context.User.HasClaim(c => c.Type.Equals(key, StringComparison.OrdinalIgnoreCase))) {
                                    claims.Add(new Claim(key, headersToClaims[key]));
                                    claimsAdded = true;
                                }
                    }

                    //if any claims were added, add a new claims identity with those claims
                    if (claimsAdded) { 
                        var appIdentity = new ClaimsIdentity(claims);
                        context.User.AddIdentity(appIdentity);
                    }
                }
            }
            await _next(context);
        }
    }

    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseHeadersToClaims(this IApplicationBuilder app){
            app.UseMiddleware<HeadersToClaimsMiddleware>();
            return app;
        }
    }

}
