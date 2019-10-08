using System.Collections.Generic;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Colors.InternalApi.Models {

    public class ColorRepo : WriteableTemporalRepo<Color, ColorDbContext, ColorHistoryDbContext> {
        public ColorRepo(ColorDbContext context, ColorHistoryDbContext historyContext, IScopeProperties scopeProperties, IEnumerable<ILogger<WriteableRepo<Color, ColorDbContext>>> loggers) : base(context, historyContext, scopeProperties, loggers) {
        }
    }
}
