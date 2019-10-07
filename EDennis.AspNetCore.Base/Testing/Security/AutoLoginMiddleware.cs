using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Testing
{

    /// <summary>
    /// This middleware class creates a mock (autologin) user
    /// defined in configuration. 
    /// The middleware should be invoked before Authentication
    /// and only when in the Development environment.
    /// </summary>
    public class AutoLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public AutoLoginMiddleware(RequestDelegate next) {
            _next = next;
        }

        /// <summary>
        /// Create an autologin user whose name is taken from Configuration. 
        /// Make sure to include "AutoLogin=SomeUserName" as a command-line
        /// argument (in launchSettings.json or otherwise) and include
        /// "AutoLogin:SomeUserName" as an entry in appsettings.Development.json.
        /// 
        /// in launchSettings.json:
        /// "AutoLogin=SomeUserName": {
        ///     "commandName": "Project",
        ///     "launchBrowser": true,
        ///     "commandLineArgs": "AutoLogin=SomeUserName",
        ///     "applicationUrl": "http://localhost:5000",
        ///     "environmentVariables": {
        ///         "ASPNETCORE_ENVIRONMENT": "Development"
        ///     }
        /// }
        ///
        /// in appsettings.Development.json:
        /// {
        ///   "AutoLogin": {
        ///     "SomeUserName": {
        ///         "Default" : true,
        ///         "Claims" : [
        ///             {
        ///                 "Type": "SomeClaimType",
        ///                 "Value": "SomeClaimValue"
        ///             },
        ///             {
        ///                 "Type": "AnotherClaimType",
        ///                 "Value": "AnotherClaimValue"
        ///             }
        ///         ]
        ///     },
        ///     "AnotherUserName": {
        ///         "Claims" : [
        ///             {
        ///                 "Type": "SomeClaimType",
        ///                 "Value": "SomeClaimValue"
        ///             },
        ///             {
        ///                 "Type": "AnotherClaimType",
        ///                 "Value": "AnotherClaimValue"
        ///             }
        ///         ]
        ///     }     
        ///   }
        /// }
        /// </summary>
        /// <param name="next"></param>
        /// </summary>
        /// <param name="httpContext">The HttpContext of the Request/Response</param>
        /// <param name="config">The config object, which gathers information from appsettings.json, environment vars ... etc.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext, IServiceProvider provider, IConfiguration config) {


            if (!httpContext.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                var autologinDict = new AutoLoginDictionary();

                config.GetSection("AutoLogin").Bind(autologinDict);

                if (autologinDict == null || autologinDict.Count == 0)
                    throw new ArgumentException($"Missing configuration for AutoLogin");


                //CommandLineConfigurationSource cmdSource

                //get command-line arguments
                var args = config.GetCommandLineArguments().ToDictionary(
                    d => d.Key.ToLower(), d => d.Value);

                //get AutoLogin entry, if it exists
                var autologinArg = (!args.ContainsKey("autologin") ? null : args["autologin"]);

                if (autologinArg == null)
                    autologinArg = autologinDict
                        .Where(x => x.Value.Default)
                        .FirstOrDefault()
                        .Key;

                if (autologinArg == null)
                    throw new ArgumentException($"Missing configuration for AutoLogin");


                if (!autologinDict.ContainsKey(autologinArg))
                    throw new ArgumentException($"Missing configuration for AutoLogin:{autologinArg}");

                //get the configuration for the autologin
                var autologinConfig = autologinDict[autologinArg];

                //get the autologin claims
                if (!(autologinConfig.Claims is IEnumerable<MockClaim> autoLoginConfigClaims) || autologinConfig.Claims.Count() == 0)
                    throw new ArgumentException($"Missing claims configuration for AutoLogin:{autologinArg}");


                //add microsoft uri claims
                var claims = autoLoginConfigClaims
                    .Select(a => new Claim(GetClaimUri(a.Type), a.Value));

                //add simple name (jwt) claims
                claims = claims.Union(autoLoginConfigClaims
                    .Select(a => new Claim(a.Type, a.Value)));


                //add the Name claim type, if it doesn't exist already
                if (claims.Where(x => x.Type == ClaimTypes.Name).Count() == 0) {
                    claims = claims.Union(
                        new Claim[] {
                        new Claim(ClaimTypes.Name, autologinArg), //microsoft uri
                        new Claim("name", autologinArg) //jwt/simple type
                        });
                }


                var scopeProperties = provider.GetRequiredService<ScopeProperties>();
                scopeProperties.User = autologinArg;
                scopeProperties.UpdateLoggerIndex();

                //create the new user object
                var identity = new ClaimsIdentity(claims,
                      CookieAuthenticationDefaults.AuthenticationScheme);
                httpContext.User = new ClaimsPrincipal(identity);

                //sign the user on and serialize the user principal to a cookie
                await httpContext.SignInAsync(httpContext.User);

            }
            await _next(httpContext);
        }


        /// <summary>
        /// Get the URI for a claim type name, based upon
        /// its short name
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns>URI, if it exists, or shortName
        /// for claim, if it doesn't</returns>
        private string GetClaimUri(string shortName) {
            return (shortName.ToLower()) switch
            {
                "actor" => ClaimTypes.Actor,
                "anonymous" => ClaimTypes.Anonymous,
                "authentication" => ClaimTypes.Authentication,
                "authenticationinstant" => ClaimTypes.AuthenticationInstant,
                "authenticationmethod" => ClaimTypes.AuthenticationMethod,
                "authorizationdecision" => ClaimTypes.AuthorizationDecision,
                "cookiepath" => ClaimTypes.CookiePath,
                "country" => ClaimTypes.Country,
                "dateofbirth" => ClaimTypes.DateOfBirth,
                "denyonlyprimarygroupsid" => ClaimTypes.DenyOnlyPrimaryGroupSid,
                "denyonlyprimarysid" => ClaimTypes.DenyOnlyPrimarySid,
                "denyonlysid" => ClaimTypes.DenyOnlySid,
                "denyonlywindowsdevicegroup" => ClaimTypes.DenyOnlyWindowsDeviceGroup,
                "dns" => ClaimTypes.Dns,
                "dsa" => ClaimTypes.Dsa,
                "email" => ClaimTypes.Email,
                "expiration" => ClaimTypes.Expiration,
                "expired" => ClaimTypes.Expired,
                "gender" => ClaimTypes.Gender,
                "givenname" => ClaimTypes.GivenName,
                "groupsid" => ClaimTypes.GroupSid,
                "hash" => ClaimTypes.Hash,
                "homephone" => ClaimTypes.HomePhone,
                "ispersistent" => ClaimTypes.IsPersistent,
                "locality" => ClaimTypes.Locality,
                "mobilephone" => ClaimTypes.MobilePhone,
                "name" => ClaimTypes.Name,
                "nameidentifier" => ClaimTypes.NameIdentifier,
                "otherphone" => ClaimTypes.OtherPhone,
                "postalcode" => ClaimTypes.PostalCode,
                "primarygroupsid" => ClaimTypes.PrimaryGroupSid,
                "primarysid" => ClaimTypes.PrimarySid,
                "role" => ClaimTypes.Role,
                "rsa" => ClaimTypes.Rsa,
                "serialnumber" => ClaimTypes.SerialNumber,
                "sid" => ClaimTypes.Sid,
                "spn" => ClaimTypes.Spn,
                "stateorprovince" => ClaimTypes.StateOrProvince,
                "streetaddress" => ClaimTypes.StreetAddress,
                "surname" => ClaimTypes.Surname,
                "system" => ClaimTypes.System,
                "thumbprint" => ClaimTypes.Thumbprint,
                "upn" => ClaimTypes.Upn,
                "uri" => ClaimTypes.Uri,
                "userdata" => ClaimTypes.UserData,
                "version" => ClaimTypes.Version,
                "webpage" => ClaimTypes.Webpage,
                "windowsaccountname" => ClaimTypes.WindowsAccountName,
                "windowsdeviceclaim" => ClaimTypes.WindowsDeviceClaim,
                "windowsdevicegroup" => ClaimTypes.WindowsDeviceGroup,
                "windowsfqbnversion" => ClaimTypes.WindowsFqbnVersion,
                "windowssubauthority" => ClaimTypes.WindowsSubAuthority,
                "windowsuserclaim" => ClaimTypes.WindowsUserClaim,
                "x500distinguishedname" => ClaimTypes.X500DistinguishedName,
                _ => shortName,
            };
        }
    }


    /// <summary>
    /// Miscellaneous extension methods supporting the
    /// AutoLoginMiddleware class
    /// </summary>
    public static class IApplicationBuilder_AutoLoginExtensionMethods
    {

        /// <summary>
        /// Creates a Use... method for the middleware
        /// </summary>
        /// <param name="app">application builder used by Startup.cs</param>
        /// <returns>application builder (for method chaining)</returns>
        public static IApplicationBuilder UseAutoLogin(
                this IApplicationBuilder app) {
            return app.UseMiddleware<AutoLoginMiddleware>();
        }


    }

}
