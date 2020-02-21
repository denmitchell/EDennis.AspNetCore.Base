using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace MockClientApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "MockClientApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;
    }
}