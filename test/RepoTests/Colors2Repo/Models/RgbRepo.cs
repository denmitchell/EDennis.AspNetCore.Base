using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace Colors2.Models
{
    public class RgbRepo : SqlServerRepo<Rgb, Color2DbContext> {
        public RgbRepo(Color2DbContext context, 
            IScopeProperties scopeProperties, 
            ILogger<RgbRepo> logger, 
            IScopedLogger scopedLogger) 
            : base(context, scopeProperties, logger, scopedLogger) {}
    }
}
