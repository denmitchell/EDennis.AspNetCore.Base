using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.Samples.EnternalApi.Models;
using System;
using System.Collections.Generic;

namespace EDennis.Samples.ExternalApi.Models {

    public class Position : IHasIntegerId {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsManager { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }

        public List<EmployeePosition> EmployeePositions { get; set; }

    }
}