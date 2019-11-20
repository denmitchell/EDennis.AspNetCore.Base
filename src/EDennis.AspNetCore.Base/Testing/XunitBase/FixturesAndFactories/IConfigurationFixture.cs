using Microsoft.Extensions.Configuration;

namespace EDennis.AspNetCore.Base.Testing {
    public interface IConfigurationFixture {
        IConfiguration Configuration { get; }

        void Dispose();
    }
}