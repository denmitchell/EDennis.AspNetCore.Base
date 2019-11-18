using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public static class Constants {
        public const string HOSTPATH_KEY = "X-HostPath";
        public const string USER_KEY = "X-User";
        public const string ROLE_KEY = "X-Role";
        public const string TESTING_INSTANCE_KEY = "X-Testing-Instance";
        public const string RESET_METHOD = "Reset";
        public const string SET_SCOPEDLOGGER_KEY = "X-Set-ScopedLogger";
        public const string CLEAR_SCOPEDLOGGER_KEY = "X-Clear-ScopedLogger";
    }
}
