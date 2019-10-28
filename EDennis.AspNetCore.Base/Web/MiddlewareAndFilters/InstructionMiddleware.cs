using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
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

        private readonly AppSettings _appSettings;
        private readonly Profiles _profiles;


        public InstructionMiddleware(RequestDelegate next,
            IOptionsMonitor<AppSettings> appSettings,
            IOptionsMonitor<Profiles> profiles) {
            _next = next;
            _appSettings = appSettings.CurrentValue;
            _profiles = profiles.CurrentValue;
    
            if(!_profiles.NamesUpdated)
                _profiles.UpdateNames();
        }


        public async Task InvokeAsync(HttpContext context,
            IScopeProperties scopeProperties) {


            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {


                scopeProperties.ActiveProfile = new Profile();
                var profileName = Profile.DEFAULT_NAME;

                var instructionHeader = GetHeaderValue(context, Instruction.HEADER);
                if(instructionHeader != null) {
                    var parser = new InstructionParser();
                    scopeProperties.Instruction = parser.Parse(instructionHeader);
                    profileName = scopeProperties.Instruction.ProfileName;
                } else {
                    var appSettingsInstruction = _appSettings.Instruction;
                    if (appSettingsInstruction != null) {
                        profileName = appSettingsInstruction;
                        var parser = new InstructionParser();
                        scopeProperties.Instruction = parser.Parse(appSettingsInstruction);
                    }
                }

                scopeProperties.ActiveProfile = _profiles[profileName];

            }

            await _next(context);

        }

        private string GetHeaderValue(HttpContext context, string headerKey)
            => context.Request?.Headers?.FirstOrDefault(x
            => x.Key.Equals(headerKey, StringComparison.OrdinalIgnoreCase)).Value.ToString();

    }
}
