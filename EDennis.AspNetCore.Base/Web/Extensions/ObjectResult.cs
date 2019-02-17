using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public class ObjectResult<T> {

        public ObjectResult() { }


        public HttpStatusCode StatusCode { get; set; }
        public T Value { get; set; }

        public int StatusCodeValue {
            get {
                return (int)StatusCode;
            }
        }

    }

}
