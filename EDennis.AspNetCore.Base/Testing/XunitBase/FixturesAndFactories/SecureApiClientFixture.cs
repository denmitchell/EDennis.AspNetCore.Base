using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Security.Claims;
using EDennis.AspNetCore.Base.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using IdentityServer4.Models;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class SecureApiClientFixture<TClient> : AbstractLauncherFixture
        where TClient : SecureApiClient {


        public const string DEFAULT_USER = "tester@example.org";

        public virtual string User { get; } = DEFAULT_USER;

        public virtual IScopeProperties ScopeProperties { get; } =
            new ScopeProperties {
                User = DEFAULT_USER,
                Claims = new Claim[] {
                    new Claim("name",DEFAULT_USER)
                }
            };

        public virtual IConfiguration Configuration { get; set; } =
            new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
            .AddJsonFile("appsettings.Shared.json", true, true)
            .Build();

        public virtual string ApisConfigurationKey { get; } = "Apis";

        public SecureTokenService SecureTokenService { get; }
        public SecureApiClient ApiClient { get; }

        public SecureApiClientFixture() {
            var apis = GetApisSettings();
            var api = GetApiSettings(apis);
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(api.MainAddress);
            ILogger<SecureTokenService> logger = new Logger<SecureTokenService>(new NullLoggerFactory());
            SecureTokenService = new SecureTokenService(GetIOptionsMonitorApis(apis), NullLogger<SecureTokenService>.Instance);
            ApiClient = (SecureApiClient) Activator.CreateInstance(typeof(TClient), 
                new object[] { GetIOptionsMonitorApis(apis),
                    ScopeProperties,
                    SecureTokenService,
                    NullLogger<TClient>.Instance }
            );
        }


        private Apis GetApisSettings() {
            var settings = new Apis();
            Configuration.GetSection(ApisConfigurationKey).Bind(settings);
            return settings;
        }

        private Api GetApiSettings(Apis apis) {
            return apis[typeof(TClient).Name];
        }

        private IOptionsMonitor<Apis> GetIOptionsMonitorApis(Apis apis) {
            return new TestOptionsMonitor<Apis>(apis);
        }


        private IScopeProperties GetScopeProperties() {
            return new ScopeProperties { User = User };
        }

    }

}

