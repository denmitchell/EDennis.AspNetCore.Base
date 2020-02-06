using ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationApi.Lib.Models {
    public class ProjectSetting {

        [Key]
        public int ProjectId { get; set; }
        [Key]
        public int SettingId { get; set; }

        public string SettingValue { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [ForeignKey("SettingId")]
        public Setting Setting { get; set; }

    }
}
