using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    public static class ObjectResultExtensions {
        public static T Object<T>(this ObjectResult value) {
            return (T)(value.Value);
        }

    }

}
