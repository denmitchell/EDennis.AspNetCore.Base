using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web.Extensions {
    public static class IServiceCollectionExtensions_ConfigureApp {

        /// <summary>
        /// Used to setup IOptionMonitor for configuration settings.
        /// The Profiles section is used to configure settings for different run profiles
        ///    (e.g., connection strings and endpoints for APIs) which can be selected
        ///    independent of environment.  This section is designed to be used with 
        ///    ScopePropertiesMiddleware, which can select an active profile based 
        ///    upon startup configuration settings (e.g., "LaunchProfile") or 
        ///    request configuration settings (see RequestConfig).
        /// The Security section provides settings for OAuth, OIDC, and the library's 
        ///    DefaultPolicies feature.  The library is designed to work with 
        ///    IServiceCollectionExtensions_Security methods, as well as microsoft
        ///    authentication, microsoft authorization, and IdentityServer4.
        /// The MockClients section provides a dictionary of OAuth clients that can be used 
        ///    for testing with IdentityServer4.  It is designed to be used with
        ///    MockClientAuthorizationMiddleware
        /// The AutoLogins section
        /// All top-level, simple key-value pair settings can be accessed with <code>IOptionsMonitor<AppSettings>()</code>
        /// The logging settings only capture the logger names.  Logging frameworks typically access appsettings directly for settings.
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="profilesSectionKey"></param>
        /// <param name="loggersSectionKey"></param>
        /// <param name="securitySectionKey"></param>
        /// <param name="mockClientsSectionKey"></param>
        /// <param name="autoLoginsSectionKey"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureApp(
            this IServiceCollection services, IConfiguration config,
            string profilesSectionKey = "Profiles", 
            string loggersSectionKey = "Logging:Loggers", string securitySectionKey = "Security",  
            string mockClientsSectionKey = "MockClients", string autoLoginsSectionKey = "AutoLogins"
            ) {

            services.Configure<AppSettings>(config);

            if (config.ContainsKey(profilesSectionKey))
                services.Configure<Profiles>(config.GetSection(profilesSectionKey));

            if (config.ContainsKey(loggersSectionKey))
                services.Configure<NamedLoggers>(config.GetSection(loggersSectionKey));

            if (config.ContainsKey(securitySectionKey))
                services.Configure<SecurityOptions>(config.GetSection(securitySectionKey));

            if (config.ContainsKey(mockClientsSectionKey))
                services.Configure<MockClients>(config.GetSection(mockClientsSectionKey));

            if (config.ContainsKey(autoLoginsSectionKey))
                services.Configure<AutoLogins>(config.GetSection(autoLoginsSectionKey));


            return services;
        }

    }
}
