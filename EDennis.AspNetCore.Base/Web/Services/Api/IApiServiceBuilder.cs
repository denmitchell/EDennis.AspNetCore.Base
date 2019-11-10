using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {

    public interface IApiServiceBuilder { 
        IServiceCollection ServiceCollection { get; set; } 
        IConfiguration Configuration { get; set; }
        string ConfigurationKey { get; set; }
    }

    public class ApiServiceBuilder : IApiServiceBuilder {
        public IServiceCollection ServiceCollection { get; set; }
        public IConfiguration Configuration { get; set; }
        public string ConfigurationKey { get; set; }
    }



    public static class IApiServiceBuilder_Extensions {

        public static IApiServiceBuilder AddLauncher<TStartup>(this IApiServiceBuilder builder) {
            builder.ServiceCollection.PostConfigure<ApiSettings>(options => {
                options.Facade = new ApiSettingsFacade {
                    Configuration = builder.Configuration,
                    ConfigurationKey = builder.ConfigurationKey
                };
            });
            return builder;
        }


        public static IApiServiceBuilder AddClient<TClientImplementation>(this IApiServiceBuilder builder, bool traceable = true)
            where TClientImplementation : ApiClient {
            builder.AddDependencies<TClientImplementation>(traceable);
            builder.AddApiClientInternal<TClientImplementation, TClientImplementation>(traceable);
            return builder;
        }

        public static IApiServiceBuilder AddClient<TClientInterface,TClientImplementation>(this IApiServiceBuilder builder, bool traceable = true)
            where TClientInterface : class, IHasILogger
            where TClientImplementation : ApiClient, TClientInterface {

            builder.AddDependencies<TClientImplementation>(traceable);
            builder.AddApiClientInternal<TClientInterface, TClientImplementation>(traceable);
            return builder;
        }

        private static void AddDependencies<TClientImplementation>(this IApiServiceBuilder builder, bool traceable)
            where TClientImplementation : class {

            builder.ServiceCollection.TryAddScoped<IScopeProperties, ScopeProperties>();

            bool isSecureClient = typeof(SecureApiClient).IsAssignableFrom(typeof(TClientImplementation));

            if (isSecureClient) {
                builder.ServiceCollection.TryAddSingleton<SecureTokenService, SecureTokenService>();
                builder.ServiceCollection.TryAddScoped<IdentityServerApi>();
            }
        }

        private static IApiServiceBuilder AddApiClientInternal<TClientInterface, TClientImplementation>(this IApiServiceBuilder builder, bool traceable)
            where TClientInterface : class, IHasILogger
            where TClientImplementation : ApiClient, TClientInterface {


            if (traceable)
                builder.ServiceCollection.AddScopedTraceable<TClientInterface, TClientImplementation>();
            else
                builder.ServiceCollection.AddHttpClient<TClientInterface, TClientImplementation>();

            return builder;
        }


    }
}
