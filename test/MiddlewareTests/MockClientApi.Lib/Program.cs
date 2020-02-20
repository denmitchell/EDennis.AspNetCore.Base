using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace EDennis.Samples.MockClientMiddlewareApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "MockClientApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;
    }
}