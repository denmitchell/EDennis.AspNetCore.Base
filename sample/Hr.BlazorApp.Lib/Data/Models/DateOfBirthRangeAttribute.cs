using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hr.BlazorApp.Lib.Data.Models {
    public class DateOfBirthRangeAttribute : RangeAttribute {
        public DateOfBirthRangeAttribute()
          : base(typeof(DateTime),
                  DateTime.Today.AddYears(-130).ToShortDateString(),
                  DateTime.Today.ToShortDateString()) { }
    }
}
