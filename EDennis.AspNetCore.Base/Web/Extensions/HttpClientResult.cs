using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    public class HttpClientResult<T> {

        public int StatusCode { get; set; }
        public T Value { get; set; }


        public ObjectResult ObjectResult() {
            return new ObjectResult(Value) {
                StatusCode = StatusCode
            };
        }

        public StatusCodeResult StatusCodeResult() {
            return new StatusCodeResult(StatusCode);
        }

    }

}
