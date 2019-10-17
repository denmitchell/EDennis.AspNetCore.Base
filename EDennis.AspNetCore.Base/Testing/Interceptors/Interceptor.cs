using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.AspNetCore.Base.Testing {

    public class Interceptor {

        protected readonly RequestDelegate _next;
        public const string HDR_PREFIX = "X-Testing-";
        public const string DEFAULT_NAMED_INSTANCE = "default";

        public const string HISTORY_INSTANCE_SUFFIX = "-hist";

        public const string HDR_USE_READONLY = HDR_PREFIX + "UseReadonly";
        public const string HDR_USE_RELATIONAL = HDR_PREFIX + "UseRelational";

        public const string HDR_USE_INMEMORY = HDR_PREFIX + "UseInMemory";
        public const string HDR_DROP_INMEMORY = HDR_PREFIX + "DropInMemory";

        public Interceptor(RequestDelegate next) {
            _next = next;
        }


        /// <summary>
        /// Retrieves the X-Testing- header from the request message
        /// </summary>
        /// <param name="context">provides access to the Request headers</param>
        /// <returns></returns>
        protected KeyValuePair<string, string> GetTestingHeader(HttpContext context) {

            //retrieve a list of all X-Testing- headers 
            var headers = context.Request.Headers
                            .Where(h => h.Key.StartsWith(HDR_PREFIX));

            //if there is no X-Testing- header, return a default KeyValuePair
            if (headers.Count() == 0)
                return new KeyValuePair<string, string>(null, null);

            //if there are duplicate headers, throw an exception
            else if (headers.Count() > 1)
                throw new DuplicateHeaderException(headers);

            //set a reference to the X-Testing- header
            var header = headers.FirstOrDefault();

            //set a reference to the X-Testing- header and first value
            var simpleHeader = new KeyValuePair<string, string>(header.Key,
                header.Value.FirstOrDefault());

            //return the header
            return simpleHeader;
        }

    }
}
