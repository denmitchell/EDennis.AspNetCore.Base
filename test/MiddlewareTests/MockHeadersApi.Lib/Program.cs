using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace EDennis.Samples.MockHeadersMiddlewareApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "MockHeadersApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;

    }
}