using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi {
    public class Person : IHasIntegerId, IHasSysUser {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SysUser { get; set; }
    }
}
