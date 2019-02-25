using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.Collections.Generic;

namespace EDennis.Samples.Colors.InternalApi.Models
{
    public partial class Color : IHasIntegerId, IEFCoreTemporalModel
    {
        public int Id { get; set; }
        public DateTime SysStart { get; set; } //NOTE: ALWAYS POSITION SysStart immediately after the primary key
        public string Name { get; set; }
        public DateTime SysEnd { get; set; }
        public string SysUser { get; set; }
        public string SysUserNext { get; set; }
    }
}
