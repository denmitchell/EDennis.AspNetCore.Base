using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDennis.Samples.Hr.InternalApi1.Models {

    public class Position : IHasLongId, IEFCoreTemporalModel {
        public int Id { get; set; }
        public DateTime SysStart { get; set; }
        public string Title { get; set; }
        public bool IsManager { get; set; }
        public DateTime SysEnd { get; set; }
        public string SysUser { get; set; }
        public string SysUserNext { get; set; }

        public List<EmployeePosition> EmployeePositions { get; set; }

    }
}