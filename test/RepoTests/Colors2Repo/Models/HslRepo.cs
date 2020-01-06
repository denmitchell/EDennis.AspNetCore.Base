using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace Colors2.Models {
    public class HslRepo : SqlServerRepo<Hsl, Color2DbContext> {
        public HslRepo(DbContextProvider<Color2DbContext> provider,
            IScopeProperties scopeProperties,
            ILogger<HslRepo> logger,
            IScopedLogger scopedLogger)
            : base(provider, scopeProperties, logger, scopedLogger) { }
    }
}
