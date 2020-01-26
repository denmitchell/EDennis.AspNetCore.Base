using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {
    public class EndpointTestResult<TResult> {
        public int StatusCode { get; set; }
        public TResult Data { get; set; }
    }
}
