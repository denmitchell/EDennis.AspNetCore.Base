using EDennis.AspNetCore.Base.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi {
    public class Position : IHasIntegerId, IHasSysUser {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SysUser { get; set; }

    }
}
