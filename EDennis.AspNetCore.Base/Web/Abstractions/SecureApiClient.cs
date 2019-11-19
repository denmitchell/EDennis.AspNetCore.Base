using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base.Security;
using Flurl;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.AspNetCore.Base.Web {

    public abstract class SecureApiClient : ApiClient, ISecureApiClient {

        public SecureApiClient(HttpClient httpClient,
            IOptionsMonitor<Apis> apis, IScopeProperties scopeProperties,
            ISecureTokenService secureTokenService, ILogger logger)
            : base(httpClient, apis, scopeProperties, logger) {

            var tokenResponse = secureTokenService.GetTokenAsync(this).Result;
            HttpClient.SetBearerToken(tokenResponse.AccessToken);
        }


    }
}
