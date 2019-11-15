using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base.Web {
    public static class MiddlewareUtils {

        public static string ResolveUser(HttpContext context, UserSource[] userSource, string purpose) {
            foreach(var source in userSource) {
                var user = ResolveUser(context, source);
                if (!string.IsNullOrEmpty(user))
                    return user;
            }
            throw new ApplicationException($"Cannot resolve user setting for {purpose} with source(s) = '{string.Join(',',userSource)}'.");
        }


        public static string ResolveUser(HttpContext context, UserSource userSource) => userSource switch
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
            UserSource.XUserHeader => GetHeaderValue(context, Constants.USER_REQUEST_KEY),
            UserSource.XUserQueryString => context.Request.Query[Constants.USER_REQUEST_KEY].ToString(),
            _ => null
        };


        private static string GetClaimValue(HttpContext context, string claimType)
            => context.User?.Claims?.FirstOrDefault(x
                => x.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase))?.Value;


        private static string GetHeaderValue(HttpContext context, string headerKey)
            => context.Request?.Headers?.FirstOrDefault(x
                => x.Key.Equals(headerKey, StringComparison.OrdinalIgnoreCase)).Value.ToString();



    }


}
