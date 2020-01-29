using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EDennis.AspNetCore.Base.Testing {
    public class RepoTestResult<TResult> {
        /// <summary>
        /// Either the direct result of an operation under test or the 
        /// result of a follow-up query after a test operation.
        /// </summary>
        public TResult Data { get; set; }
        /// <summary>
        /// Simple class name of the exception thrown as a result of the test
        /// operation with the provided parameters
        /// </summary>
        public string Exception { get; set; }
    }
}
