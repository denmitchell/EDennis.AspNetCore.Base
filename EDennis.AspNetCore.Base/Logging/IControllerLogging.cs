using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Logging {
    public interface IControllerLogging {
        HttpContext HttpContext { get; }

        public string P() => DefaultPath(this);
        protected static string DefaultPath(IControllerLogging c) => c.HttpContext.Request.Path;

        public string C() => DefaultController(this);
        protected static string DefaultController(IControllerLogging c) => c.HttpContext.Request.RouteValues["controller"].ToString();

        public string A() => DefaultAction(this);
        protected static string DefaultAction(IControllerLogging c) => c.HttpContext.Request.RouteValues["action"].ToString();

        public string U() => DefaultUser(this);
        protected static string DefaultUser(IControllerLogging c) => c.HttpContext.User?.Identity?.Name ?? "unknown";

        public AutoJson D(dynamic data) => DefaultData(this, data);
        protected static AutoJson DefaultData(IControllerLogging c, dynamic data) => new AutoJson(data);

    }
}
