using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Colors2Api.Lib.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RgbController : SqlServerWriteableController<Rgb, Color2DbContext> {
        public RgbController(RgbRepo repo, ILogger<SqlServerWriteableController<Rgb, Color2DbContext>> logger) 
            : base(repo, logger) { }


        [HttpGet("RgbHslByColorName")]
        public RgbHsl GetRgbHslByColorName([FromQuery] string colorName) {
            var parameters = new Params().Add("colorName", colorName);
            var result = Repo.Context.GetSingleFromStoredProcedure<Color2DbContext,RgbHsl>(
                "RgbHslByColorName", parameters);
            return result;
        }

        [HttpGet("RgbHslByColorNameContains")]
        public List<RgbHsl> GetRgbHslByColorNameContains([FromQuery] string colorName) {
            var parameters = new Params().Add("colorNameContains", colorName);
            var result = Repo.Context.GetSingleFromStoredProcedure<Color2DbContext, List<RgbHsl>>(
                "RgbHslByColorNameContains", parameters);
            return result;
        }

        [HttpPost]
        public override IActionResult Post([FromBody] Rgb rgb)
            => base.Post(rgb);

    }
}
