using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Colors.ExternalApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.Samples.Colors.ExternalApi {

    public class InternalApi : ApiClient, IInternalApi {

        private const string COLOR_URL = "iapi/color";

        public InternalApi(HttpClient client, IConfiguration config, ScopeProperties scopeProperties):
            base (client,config,scopeProperties){ }


        public HttpClientResult<Color> Create(Color color) {
            return HttpClient.Post(COLOR_URL, color);
        }

        public HttpClientResult<List<Color>> GetColors() {
            return HttpClient.Get<List<Color>>(COLOR_URL);
        }

        public HttpClientResult<Color> GetColor(int id) {
            return HttpClient.Get<Color>($"{COLOR_URL}/{id}");
        }

        public HttpClientResult<Color> Forward(HttpRequest request) {
            return HttpClient.Forward<Color>(request, $"{COLOR_URL}/forward");
        }


    }
}
