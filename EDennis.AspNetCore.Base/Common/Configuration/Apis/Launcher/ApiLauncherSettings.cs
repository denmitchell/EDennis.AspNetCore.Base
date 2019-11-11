using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ApiLauncherSettings : ApiBaseSettings {
        public ApiSettingsFacade Facade { get; set; }
        public string ProjectName { get; set; }
        public bool NeedsLaunched { get; set; }

    }
}
