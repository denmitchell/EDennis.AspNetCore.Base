using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

using static EDennis.AspNetCore.Base.Web.TestingActionFilter;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Configuration;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This class provides a number of extension methods
    /// for sending POST, PUT, GET, and DELETE message and
    /// deserializing the response bodies.  These methods 
    /// are convenience methods that should result in fewer
    /// lines of code required for requests using HttpClient.
    /// </summary>
    public static partial class HttpClientExtensions {

        #region TrySend

        public static List<HttpResponseMessage> TrySend(
            this HttpClient client, List<HttpRequestMessage> messages) {

            var responses = new List<HttpResponseMessage>();

            foreach (var message in messages)
                responses.Add(client.SendAsync(message).Result);

            return responses;
        }

        #endregion

    }

}

