using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public static class Constants {
        public const string HOSTPATH_HEADER = "X-HostPath";
        public const string USER_HEADER = "X-User";
        public const string ROLLBACK_HEADER_KEY = "X-Testing-Rollback";
        public const string ROLLBACK_QUERY_KEY = "x-testing-rollback";
    }
}
