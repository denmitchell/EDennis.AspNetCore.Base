using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web
{
    public class DynamicLinqSpec
    {
        public string Where { get; set; }
        public string OrderBy { get; set; }
        public string Select { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }

    }
}
