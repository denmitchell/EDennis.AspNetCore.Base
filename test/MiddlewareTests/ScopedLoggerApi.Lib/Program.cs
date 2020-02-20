using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace EDennis.Samples.ScopedLoggerMiddlewareApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "ScopedLoggerApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;
    }
}