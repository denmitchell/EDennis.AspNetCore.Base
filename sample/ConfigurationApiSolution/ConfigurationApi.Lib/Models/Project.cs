using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationApi.Lib.Models {
    public class Project {
        [Key]
        public int Id { get; set; }
        public string ProjectName { get; set; }
    }
}
