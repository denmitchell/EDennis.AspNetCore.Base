using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {
    public class DeserializableLoadResult<T> {
        public ICollection<T> data { get; set; }
        public int totalCount { get; set; }
        public int groupCount { get; set; }
        public object[] summary { get; set; }
    }
}
