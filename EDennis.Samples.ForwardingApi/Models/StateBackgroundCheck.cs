using EDennis.AspNetCore.Base.EntityFramework;
using System;

namespace EDennis.Samples.ForwardingApi.Models {
    public class StateBackgroundCheck : IHasIntegerId {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DateCompleted { get; set; }
        public string Status { get; set; }
    }
}
