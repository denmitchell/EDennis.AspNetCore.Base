using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hr.Api.Models {
    public class Address : IHasIntegerId, IHasSysUser {

        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string StreetAddress { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(2, MinimumLength = 2)]
        public string State { get; set; }

        [RegularExpression("[0-9]{5}")]
        public string PostalCode { get; set; }


        public int PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }


        [StringLength(100)]
        public string SysUser { get; set; }

        [DataType("datetime2(7)")]
        public DateTime SysStart { get; set; }

        [DataType("datetime2(7)")]
        public DateTime SysEnd { get; set; }
    }
}
