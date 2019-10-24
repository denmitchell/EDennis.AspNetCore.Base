using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web
{
    [Obsolete("Use ScopePropertiesMiddleware, instead")]
    public class UserFilter {

        private readonly RequestDelegate _next;
        private readonly UserFilterOptions _options;

        public UserFilter(RequestDelegate next, IOptions<UserFilterOptions> options) {
            _next = next;
            _options = options?.Value;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider) {



            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                var scopeProperties = provider.GetRequiredService(typeof(ScopeProperties22)) as ScopeProperties22;

                string sysUser = null;
                var sources = _options.Sources.ToArray();
                var nameClaims = _options.SysUserClaimTypes.ToArray();
                string nameClaimType = null;
                var otherClaims = _options.OtherUserClaimsToAddToScopeProperties.ToArray();

                context.Request.Headers.TryGetValue("X-User", out StringValues userHeader);

                foreach (var type in sources) {

                    if (type == SysUserSource.ExistingScopeProperties && scopeProperties.User != null) {
                        sysUser = scopeProperties.User;
                        break;
                    }

                    if (type == SysUserSource.RequestHeader) {
                        if (userHeader.Count() > 0) {
                            sysUser = userHeader.LastOrDefault();
                            break;
                        }
                    }

                    if (type == SysUserSource.UserPrincipleName
                        && context.User.Identity.Name != null) {
                        sysUser = context.User.Identity.Name;
                        break;
                    }

                    if (type == SysUserSource.UserClaim) {
                        foreach(var nameClaim in nameClaims) {
                            var claim = context.User.Claims.Where(x => x.Type == nameClaim)?.FirstOrDefault();
                            if (claim != null) {
                                nameClaimType = claim.Type;
                                sysUser = claim.Value;
                                break;
                            }
                        }
                    }
                }

                if (sysUser != null) {
                    scopeProperties.User = sysUser;
                    if (_options.AddXUserHeaderForPropagation) {
                        if (userHeader.Count > 0) {
                            context.Request.Headers.Remove("X-User");
                        }
                        context.Request.Headers.Add("X-User", scopeProperties.User);
                    }
                }

                if (context.User != null && context.User.Claims != null)
                    scopeProperties.Claims = context.User.Claims
                    .Where(c => c.Type != nameClaimType
                        && !otherClaims.Any(o => o == c.Type)).ToArray();
                
                

            }
            await _next(context);

        }
    }

    public static class IApplicationBuilderExtensionsForUserFilter {
        public static IApplicationBuilder UseUser(this IApplicationBuilder app) {
            app.UseMiddleware<UserFilter>();
            return app;
        }
    }


}