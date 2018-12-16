using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System;
using static EDennis.AspNetCore.Base.Web.TestingActionFilter;
using Microsoft.Extensions.Configuration;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This type of controller holds references to Api proxies,
    /// which can be used to invoke Apis that are defined in
    /// another project.
    /// </summary>
    public abstract class ProxyController : ControllerBase{

        /// <summary>
        /// The HEAD endpoint is used for testing purposes
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpHead] public virtual void Head() { }

        /// <summary>
        /// Get an array of all IProxy objects
        /// defined in the controller.
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public abstract IApiProxy[] GetApis();

        /// <summary>
        /// Propagates headers from the host application
        /// through HttpClients to a dependent API.
        /// </summary>
        /// <param name="headerToPropagate"></param>
        [ApiExplorerSettings(IgnoreApi = true)]
        public void PropagateHeaders(KeyValuePair<string,string> headerToPropagate) {

            //get the configuration data
            var config = HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

            //get all APIs for the controller
            var apis = GetApis();
            foreach (var api in apis) {

                api.HttpClient.DefaultRequestHeaders.Add(
                    headerToPropagate.Key, headerToPropagate.Value);

                if (headerToPropagate.Key == HDR_DROP_INMEMORY) {

                    var uri = api.ResetUri;

                    var msg = new HttpRequestMessage {
                        RequestUri = uri,
                        Method = HttpMethod.Head
                    };
                    var response = api.HttpClient.SendAsync(msg).Result;
                    break;
                }

            }
        }

    }
}
