using EDennis.AspNetCore.Base.EntityFramework;
using System;

namespace EDennis.Samples.Hr.InternalApi2.Models {
    public class AgencyInvestigatorCheck : IHasIntegerId, IEFCoreTemporalModel {
        public int Id { get; set; }
        public DateTime SysStart { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DateCompleted { get; set; }
        public string Status { get; set; }
        public DateTime SysEnd { get; set; }
        public string SysUser { get; set; }
        public string SysUserNext { get; set; }
    }
}
