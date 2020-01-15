using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.Extensions.Logging;

namespace Colors2.Models {
    public class RgbRepo : Repo<Rgb, Color2DbContext> {

        public RgbRepo(DbContextProvider<Color2DbContext> provider,
            IScopeProperties scopeProperties,
            ILogger<RgbRepo> logger)
            : base(provider, scopeProperties, logger) {

        }
    }
}
