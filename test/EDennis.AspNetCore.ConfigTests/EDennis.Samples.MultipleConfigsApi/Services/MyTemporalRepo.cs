using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.MultipleConfigsApi.Services {
    public class MyTemporalRepo : TemporalRepo<MyTemporalEntity1,
        MyTemporalHistoryEntity1,MyTemporalDbContext,MyTemporalHistoryDbContext> {
        public MyTemporalRepo(MyTemporalDbContext context, 
            MyTemporalHistoryDbContext historyDbContext, IScopeProperties scopeProperties, 
            ILogger<TemporalRepo<MyTemporalEntity1, MyTemporalHistoryEntity1, 
                MyTemporalDbContext, MyTemporalHistoryDbContext>> logger, 
                IScopedLogger scopedLogger) : base(context, historyDbContext, scopeProperties, logger, scopedLogger) {
        }
    }
}
