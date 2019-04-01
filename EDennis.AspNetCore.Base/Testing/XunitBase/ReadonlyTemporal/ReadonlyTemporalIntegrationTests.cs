using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class ReadonlyTemporalIntegrationTests<TStartup> 
            : ReadonlyIntegrationTests<TStartup>
        where TStartup: class {

        protected new string InstanceName { get; }

        public ReadonlyTemporalIntegrationTests(ITestOutputHelper output, 
                ConfiguringWebApplicationFactory<TStartup> factory)
            : base(output, factory) {
        }


    }
}
