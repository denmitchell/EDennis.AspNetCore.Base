using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface IEFCoreTemporalModel : IHasSysUser {
        DateTime SysStart { get; set; }
        DateTime SysEnd { get; set; }
        string SysUserNext { get; set; }
    }
}
