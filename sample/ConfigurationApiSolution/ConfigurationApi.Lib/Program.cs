using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace ConfigurationApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "ConfigurationApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;
    }
}


