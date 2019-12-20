using EDennis.Samples.ApiConfigsApi.Apis;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EDennis.Samples.ApisConfigsApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class Api1Controller : ControllerBase {

        private readonly Api1 _api1;
        public Api1Controller(Api1 api1) {
            _api1 = api1;
        }

        [HttpGet]
        public Dictionary<string,string> Get() {

            return _api1.GetObjects();
        }
    }
}
