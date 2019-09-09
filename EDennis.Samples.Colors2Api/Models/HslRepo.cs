using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.Colors2Api.Models
{
    public class HslRepo : ReadonlyRepo<Hsl, ColorsDbContext> {
        public HslRepo(ColorsDbContext context) 
            : base(context) { }
    }
}
