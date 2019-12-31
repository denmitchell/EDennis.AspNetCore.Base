using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Colors2Repo.Models {
    public class HslRepo : SqlServerRepo<Hsl, ColorsDbContext> {
        public HslRepo(ColorsDbContext context,
            IScopeProperties scopeProperties,
            ILogger<HslRepo> logger,
            IScopedLogger scopedLogger)
            : base(context, scopeProperties, logger, scopedLogger) { }
    }
}
