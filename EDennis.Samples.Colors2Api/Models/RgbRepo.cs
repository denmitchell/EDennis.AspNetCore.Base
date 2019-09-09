using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.Colors2Api.Models
{
    public class RgbRepo : WriteableRepo<Rgb, ColorsDbContext> {
        public RgbRepo(ColorsDbContext context, ScopeProperties scopeProperties) 
            : base(context, scopeProperties) { }
    }
}
