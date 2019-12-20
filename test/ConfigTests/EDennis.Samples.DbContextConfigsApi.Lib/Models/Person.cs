using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.DbContextConfigsApi {
    public class Person : IHasSysUser {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SysUser { get; set; }
    }
}
