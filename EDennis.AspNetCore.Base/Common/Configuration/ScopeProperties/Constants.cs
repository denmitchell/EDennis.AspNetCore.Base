using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public static class Constants {
        public const string HOSTPATH_REQUEST_KEY = "X-HostPath";
        public const string USER_REQUEST_KEY = "X-User";
        public const string ROLLBACK_REQUEST_KEY = "X-Testing-Rollback";
        public const string SET_SCOPEDLOGGER_REQUEST_KEY = "X-Set-ScopedLogger";
        public const string CLEAR_SCOPEDLOGGER_REQUEST_KEY = "X-Clear-ScopedLogger";
    }
}
