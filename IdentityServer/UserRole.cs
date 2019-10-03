using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer {
    public class UserRole {
        public string SubjectId { get; set; }
        public string AppName { get; set; }
        public string RoleName { get; set; }
    }
}
