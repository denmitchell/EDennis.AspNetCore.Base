using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ApiSettings : ApiBaseSettings {
        public ApiClientSettings Client { get; set; }
        public ApiLauncherSettings Launcher { get; set; }

    }
}
