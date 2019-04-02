using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public abstract class WriteableTemporalEndpointTests<TStartup> : WriteableEndpointTests<TStartup>
        where TStartup : class {

        public WriteableTemporalEndpointTests(ITestOutputHelper output, 
                ConfiguringWebApplicationFactory<TStartup> factory) 
            : base(output,factory) { }

    }
}
