using EDennis.AspNetCore.Base.EntityFramework;
using System;

namespace EDennis.Samples.Hr.ExternalApi.Models {
    public class StateBackgroundCheck : IHasLongId {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DateCompleted { get; set; }
        public string Status { get; set; }
    }
}
