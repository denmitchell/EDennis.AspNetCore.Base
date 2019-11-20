using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Colors.ExternalApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.Samples.Colors.ExternalApi {

    public class InternalApi : ApiClient, IInternalApi {

        private const string COLOR_URL = "iapi/color";

        public InternalApi(HttpClient client, IConfiguration config, ScopeProperties22 scopeProperties, ILogger<InternalApi> logger):
            base (client,config,scopeProperties, logger){ }


        public ObjectResult Create(Color color) {
            return HttpClient.Post(COLOR_URL, color);
        }

        public ObjectResult GetColors() {
            return HttpClient.Get<List<Color>>(COLOR_URL);
        }

        public ObjectResult GetColor(int id) {
            return HttpClient.Get<Color>($"{COLOR_URL}/{id}");
        }

        public ObjectResult Forward(HttpRequest request) {
            return HttpClient.Forward<Color>(request, $"{COLOR_URL}/forward");
        }


    }
}
