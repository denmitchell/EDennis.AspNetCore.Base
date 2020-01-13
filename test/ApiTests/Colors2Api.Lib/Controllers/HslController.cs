using Colors2.Models;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Colors2Api.Lib.Controllers {
    public class HslController : SqlServerReadonlyController<Hsl, Color2DbContext> {
        public HslController(HslRepo repo, ILogger<SqlServerReadonlyController<Hsl, Color2DbContext>> logger) 
            : base(repo, logger) { }


        [HttpGet("RgbJsonByColorName")]
        public Rgb GetRgbJsonByColorName([FromQuery] string colorName) {
            var parameters = new Params().Add("colorName", colorName);
            var result = Repo.Context.GetSingleFromJsonStoredProcedure<Color2DbContext, Rgb>("RgbJsonByColorName", parameters);
            return result;
        }


    }
}
