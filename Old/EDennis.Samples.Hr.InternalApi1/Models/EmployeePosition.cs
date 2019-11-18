using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDennis.Samples.Hr.InternalApi1.Models {
    public class EmployeePosition : IEFCoreTemporalModel {
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }
        public string SysUser { get; set; }
        public string SysUserNext { get; set; }

        public Employee Employee { get; set; }
        public Position Position { get; set; }
    }
}