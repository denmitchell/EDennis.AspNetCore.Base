using System.Collections.Generic;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.Colors2Api.Models
{
    public class RgbRepo : WriteableRepo<Rgb, ColorsDbContext> {
        public RgbRepo(ColorsDbContext context, IScopeProperties scopeProperties, IEnumerable<ILogger<WriteableRepo<Rgb, ColorsDbContext>>> loggers) : base(context, scopeProperties, loggers) {
        }
    }
}
