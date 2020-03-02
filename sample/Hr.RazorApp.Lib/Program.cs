using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Hr.RazorApp.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "Hr.RazorApp";

        public override bool UsesProjectRoot => true;

        public override ConfigurationType ConfigurationType => ConfigurationType.ConfigurationApi;
    }
}
