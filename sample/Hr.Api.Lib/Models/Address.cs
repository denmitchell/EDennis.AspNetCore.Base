using EDennis.AspNetCore.Base.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hr.Api.Models {
    public class Address : IHasIntegerId, IHasSysUser {
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string SysUser { get; set; }

    }
}
