using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    ///- *** ENVIRONMENT GUARD - NEVER IN PRODUCTION
    ///- Creates ScopeProperties.ActiveProfile based upon ...
    ///   - X-Instruction header (if present)
    ///   - AppSettings.Instruction configuration setting (if present)
    /// NOTE: Place in pipeline before PreAuthentication middleware
    /// </summary>
    public class InstructionMiddleware {

        private readonly RequestDelegate _next;

        public InstructionMiddleware(RequestDelegate next) {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context,
            IScopeProperties scopeProperties,
            IOptionsMonitor<AppSettings> appSettings,
            IOptionsMonitor<Profiles> profiles,
            IOptionsMonitor<Apis> apis,
            IOptionsMonitor<ConnectionStrings> connectionStrings,
            IOptionsMonitor<MockClients> mockClients,
            IOptionsMonitor<AutoLogins> autoLogins) {


            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                scopeProperties.ActiveProfile = new ResolvedProfile();
                var profileName = "Default";

                var instructionHeader = GetHeaderValue(context, Instruction.HEADER);
                if(instructionHeader != null) {
                    var parser = new InstructionParser();
                    scopeProperties.Instruction = parser.Parse(instructionHeader);
                    profileName = scopeProperties.Instruction.ProfileName;
                } else {
                    var appSettingsInstruction = appSettings?.CurrentValue?.Instruction;
                    if (appSettingsInstruction != null) {
                        profileName = appSettingsInstruction;
                        var parser = new InstructionParser();
                        scopeProperties.Instruction = parser.Parse(appSettingsInstruction);
                    }
                }

                scopeProperties.ActiveProfile.Load(profileName,
                    profiles?.CurrentValue, apis?.CurrentValue,
                    connectionStrings?.CurrentValue, mockClients?.CurrentValue,
                    autoLogins?.CurrentValue);

            }

            await _next(context);

        }

        private string GetHeaderValue(HttpContext context, string headerKey)
            => context.Request?.Headers?.FirstOrDefault(x
            => x.Key.Equals(headerKey, StringComparison.OrdinalIgnoreCase)).Value.ToString();

    }
}
