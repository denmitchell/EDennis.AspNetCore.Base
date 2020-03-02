//using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hr.BlazorApp.Lib.Data.Models {
    public class Person /*: IHasIntegerId, IHasSysUser*/ {

        [Key]
        public int Id { get; set; }
        
        [StringLength(40, ErrorMessage ="First name must not exceed 40 characters.")]
        public string FirstName { get; set; }
        
        [StringLength(40, ErrorMessage = "Last name must not exceed 40 characters.")]
        public string LastName { get; set; }
        
        [DateOfBirthRange(ErrorMessage ="Date of birth must not be any earlier than 135 years ago and no later than today.")]
        public DateTime DateOfBirth { get; set; }
        
        [StringLength(100)]
        public string SysUser { get; set; }

        [DataType("datetime2(7)")]
        public DateTime SysStart { get; set; }

        [DataType("datetime2(7)")]
        public DateTime SysEnd { get; set; }

        public IEnumerable<Address> Addresses { get; set; }
    }
}
