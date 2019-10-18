using Microsoft.Extensions.Logging;
namespace EDennis.AspNetCore.Base.Logging {
    public interface IHasILogger {
        public ILogger Logger { get; }
    }
}
