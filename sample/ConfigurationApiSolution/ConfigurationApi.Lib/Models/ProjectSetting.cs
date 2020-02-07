using System;

namespace ConfigurationApi.Lib.Models {
    public class ProjectSetting {

        public string ProjectName { get; set; }
        public string SettingKey { get; set; }

        public string SettingValue { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }

    }
}
