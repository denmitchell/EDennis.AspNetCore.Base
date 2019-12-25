using EDennis.Samples.ApiConfigsApi.Apis;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EDennis.Samples.ApisConfigsApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class Api2Controller : ControllerBase {

        private readonly Api2 _api2;
        public Api2Controller(Api2 api2) {
            _api2 = api2;
        }

        [HttpGet]
        public Dictionary<string,string> Get() {
            return _api2.GetObjects();
        }
    }
}
