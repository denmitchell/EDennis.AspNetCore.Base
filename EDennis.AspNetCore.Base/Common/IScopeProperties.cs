using System.Collections.Generic;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base {
    public interface IScopeProperties {
        int LoggerIndex { get; set; }
        Claim[] Claims { get; set; }
        Dictionary<string, object> OtherProperties { get; set; }
        string User { get; set; }
        void UpdateLoggerIndex();
    }
}