using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {
    public class ProjectPortAssignment {
        public decimal Version { get; set; }
        public int HttpsPort { get; set; }
        public int HttpPort { get; set; }
        public bool AlreadyAssigned { get; set; }
    }
}
