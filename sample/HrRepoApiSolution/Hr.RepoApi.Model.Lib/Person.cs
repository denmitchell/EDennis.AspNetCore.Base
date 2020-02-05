using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hr.Api.Models {
    public class Person : IHasIntegerId, IHasSysUser {

        [Key]
        public int Id { get; set; }
        
        [StringLength(40)]
        public string FirstName { get; set; }
        
        [StringLength(40)]
        public string LastName { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [StringLength(100)]
        public string SysUser { get; set; }

        [DataType("datetime2(7)")]
        public DateTime SysStart { get; set; }

        [DataType("datetime2(7)")]
        public DateTime SysEnd { get; set; }
    }
}
