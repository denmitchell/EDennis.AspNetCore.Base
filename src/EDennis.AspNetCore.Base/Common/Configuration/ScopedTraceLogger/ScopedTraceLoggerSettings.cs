using Microsoft.Extensions.Configuration;

namespace EDennis.AspNetCore.Base {
    public class ScopedTraceLoggerSettings {
        public bool Enabled { get; set; } = false;
        public AssignmentKeySource AssignmentKeySource { get; set; } = AssignmentKeySource.User;
        public string AssignmentKeyName { get; set; }
    }
}
