using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;


namespace EDennis.AspNetCore.Base.Testing {

    public abstract class SecureApiClientFixture<TClient> : ApiClientFixture<TClient>
        where TClient : SecureApiClient {


        public ISecureTokenService SecureTokenService { get; }

        public SecureApiClientFixture() {
            Apis = GetApisSettings();
            Api = GetApiSettings(Apis);

            var httpClientFactory = new TestHttpClientFactory(TestApis.CreateClient);

            SecureTokenService = new SecureTokenService(GetIOptionsMonitorApis(Apis), NullLogger<SecureTokenService>.Instance, null) {
                ApplicationName = typeof(TClient).Assembly.GetName().Name
            };

            ApiClient = (SecureApiClient) Activator.CreateInstance(typeof(TClient), 
                new object[] {
                    httpClientFactory,
                    GetIOptionsMonitorApis(Apis),
                    ScopeProperties,
                    SecureTokenService,
                    NullLogger<TClient>.Instance,
                    new NullScopedLogger()
                }
            );
        }


        private IOptionsMonitor<Apis> GetIOptionsMonitorApis(Apis apis) {
            return new TestOptionsMonitor<Apis>(apis);
        }

    }

}

