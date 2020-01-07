using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ExplicitIdSettings {
        public string IdFieldName { get; set; } = "Id";
        public int BaseId { get; set; } = -999001;
    }
}
