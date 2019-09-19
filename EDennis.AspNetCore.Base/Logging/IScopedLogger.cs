using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Logging {
    public interface IScopedLogger<T> {
        bool Enabled { get; set; }
        ILogger<T> Logger { get; set; }
    }
}