using EDennis.AspNetCore.Base.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbContextInterceptorApi {
    public class Person : IHasIntegerId, IHasSysUser {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SysUser { get; set; }
    }
}
