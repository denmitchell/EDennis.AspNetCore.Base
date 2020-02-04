using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace Colors2.Models {
    public class HslRepo : QueryRepo<Hsl, Color2DbContext> {
        public HslRepo(DbContextProvider<Color2DbContext> provider,
            IScopeProperties scopeProperties)
            : base(provider, scopeProperties) { }
    }
}
