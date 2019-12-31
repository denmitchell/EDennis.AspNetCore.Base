using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace Colors.Models {

    public class ColorRepo : TemporalRepo<
        Color, ColorHistory, ColorDbContext, ColorHistoryDbContext> {
        public ColorRepo(ColorDbContext context, ColorHistoryDbContext historyContext, 
            IScopeProperties scopeProperties, 
            ILogger<ColorRepo> logger,
            IScopedLogger scopedLogger) 
            : base(context, historyContext, scopeProperties, logger, scopedLogger) {
        }
    }
}
