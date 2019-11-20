using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.MultipleConfigsApi.Services {
    public class MyRepo : Repo<MyEntity1, MyDbContext> {
        public MyRepo(MyDbContext context, IScopeProperties scopeProperties, ILogger<Repo<MyEntity1, MyDbContext>> logger, IScopedLogger scopedLogger) : base(context, scopeProperties, logger, scopedLogger) {
        }
    }
}
