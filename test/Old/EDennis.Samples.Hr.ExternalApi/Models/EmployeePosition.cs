using EDennis.Samples.Hr.ExternalApi.Models;
using System;

namespace EDennis.Samples.EnternalApi.Models {
    public class EmployeePosition {
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }

        public Employee Employee { get; set; }
        public Position Position { get; set; }
    }
}