using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// Exception that can be used with a "Not Found" result
    /// </summary>
    public class MissingEntityException : RequestException {

        /// <summary>
        /// Constructs a new MissingEntityException with 
        /// the provided message.  The message should
        /// include the query parameters used to try to
        /// find the entity.
        /// </summary>
        /// <param name="message"></param>
        public MissingEntityException(string title, string message) 
            : base(title, message) {
        }
    }
}
