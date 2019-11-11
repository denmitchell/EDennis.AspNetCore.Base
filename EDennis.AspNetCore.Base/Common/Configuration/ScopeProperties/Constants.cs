using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public static class Constants {
        public const string HOSTPATH_HEADER_KEY = "X-HostPath";
        public const string USER_HEADER_KEY = "X-User";
        public const string USER_QUERY_KEY = "x-user";
        public const string ROLLBACK_HEADER_KEY = "X-Testing-Rollback";
        public const string ROLLBACK_QUERY_KEY = "x-testing-rollback";
    }
}
