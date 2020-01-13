using Colors2.Models;
using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Colors2Api.Lib.Controllers {
    public class HslController : SqlServerReadonlyController<Hsl, Color2DbContext> {
        public HslController(HslRepo repo, ILogger<SqlServerReadonlyController<Hsl, Color2DbContext>> logger) 
            : base(repo, logger) { }

        public override Dictionary<string, Type> StoredProcedureReturnTypes 
            => new Dictionary<string, Type> {
                { "HslByColorName", typeof(List<Hsl>) },
                { "RgbJsonByColorName", typeof(List<Rgb>) }
            };
    }
}
