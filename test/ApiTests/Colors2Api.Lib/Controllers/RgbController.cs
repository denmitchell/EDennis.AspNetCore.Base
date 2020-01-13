using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Colors2Api.Lib.Controllers {
    public class RgbController : SqlServerWriteableController<Rgb, Color2DbContext> {
        public RgbController(RgbRepo repo, ILogger<SqlServerWriteableController<Rgb, Color2DbContext>> logger) 
            : base(repo, logger) { }


        [HttpGet("HslByColorName")]
        public Hsl GetHslByColorName([FromQuery] string colorName) {
            var parameters = new Params().Add("colorName", colorName);
            var result = Repo.Context.GetSingleFromStoredProcedure<Color2DbContext,Hsl>("HslByColorName", parameters);
            return result;
        }

    }
}
