using Colors2.Models;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Colors2Api.Lib.Controllers {
    public class RgbController : SqlServerWriteableController<Rgb, Color2DbContext> {
        public RgbController(RgbRepo repo, ILogger<SqlServerWriteableController<Rgb, Color2DbContext>> logger) 
            : base(repo, logger) { }

        public override Dictionary<string, Type> StoredProcedureReturnTypes
            => new Dictionary<string, Type> {
                { "HslByColorName", typeof(List<Hsl>) },
                { "RgbJsonByColorName", typeof(List<Rgb>) }
            };
    }
}
