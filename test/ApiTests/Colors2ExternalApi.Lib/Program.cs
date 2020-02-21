using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace Colors2ExternalApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "Colors2ExternalApi";

        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;
    }
}
