using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace Colors.Models {

    public class ColorRepo : TemporalRepo<
        Color, ColorHistory, ColorDbContext, ColorHistoryDbContext> {
        public ColorRepo(DbContextProvider<ColorDbContext> provider, DbContextProvider<ColorHistoryDbContext> historyContext, 
            IScopeProperties scopeProperties) 
            : base(provider, historyContext, scopeProperties) {
        }
    }
}
