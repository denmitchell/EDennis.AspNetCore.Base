using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class ReadonlyTemporalEndpointTests<TStartup> 
            : ReadonlyEndpointTests<TStartup>
        where TStartup: class {

        protected new string InstanceName { get; }

        public ReadonlyTemporalEndpointTests(ITestOutputHelper output, 
                ConfiguringWebApplicationFactory<TStartup> factory)
            : base(output, factory) {
        }


    }
}
