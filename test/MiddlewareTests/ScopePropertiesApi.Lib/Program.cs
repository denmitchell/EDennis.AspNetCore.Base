using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace ScopePropertiesApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "ScopePropertiesApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;

    }
}