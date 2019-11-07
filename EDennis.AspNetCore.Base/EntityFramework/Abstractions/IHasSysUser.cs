using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {
    /// <summary>
    /// All entities that have a key backed by
    /// a sequence or identity field are expected
    /// to implement this interface.
    /// </summary>
    public interface IHasSysUser {
        string SysUser { get; set; }
    }
}
