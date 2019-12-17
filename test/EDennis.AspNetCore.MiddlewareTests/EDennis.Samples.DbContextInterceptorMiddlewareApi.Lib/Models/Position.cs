using EDennis.AspNetCore.Base.EntityFramework;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi {
    public class Position : IHasSysUser {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SysUser { get; set; }

    }
}
