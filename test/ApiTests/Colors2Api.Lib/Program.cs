using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace Colors2Api.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "Colors2Api";

        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;
    }
}
