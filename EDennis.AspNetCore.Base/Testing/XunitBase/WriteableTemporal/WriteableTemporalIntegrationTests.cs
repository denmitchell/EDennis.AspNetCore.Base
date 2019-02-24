using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class WriteableTemporalIntegrationTests<TStartup> : WriteableIntegrationTests<TStartup>
        where TStartup : class {

        public WriteableTemporalIntegrationTests(ITestOutputHelper output, WebApplicationFactory<TStartup> factory) 
            : base(output,factory) { }

    }
}
