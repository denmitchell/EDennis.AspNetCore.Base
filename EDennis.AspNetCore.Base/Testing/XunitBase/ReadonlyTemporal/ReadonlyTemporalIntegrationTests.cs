using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class ReadonlyTemporalIntegrationTests<TStartup> : ReadonlyIntegrationTests<TStartup>
        where TStartup: class {

        protected new string InstanceName { get; } = "readonly-temporal";

        public ReadonlyTemporalIntegrationTests(ITestOutputHelper output, WebApplicationFactory<TStartup> factory)
            : base(output, factory) {
        }

    }
}
