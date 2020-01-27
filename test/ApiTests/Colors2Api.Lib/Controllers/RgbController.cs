using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Colors2Api.Lib.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RgbController : RepoController<Rgb, Color2DbContext, RgbRepo> {

        public RgbController(RgbRepo repo) 
            : base(repo) { }


        [HttpGet("RgbByColorName")]
        public IActionResult GetRgbByColorName([FromQuery] string colorName) {

            var parameters = Params.Create(new { colorName });

            var json = Repo.Context.GetJsonObjectFromStoredProcedure(
                "RgbByColorName", parameters);

            return new ContentResult { Content = json, ContentType = "application/json", StatusCode = 200 };
        }


        [HttpGet("RgbByColorNameContains")]
        public IActionResult GetRgbByColorNameContains([FromQuery] string colorNameContains) {

            var parameters = Params.Create(new { colorNameContains });

            var json = Repo.Context.GetJsonArrayFromStoredProcedure(
                "RgbByColorNameContains", parameters);

            return new ContentResult { Content = json, ContentType = "application/json", StatusCode = 200 };
        }

    }
}
