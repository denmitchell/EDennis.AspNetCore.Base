using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web
{

    public class ScopePropertiesFilter {

        private readonly RequestDelegate _next;
        private readonly ScopePropertiesOptions _options;

        public ScopePropertiesFilter(RequestDelegate next, IOptions<ScopePropertiesOptions> options) {
            _next = next;
            _options = options?.Value ?? new ScopePropertiesOptions();
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider) {



            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                var scopeProperties = provider.GetRequiredService(typeof(ScopeProperties)) as ScopeProperties;

                //set User property
                scopeProperties.User = _options.UserSource switch
                {
                    UserSource.CLAIMS_PRINCIPAL_IDENTITY_NAME => context.User?.Identity?.Name,
                    UserSource.NAME_CLAIM => GetClaimValue(context, JwtClaimTypes.Name),
                    UserSource.PREFERRED_USERNAME_CLAIM => GetClaimValue(context, JwtClaimTypes.PreferredUserName),
                    UserSource.SUBJECT_CLAIM => GetClaimValue(context, JwtClaimTypes.Subject),
                    UserSource.EMAIL_CLAIM => GetClaimValue(context, IdentityServerConstants.StandardScopes.Email),
                    UserSource.PHONE_CLAIM => GetClaimValue(context, IdentityServerConstants.StandardScopes.Phone),
                    UserSource.CLIENT_ID_CLAIM => GetClaimValue(context, JwtClaimTypes.ClientId),
                    UserSource.CUSTOM_CLAIM => GetClaimValue(context, _options.UserSourceClaimType),
                    UserSource.SESSION_ID => context.Session?.Id,
                    UserSource.X_USER_HEADER => GetHeaderValue(context,"X-User"),
                    _ => null
                };

                foreach (var prefix in _options.StoreHeadersWithPrefixes) {
                    var headers = context.Request.Headers
                        .Where(x => x.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        .Select(y => (y.Value.ToArray()).Select(x => KeyValuePair.Create(y,x)));

                    scopeProperties.Headers.AddRange(headers);

                        .SelectMany(x => x).Select(y=> new  item.Value.ToArray(), 
                            (item,value) => Tuple.Create(item.Key, item.Value));
                    }

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
                        foreach (var nameClaim in nameClaims) {
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


        private string GetClaimValue(HttpContext context, string claimType)
            => context.User?.Claims?.FirstOrDefault(x 
                => x.Type.Equals(claimType,StringComparison.OrdinalIgnoreCase))?.Value;


        private string GetHeaderValue(HttpContext context, string headerKey)
            => context.Request?.Headers?.FirstOrDefault(x
                => x.Key.Equals(headerKey, StringComparison.OrdinalIgnoreCase)).Value.ToString();


    }

    public static class IApplicationBuilderExtensionsForUserFilter {
        public static IApplicationBuilder UseUser(this IApplicationBuilder app) {
            app.UseMiddleware<UserFilter>();
            return app;
        }
    }


}