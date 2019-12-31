using System.Collections.Generic;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Colors.InternalApi.Models {

    public class ColorRepo : WriteableTemporalRepo<Color, ColorDbContext, ColorHistoryDbContext> {
        public ColorRepo(ColorDbContext context, ColorHistoryDbContext historyContext, 
            ScopeProperties22 scopeProperties, 
            ILogger<WriteableRepo<Color, ColorDbContext>> logger) 
            : base(context, historyContext, scopeProperties, logger) {
        }
    }
}
