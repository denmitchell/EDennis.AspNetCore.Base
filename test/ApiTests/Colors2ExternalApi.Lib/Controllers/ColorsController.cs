using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using APIS = Colors2ExternalApi.Lib.ApiClients;

namespace Colors2Api.Lib.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase {
        readonly APIS.Colors2Api _api;

        public ColorsController(APIS.Colors2Api api) {
            _api = api;
        }

        [HttpGet("rgb/{id}")]
        public ObjectResult<Rgb> GetRgb(int id) {
            return _api.Rgb.Get(id);
        }

        [HttpDelete("rgb/{id}")]
        public StatusCodeResult DeleteRgb(int id) {
            return _api.Rgb.Delete(id);
        }

        [HttpGet("hsl/linq")]
        public ObjectResult<DynamicLinqResult<Hsl>> GetHsl(string where,
            string orderBy, int? skip, int? take
            ) {
            return _api.Hsl.GetWithDynamicLinq(where, orderBy, skip, take);
        }

        [HttpGet("rgbid")]
        public ObjectResult<int> GetRgbId([FromQuery]string colorName) {
            return _api.GetScalarFromStoredProcedure<Color2DbContext, int>(
                "RgbInt", new Dictionary<string, string> { 
                    { "colorName", colorName },
                    { "returnType", "int" } 
                });
        }


    }
}
