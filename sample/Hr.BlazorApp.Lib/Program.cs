using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;

namespace Hr.BlazorApp.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "Hr.BlazorApp";

        public override bool UsesProjectRoot => true;

        public override ConfigurationType ConfigurationType => ConfigurationType.ConfigurationApi;

    }
}
