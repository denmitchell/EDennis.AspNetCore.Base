using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Testing {

    public abstract class ApiClientFixture<TClient>
        where TClient : ApiClient {

        public const string DEFAULT_USER = "tester@example.org";

        public virtual ProgramBase Program { get; set; }

        public virtual string ApiKey { get; } = typeof(TClient).Name;

        public abstract TestApisBase TestApis { get; }

        public virtual string User { get; } = DEFAULT_USER;

        public virtual IScopeProperties ScopeProperties { get; } =
            new ScopeProperties {
                User = DEFAULT_USER,
                Claims = new Claim[] {
                    new Claim("name",DEFAULT_USER)
                }
            };

        public ApiClient ApiClient { get; protected set; }
        public Apis Apis { get; protected set; }
        public Api Api { get; protected set; }

        public ApiClientFixture() {
            Apis = GetApisSettings();
            Api = GetApiSettings(Apis);

            var httpClientFactory = new TestHttpClientFactory(TestApis.CreateClient);

            ApiClient = (ApiClient) Activator.CreateInstance(typeof(TClient), 
                new object[] {
                    httpClientFactory,
                    GetIOptionsMonitorApis(Apis),
                    ScopeProperties,
                    NullLogger<TClient>.Instance,
                    new NullScopedLogger()                
                }
            );
        }


        public async Task SendResetAsync() {
            var clients = TestApis.CreateClient.Values.ToList().Select(f => f.Invoke()).ToArray();                
            await MiddlewareUtils.Reset(clients, TestApis.InstanceName);
        }

        public void SendReset() {
            SendResetAsync().Wait();
        }

        protected Apis GetApisSettings() {
            var settings = new Apis();
            Program.Configuration.GetSection(Program.ApisConfigurationSection).Bind(settings);
            return settings;
        }

        protected Api GetApiSettings(Apis apis) {
            return apis[ApiKey];
        }

        private IOptionsMonitor<Apis> GetIOptionsMonitorApis(Apis apis) {
            return new TestOptionsMonitor<Apis>(apis);
        }

    }

}

