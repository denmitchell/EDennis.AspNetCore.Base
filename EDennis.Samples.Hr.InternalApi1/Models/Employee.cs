using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDennis.Samples.Hr.InternalApi1.Models {
    public class Employee : IHasIntegerId {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }

        public List<EmployeePosition> EmployeePositions { get; set; }
    }
}