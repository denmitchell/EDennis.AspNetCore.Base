using EDennis.AspNetCore.Base.EntityFramework;
using System;

namespace EDennis.Samples.ExternalApi.Models {
    public class FederalBackgroundCheck : IHasIntegerId {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DateCompleted { get; set; }
        public string Status { get; set; }
    }
}
