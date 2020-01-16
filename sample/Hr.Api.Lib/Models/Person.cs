using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hr.Api.Models {
    public class Person : IHasIntegerId, IHasSysUser {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string SysUser { get; set; }
    }
}
