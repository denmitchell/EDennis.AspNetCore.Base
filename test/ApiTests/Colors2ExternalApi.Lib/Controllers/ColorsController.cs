using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using APIS = Colors2ExternalApi.Lib.ApiClients;

namespace Colors2Api.Lib.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase {

        APIS.Colors2Api _api;

        public ColorsController(APIS.Colors2Api api) {
            _api = api;
        }

        [HttpGet("rgb/{id}")]
        public ObjectResult<Rgb> GetRgb(int id) {
            return _api.Rgb.Get(id);
        }

        [HttpGet("hsl/{id}")]
        public ObjectResult<Hsl> GetHsl(int id) {
            return _api.Hsl.Get(id);
        }

        [HttpGet("rgbid/{colorName}")]
        public ObjectResult<int> GetRgbId(string colorName) {
            return _api.GetScalarFromStoredProcedure<Color2DbContext, int>("RgbInt", new Dictionary<string, string> { { "colorName", colorName } });
        }

        [HttpDelete("rgb/{id}")]
        public StatusCodeResult DeleteRgb(int id) {
            return _api.Rgb.Delete(id);
        }

    }
}
