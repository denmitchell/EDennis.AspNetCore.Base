//using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hr.BlazorApp.Lib.Data.Models {
    public class Address /*: IHasIntegerId, IHasSysUser*/ {
        [Key]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Street address must not exceed 100 characters.")]
        public string StreetAddress { get; set; }

        [StringLength(40, ErrorMessage = "City must not exceed 40 characters.")]
        public string City { get; set; }

        [StringLength(2, MinimumLength = 2,ErrorMessage = "State must be a two-letter abbreviation")]
        public string State { get; set; }

        [RegularExpression("[0-9]{5}", ErrorMessage = "Postal code must be a 5-digit number with leading zeroes, when necessary.")]
        public string PostalCode { get; set; }


        public Person Person { get; set; }
        public int PersonId { get; set; }


        [StringLength(100)]
        public string SysUser { get; set; }

        [DataType("datetime2(7)")]
        public DateTime SysStart { get; set; }

        [DataType("datetime2(7)")]
        public DateTime SysEnd { get; set; }
    }
}
