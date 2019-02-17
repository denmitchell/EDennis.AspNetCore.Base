using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.Samples.EnternalApi.Models;
using System;
using System.Collections.Generic;

namespace EDennis.Samples.Hr.ExternalApi.Models {
    public class Employee : IHasIntegerId {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }

        public List<EmployeePosition> EmployeePositions { get; set; }
    }
}