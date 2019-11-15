using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public interface IScopedLoggerAssignments {
        ConcurrentDictionary<string, LogLevel> Assignments { get; set; }
        ILogger GetLogger(LogLevel level);
    }
}
