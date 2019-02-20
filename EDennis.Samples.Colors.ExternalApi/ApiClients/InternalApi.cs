using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Colors.ExternalApi.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EDennis.Samples.Colors.ExternalApi {

    public class InternalApi : ApiClient {

        private const string COLOR_URL = "iapi/color";

        public InternalApi(HttpClient client, IConfiguration config, TestHeader testHeader):
            base (client,config,testHeader){ }


        public void Create(Color color) {
            HttpClient.Post(COLOR_URL, color);
        }

        public List<Color> GetColors() {
            var result = HttpClient.Get<List<Color>>(COLOR_URL);
            return result.Value; //second line for easier debugging
        }

        public Color GetColor(int id) {
            var result = HttpClient.Get<Color>($"{COLOR_URL}/{id}");
            return result.Value; //second line for easier debugging
        }

    }
}
