using EDennis.Samples.Colors.ExternalApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EDennis.Samples.Colors.ExternalApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {

        InternalApi _api;

        public ColorController(InternalApi api) {
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
        public void Post(string colorName) {
            _api.Create(new Color { Name = colorName });
        }

    }
}