using EDennis.AspNetCore.Base.EntityFramework;
using System;

namespace EDennis.Samples.TodoApi.Models {
    public class Task : IHasIntegerId {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal PercentComplete { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }
    }
}
