using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web.Extensions {
    public static class IServiceCollectionExtensions_ConfigureApp {

        public static IServiceCollection ConfigureApp(
            this IServiceCollection services, IConfiguration config,
            string profilesSectionKey = "Profiles", string startupProfileKey = "StartupProfile",
            string loggersSectionKey = "Logging:Loggers", string securitySectionKey = "Security",  
            string mockClientsSectionKey = "MockClients", string autoLoginsSectionKey = "AutoLogins"
            ) {
            
            if(config.ContainsKey(profilesSectionKey))
                services.Configure<Profiles>(config.GetSection(profilesSectionKey));

            if (config.ContainsKey(startupProfileKey))
                services.Configure<Profiles>(config.GetSection(startupProfileKey));

            if (config.ContainsKey(loggersSectionKey))
                services.Configure<Profiles>(config.GetSection(loggersSectionKey));

            if (config.ContainsKey(securitySectionKey))
                services.Configure<Profiles>(config.GetSection(securitySectionKey));
        }

    }
}
