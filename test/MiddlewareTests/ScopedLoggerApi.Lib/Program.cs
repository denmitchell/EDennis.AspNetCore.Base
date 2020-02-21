using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace ScopedLoggerApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "ScopedLoggerApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;
    }
}