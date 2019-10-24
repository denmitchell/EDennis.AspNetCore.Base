using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base.Logging {
    public abstract class LoggableControllerBase<T> : ControllerBase {

        /// <summary>
        /// Request Path
        /// </summary>
        public string Path { get { return HttpContext.Request.Path; } }

        /// <summary>
        /// Controller Name
        /// </summary>
        public string Controller { get { return HttpContext.Request.RouteValues["controller"].ToString(); } }

        /// <summary>
        /// Action (Method) Name
        /// </summary>
        public string Action { get { return HttpContext.Request.RouteValues["action"].ToString(); } }

        /// <summary>
        /// User (from ScopeProperties, if present)
        /// </summary>
        public new string User { get { return ScopeProperties.User ?? HttpContext.User?.Identity?.Name ?? "unknown"; } }

        /// <summary>
        /// Data that serializes to Json upon call to ToString()
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public AutoJson Data(dynamic data) => new AutoJson(data);

        /// <summary>
        /// Request Path
        /// </summary>
        public string P { get { return HttpContext.Request.Path; } }

        /// <summary>
        /// Controller Name
        /// </summary>
        public string C { get { return HttpContext.Request.RouteValues["controller"].ToString(); } }

        /// <summary>
        /// Action (Method) Name
        /// </summary>
        public string A { get { return HttpContext.Request.RouteValues["action"].ToString(); } }

        /// <summary>
        /// User (from ScopeProperties, if present)
        /// </summary>
        public string U { get { return ScopeProperties.User ?? HttpContext.User?.Identity?.Name ?? "unknown"; } }


        /// <summary>
        /// Data that serializes to Json upon call to ToString()
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public AutoJson D(dynamic data) => new AutoJson(data);

        protected ILogger Logger;
        protected ScopeProperties22 ScopeProperties;

        public LoggableControllerBase(
            IEnumerable<ILogger<T>> loggers,
            ScopeProperties22 scopeProperties) {
            Logger = loggers.ElementAt(scopeProperties.LoggerIndex);
            ScopeProperties = scopeProperties;
        }

    }
}
