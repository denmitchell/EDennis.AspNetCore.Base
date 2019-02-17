using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// This class is used to identify HTTP messages
    /// that have the same header key prefix represented more
    /// than once.
    /// </summary>
    public class DuplicateHeaderException : ArgumentException{

        /// <summary>
        /// Constructs a new DuplicateHeaderException object
        /// with the provided listed headers
        /// </summary>
        /// <param name="headers">The headers containing duplicates</param>
        public DuplicateHeaderException(
            IEnumerable<KeyValuePair<string, StringValues>> headers) :
            base(FlattenHeaders(headers)) { }


        /// <summary>
        /// Constructs a new DuplicateHeaderException object
        /// with the provided listed headers
        /// </summary>
        /// <param name="header">The header containing duplicate values</param>
        public DuplicateHeaderException(
            KeyValuePair<string, StringValues> header) :
            base(FlattenHeaders(header)) { }

        /// <summary>
        /// Builds a string of all duplicate headers
        /// </summary>
        /// <param name="headers">headers</param>
        /// <returns>a single string representation of the headers</returns>
        private static string FlattenHeaders(IEnumerable<KeyValuePair<string, StringValues>> headers) {
            var sb = new StringBuilder();
            foreach(var kv in headers) {
                sb.Append($"Key:{kv.Key},Value:{kv.Value};");
            }
            return $"More than one testing header was detected ({sb.ToString()})";
        }

        /// <summary>
        /// Builds a string of all duplicate headers
        /// </summary>
        /// <param name="headers">headers</param>
        /// <returns>a single string representation of the headers</returns>
        private static string FlattenHeaders(KeyValuePair<string, StringValues> header) {
            var sb = new StringBuilder();
            sb.Append($"Key:{header.Key}");
            foreach (var v in header.Value) {
                sb.Append($",Value:{v}");
            }
            sb.Append(";");
            return $"More than one testing header value was detected ({sb.ToString()})";
        }
    }
}
