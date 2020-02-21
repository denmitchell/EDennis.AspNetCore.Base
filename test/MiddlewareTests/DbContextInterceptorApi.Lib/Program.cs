using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace DbContextInterceptorApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "DbContextInterceptorApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ManifestedEmbeddedFiles;
    }
}