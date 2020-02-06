using System.ComponentModel.DataAnnotations;

namespace ConfigurationApi.Lib.Models {
    public class Setting {
        [Key]
        public int Id { get; set; }
        public string SettingKey { get; set; }
    }
}
