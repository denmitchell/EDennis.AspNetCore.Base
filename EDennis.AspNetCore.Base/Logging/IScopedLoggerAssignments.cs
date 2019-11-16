using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace EDennis.AspNetCore.Base.Logging {
    public interface IScopedLoggerAssignments {
        ConcurrentDictionary<string, LogLevel> Assignments { get; set; }
        ILogger GetLogger(LogLevel level);
    }
}
