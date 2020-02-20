using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace EDennis.Samples.ScopePropertiesMiddlewareApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "ScopePropertiesApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;

    }
}