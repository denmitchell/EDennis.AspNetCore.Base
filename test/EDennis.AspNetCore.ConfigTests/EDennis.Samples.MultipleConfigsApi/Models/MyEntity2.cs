using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.MultipleConfigsApi.Services {
    public class MyEntity2 : IHasSysUser {
        public int MyIntProp { get; set; }
        public string MyStringProp { get; set; }
        public string SysUser { get; set; }
    }
}
