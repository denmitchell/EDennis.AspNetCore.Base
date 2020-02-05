﻿using EDennis.AspNetCore.Base.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace EDennis.AspNetCore.Base.Web {
    //note: use ApiAttribute(ApiKey) to specify a key for the configuration file that
    //      differs from the class name
    public abstract class ApiClient : IApiClient {

        public HttpClient HttpClient { get; }
        public Api Api { get; }
        public IScopeProperties ScopeProperties { get; }
        IWebHostEnvironment HostEnvironment { get; }

        public ApiClient(
            IHttpClientFactory httpClientFactory,
            IOptionsMonitor<Apis> apis,
            IScopeProperties scopeProperties,
            IWebHostEnvironment env) {

            ScopeProperties = scopeProperties;
            HostEnvironment = env;

            ApiKey = GetType().Name;
            HttpClient = httpClientFactory.CreateClient(ApiKey);
            try {
                Api = apis.CurrentValue[ApiKey];
            } catch (Exception) {
                Debug.WriteLine(string.Format("For ApiClient {0} Cannot find '{1}' in Apis section of Configuration", GetType().Name, ApiKey));
                throw;
            }

            BuildClient();

        }

        public virtual string ApiKey { get;}




        private void BuildClient() {

            #region BaseAddress
            if(!HttpClient.BaseAddress.ToString().Equals(Api.MainAddress))
                HttpClient.BaseAddress = new Uri(Api.MainAddress);
            #endregion
            #region DefaultRequestHeaders

            var headersToTransfer = Api.Mappings.HeadersToHeaders;
            var claimsToTransfer = Api.Mappings.ClaimsToHeaders;

            //add X-RequestFrom header to allow child API to know that it is a child
            //(important for ApplicationPropertiesMiddleware)
            HttpClient.DefaultRequestHeaders.Add("X-RequestFrom",HostEnvironment.ApplicationName);


            //add claims as headers
            if (ScopeProperties.Claims != null) {
                var claimsDictionary = ScopeProperties.Claims
                            .GroupBy(x => x.Type)
                            .ToDictionary(g => g.Key, g => new StringValues(g.Select(x => x.Value).ToArray()));

                foreach (var key in claimsToTransfer.Keys)
                    foreach (var claim in claimsDictionary.Where(d => d.Key == key))
                        HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, claim.Value.ToArray());
            }

            //add additional headers
            if (ScopeProperties.Headers != null) {
                foreach (var key in headersToTransfer.Keys)
                    foreach (var hdr in ScopeProperties.Headers.Where(d => d.Key == key))
                        HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, hdr.Value.ToArray());
            }

            #endregion

        }
    }
}