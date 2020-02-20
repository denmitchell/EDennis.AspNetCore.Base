using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace PkRewriterApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "PkRewriterApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;

    }
}