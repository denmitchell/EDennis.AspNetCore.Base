using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.Colors.InternalApi.Models {

    public class ColorRepo : WriteableTemporalRepo<Color,ColorDbContext,ColorHistoryDbContext> {

        public ColorRepo(ColorDbContext context,ColorHistoryDbContext historyContext, ScopeProperties scopeProperties) 
            : base(context, historyContext, scopeProperties) {}

    }
}
