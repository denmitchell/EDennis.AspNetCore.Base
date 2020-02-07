using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConfigurationApi.Lib.Models {
    public class Setting {
        public int Id { get; set; }
        public string SettingKey { get; set; }

        public List<ProjectSetting> ProjectSettings { get; set; }
    }
}
