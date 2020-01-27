using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc;

namespace Colors2Api.Lib.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RgbController : RepoController<Rgb, Color2DbContext, RgbRepo> {

        public RgbController(RgbRepo repo) 
            : base(repo) { }


        [HttpGet("RgbHslByColorName")]
        public IActionResult GetRgbHslByColorName([FromQuery] string colorName) {

            var parameters = Params.Create(new { colorName });

            var json = Repo.Context.GetJsonObjectFromStoredProcedure(
                "RgbHslByColorName", parameters);

            return new ContentResult { Content = json, ContentType = "application/json", StatusCode = 200 };
        }


        [HttpGet("RgbHslByColorNameContains")]
        public IActionResult GetRgbHslByColorNameContains([FromQuery] string colorNameContains) {

            var parameters = Params.Create(new { colorNameContains });

            var json = Repo.Context.GetJsonArrayFromStoredProcedure(
                "RgbHslByColorNameContains", parameters);

            return new ContentResult { Content = json, ContentType = "application/json", StatusCode = 200 };
        }

    }
}
