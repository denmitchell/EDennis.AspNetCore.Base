using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base {
    public class AppSettings {
        public string Instruction { get; set; }
        public bool PreAuthentication { get; set; }
    }
}
