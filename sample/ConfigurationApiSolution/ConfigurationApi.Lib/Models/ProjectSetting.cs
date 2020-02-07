using System;

namespace ConfigurationApi.Lib.Models {
    public class ProjectSetting {

        public int ProjectId { get; set; }
        public int SettingId { get; set; }

        public string SettingValue { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }

        public Project Project { get; set; }

        public Setting Setting { get; set; }

    }
}
