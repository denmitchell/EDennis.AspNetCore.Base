using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// This middleware class creates a mock (autologin) user
    /// defined in configuration. 
    /// The middleware should be invoked before Authentication
    /// and only when in the Development environment.
    /// </summary>
    public class AutoLoginMiddleware {
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
        ///       "SomeClaimType": "SomeClaimValue",
        ///       "AnotherClaimType": "AnotherClaimValue"
        ///     }
        ///   }
        /// }
        /// </summary>
        /// <param name="next"></param>
        /// </summary>
        /// <param name="httpContext">The HttpContext of the Request/Response</param>
        /// <param name="config">The config object, which gathers information from appsettings.json, environment vars ... etc.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext, IConfiguration config) {


            var autologinDict = new AutoLoginDictionary();

            config.GetSection("AutoLogin").Bind(autologinDict);

            if (autologinDict == null || autologinDict.Count == 0)
                throw new ArgumentException($"Missing configuration for AutoLogin");


            //CommandLineConfigurationSource cmdSource

            //get command-line arguments
            var args = config.GetCommandLineArguments().ToDictionary(
                d=>d.Key.ToLower(),d=>d.Value);

            //get AutoLogin entry, if it exists
            var autologinArg = (!args.ContainsKey("autologin") ? null : args["autologin"]);

            if (autologinArg == null)
                autologinArg = autologinDict
                    .Where(x=>x.Value.Default)
                    .FirstOrDefault()
                    .Key;

            if (autologinArg == null)
                throw new ArgumentException($"Missing configuration for AutoLogin");


            if(!autologinDict.ContainsKey(autologinArg))
                throw new ArgumentException($"Missing configuration for AutoLogin:{autologinArg}");

            //get the configuration for the autologin
            var autologinConfig = autologinDict[autologinArg];

            //get the autologin claims
            var autoLoginConfigClaims = autologinConfig.Claims as IEnumerable<MockClaim>;
            if (autoLoginConfigClaims == null)
                throw new ArgumentException($"Missing claims configuration for AutoLogin:{autologinArg}");


            //add microsoft uri claims
            var claims = autoLoginConfigClaims
                .Select(a => new Claim(GetClaimUri(a.Type), a.Value));

            //add simple name (jwt) claims
            claims = claims.Union( autoLoginConfigClaims
                .Select(a => new Claim(a.Type, a.Value)));


            //add the Name claim type, if it doesn't exist already
            if (claims.Where(x => x.Type == ClaimTypes.Name).Count() == 0) {
                claims = claims.Union(
                    new Claim[] {
                        new Claim(ClaimTypes.Name, autologinArg), //microsoft uri
                        new Claim("name", autologinArg) //jwt/simple type
                    });
            }




            //create the new user object
            var identity = new ClaimsIdentity(claims,
                  CookieAuthenticationDefaults.AuthenticationScheme);
            httpContext.User = new ClaimsPrincipal(identity);

            //sign the user on and serialize the user principal to a cookie
            await httpContext.SignInAsync(httpContext.User);


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
            switch (shortName.ToLower()) {
                case "actor":
                    return ClaimTypes.Actor;
                case "anonymous":
                    return ClaimTypes.Anonymous;
                case "authentication":
                    return ClaimTypes.Authentication;
                case "authenticationinstant":
                    return ClaimTypes.AuthenticationInstant;
                case "authenticationmethod":
                    return ClaimTypes.AuthenticationMethod;
                case "authorizationdecision":
                    return ClaimTypes.AuthorizationDecision;
                case "cookiepath":
                    return ClaimTypes.CookiePath;
                case "country":
                    return ClaimTypes.Country;
                case "dateofbirth":
                    return ClaimTypes.DateOfBirth;
                case "denyonlyprimarygroupsid":
                    return ClaimTypes.DenyOnlyPrimaryGroupSid;
                case "denyonlyprimarysid":
                    return ClaimTypes.DenyOnlyPrimarySid;
                case "denyonlysid":
                    return ClaimTypes.DenyOnlySid;
                case "denyonlywindowsdevicegroup":
                    return ClaimTypes.DenyOnlyWindowsDeviceGroup;
                case "dns":
                    return ClaimTypes.Dns;
                case "dsa":
                    return ClaimTypes.Dsa;
                case "email":
                    return ClaimTypes.Email;
                case "expiration":
                    return ClaimTypes.Expiration;
                case "expired":
                    return ClaimTypes.Expired;
                case "gender":
                    return ClaimTypes.Gender;
                case "givenname":
                    return ClaimTypes.GivenName;
                case "groupsid":
                    return ClaimTypes.GroupSid;
                case "hash":
                    return ClaimTypes.Hash;
                case "homephone":
                    return ClaimTypes.HomePhone;
                case "ispersistent":
                    return ClaimTypes.IsPersistent;
                case "locality":
                    return ClaimTypes.Locality;
                case "mobilephone":
                    return ClaimTypes.MobilePhone;
                case "name":
                    return ClaimTypes.Name;
                case "nameidentifier":
                    return ClaimTypes.NameIdentifier;
                case "otherphone":
                    return ClaimTypes.OtherPhone;
                case "postalcode":
                    return ClaimTypes.PostalCode;
                case "primarygroupsid":
                    return ClaimTypes.PrimaryGroupSid;
                case "primarysid":
                    return ClaimTypes.PrimarySid;
                case "role":
                    return ClaimTypes.Role;
                case "rsa":
                    return ClaimTypes.Rsa;
                case "serialnumber":
                    return ClaimTypes.SerialNumber;
                case "sid":
                    return ClaimTypes.Sid;
                case "spn":
                    return ClaimTypes.Spn;
                case "stateorprovince":
                    return ClaimTypes.StateOrProvince;
                case "streetaddress":
                    return ClaimTypes.StreetAddress;
                case "surname":
                    return ClaimTypes.Surname;
                case "system":
                    return ClaimTypes.System;
                case "thumbprint":
                    return ClaimTypes.Thumbprint;
                case "upn":
                    return ClaimTypes.Upn;
                case "uri":
                    return ClaimTypes.Uri;
                case "userdata":
                    return ClaimTypes.UserData;
                case "version":
                    return ClaimTypes.Version;
                case "webpage":
                    return ClaimTypes.Webpage;
                case "windowsaccountname":
                    return ClaimTypes.WindowsAccountName;
                case "windowsdeviceclaim":
                    return ClaimTypes.WindowsDeviceClaim;
                case "windowsdevicegroup":
                    return ClaimTypes.WindowsDeviceGroup;
                case "windowsfqbnversion":
                    return ClaimTypes.WindowsFqbnVersion;
                case "windowssubauthority":
                    return ClaimTypes.WindowsSubAuthority;
                case "windowsuserclaim":
                    return ClaimTypes.WindowsUserClaim;
                case "x500distinguishedname":
                    return ClaimTypes.X500DistinguishedName;
                default:
                    return shortName;
            }
        }
    }


    /// <summary>
    /// Miscellaneous extension methods supporting the
    /// AutoLoginMiddleware class
    /// </summary>
    public static class IApplicationBuilder_AutoLoginExtensionMethods {

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
