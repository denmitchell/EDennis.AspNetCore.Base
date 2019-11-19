using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;


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

        public ISecureTokenService SecureTokenService { get; }
        public SecureApiClient SecureApiClient { get; }
        public Apis Apis { get; }
        public Api Api { get; }

        public SecureApiClientFixture() {
            Apis = GetApisSettings();
            Api = GetApiSettings(Apis);

            var httpClient = new HttpClient {
                BaseAddress = new Uri(Api.MainAddress)
            };

            SecureTokenService = new SecureTokenService(GetIOptionsMonitorApis(Apis), NullLogger<SecureTokenService>.Instance, null) {
                ApplicationName = typeof(TClient).Assembly.GetName().Name
            };

            SecureApiClient = (SecureApiClient) Activator.CreateInstance(typeof(TClient), 
                new object[] {
                    httpClient,
                    GetIOptionsMonitorApis(Apis),
                    ScopeProperties,
                    SecureTokenService,
                    NullLogger<TClient>.Instance }
            );
        }


        public async Task SendResetAsync() {
            await MiddlewareUtils.Reset(Apis, InstanceName);
        }

        public void SendReset() {
            MiddlewareUtils.Reset(Apis, InstanceName);
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

    }

}

