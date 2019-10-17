using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace EDennis.AspNetCore.Base.Web
{
    public abstract class ApiClient {

        [Obsolete]
        public const string HEADER_KEY = "ApiClientHeaders";
        public HttpClient HttpClient { get; set; }
        public ScopeProperties ScopeProperties { get; set; }
        public IConfiguration Configuration { get; set; }
        public ILogger Logger { get; }
        public ApiClient(HttpClient httpClient, IConfiguration config, 
            ScopeProperties scopeProperties, ILogger logger) {

            HttpClient = httpClient;
            ScopeProperties = scopeProperties;
            Configuration = config;
            Logger = logger;

            BuildClient(config);
        }

        private void BuildClient(IConfiguration config) {

            var apiClientConfig = new ApiConfig();
            try {
                config.Bind($"Apis:{GetType().Name}", apiClientConfig);
            } catch (Exception ex) {
                string msg;
                if (string.IsNullOrEmpty(config[$"Apis:{GetType().Name}"]))
                    msg = $"Configuration does not contain an entry for Apis:{GetType().Name}";
                else
                    msg = $"Unable to bind Apis:{GetType().Name} as ApiConfig object.";
                throw new ApplicationException(msg,ex);
            }

            #region BaseAddress
            var baseAddress = apiClientConfig.BaseAddress;

            var env = config["ASPNETCORE_ENVIRONMENT"];

            if (string.IsNullOrEmpty(baseAddress))
                throw new ApplicationException($"Cannot find entry for 'Apis:{GetType().Name}:BaseAddress' in the configuration (e.g., appsettings.{env}.json).  Each ApiClient and SecureApiClient must have an entry in the configuration that corresponds to the class name of the ApiClient or SecureApiClient.");


            HttpClient.BaseAddress = new Uri(baseAddress);
            #endregion
            #region DefaultRequestHeaders

            var pOpt = apiClientConfig.ScopePropertiesOptions;

            ScopeProperties.Headers
                .Where(h => Regex.IsMatch(h.Key, pOpt.PropagateHeadersWithPattern, RegexOptions.IgnoreCase))
                .ToList()
                .ForEach(x => HttpClient.DefaultRequestHeaders
                .Add(x.Key, x.Value.ToArray()));

            ScopeProperties.Claims
                .Where(c => Regex.IsMatch(c.Type, pOpt.PropagateClaimTypesWithPattern, RegexOptions.IgnoreCase))
                .GroupBy(c => c.Type)
                .Select(g => KeyValuePair.Create(g.Key, g.Select(v => v.Value)))
                .ToList()
                .ForEach(x => HttpClient.DefaultRequestHeaders
                .AddOrReplace($"X-{x.Key}", x.Value));

            if (ScopeProperties.User != null && pOpt.PropagateUser)
                HttpClient.DefaultRequestHeaders
                    .AddOrReplace("X-User", ScopeProperties.User);

            #endregion

        }
    }
}
