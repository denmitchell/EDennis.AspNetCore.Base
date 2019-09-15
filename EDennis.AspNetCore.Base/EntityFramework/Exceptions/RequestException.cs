
using Microsoft.AspNetCore.Mvc;
using System;

namespace EDennis.AspNetCore.Base.EntityFramework {

    public class RequestException : ApplicationException {

        /// <summary>
        /// Non-instance-specific general label for the error.
        /// </summary>
        public string Title { get; set; }

        public RequestException(string message, string title) : base(message) {
            Title = title;
        }

    }
}