using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Colors2Repo.Models
{
    public class RgbRepo : SqlServerRepo<Rgb, ColorsDbContext> {
        public RgbRepo(ColorsDbContext context, 
            IScopeProperties scopeProperties, 
            ILogger<RgbRepo> logger, 
            IScopedLogger scopedLogger) 
            : base(context, scopeProperties, logger, scopedLogger) {}
    }
}
