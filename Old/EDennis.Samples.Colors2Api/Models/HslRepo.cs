using System.Collections.Generic;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Colors2Api.Models
{
    public class HslRepo : ReadonlyRepo<Hsl, ColorsDbContext> {
        public HslRepo(ColorsDbContext context, IScopeProperties scopeProperties, IEnumerable<ILogger<ReadonlyRepo<Hsl, ColorsDbContext>>> loggers) : base(context, scopeProperties, loggers) {
        }
    }
}
