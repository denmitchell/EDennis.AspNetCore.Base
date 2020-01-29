using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {
    public class EndpointTestResult<TResult> {
        /// <summary>
        /// Either the direct result of an endpoint under test or the 
        /// result of a follow-up endpoint query after a test operation.
        /// </summary>
        public TResult Data { get; set; }
        /// <summary>
        /// Status code returned by the endpoint under test
        /// </summary>
        public int StatusCode { get; set; }
    }
}
