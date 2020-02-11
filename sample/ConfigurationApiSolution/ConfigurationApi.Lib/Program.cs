using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using System;

namespace ConfigurationApi.Lib {
    public class Program : ProgramBase<Startup> {
        public override string ProjectName => "ConfigurationApi";
    }
}


