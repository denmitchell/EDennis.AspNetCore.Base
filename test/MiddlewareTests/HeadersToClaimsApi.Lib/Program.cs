using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace EDennis.Samples.HeadersToClaimsMiddlewareApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "HeadersToClaimsApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;
    }
}