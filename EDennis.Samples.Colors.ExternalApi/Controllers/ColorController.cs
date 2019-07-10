using EDennis.Samples.Colors.ExternalApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EDennis.Samples.Colors.ExternalApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {

        IInternalApi _api;

        public ColorController(IInternalApi api) {
            _api = api;
        }

        [HttpGet]
        public ActionResult<List<Color>> Get() {
            return _api.GetColors();
        }


        [HttpGet("{id}")]
        public ActionResult<Color> Get([FromRoute]int id) {
            return _api.GetColor(id);
        }

        [HttpPost]
        public ActionResult Post(Color color) {
            return _api.Create(color);
        }

        [HttpGet("forward")]
        public ActionResult Get_Forward() {
            return _api.Forward(HttpContext.Request);
        }

    }
}