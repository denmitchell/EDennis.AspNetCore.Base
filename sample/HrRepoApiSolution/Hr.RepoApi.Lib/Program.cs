using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace Hr.RepoApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "Hr.RepoApi";
        public override ConfigurationType ConfigurationType => ConfigurationType.ConfigurationApi;
    }
}
