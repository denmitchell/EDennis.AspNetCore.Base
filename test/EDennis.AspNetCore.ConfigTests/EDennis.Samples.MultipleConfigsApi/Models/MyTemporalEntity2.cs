using System;
using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.MultipleConfigsApi.Services {
    public class MyTemporalHistoryEntity2 : MyTemporalEntity2 { }
    public class MyTemporalEntity2 : IEFCoreTemporalModel {
        public int MyIntProp { get; set; }
        public string MyStringProp { get; set; }
        public string SysUser { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime SysEnd { get; set; }
        public string SysUserNext { get; set; }
    }
}
