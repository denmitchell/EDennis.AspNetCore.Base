using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.AspNetCore.Base.Security {

    /// <summary>
    /// This obviates the need for adding an [Authorize(SomePolicy)] attribute
    /// to the controller and actions
    /// </summary>
    public class DefaultAuthorizationPolicyConvention : IControllerModelConvention, IPageApplicationModelConvention {

        private readonly string _appName;
        private readonly IConfiguration _config;

        public DefaultAuthorizationPolicyConvention(string appName, IConfiguration config) {
            _appName = appName;
            _config = config;
        }

        public void Apply(ControllerModel controller) {

            //don't add Filter if AllowAnonymousFilter is already added
            if (controller.Filters.Any(f => f.GetType() == typeof(AllowAnonymousFilter)))
                return;

            var controllerPath = _appName.Replace(".Lib","") + '.' + controller.ControllerName;

            int i = 0;
            if (_config.ContainsKey("DefaultPolicies")) {
                var dfCurr = new List<string>();
                _config.GetSection("DefaultPolicies").Bind(dfCurr);
                i = dfCurr.Count();
            }
            foreach (var action in controller.Actions) {
                var actionPath = controllerPath + '.' + action.ActionName;
                action.Filters.Add(new AuthorizeFilter(actionPath));
                _config[$"DefaultPolicies:{i}"] = actionPath;
                i++;
            }
        }

        public void Apply(PageApplicationModel model) {

            //don't add Filter if AllowAnonymousFilter is already added
            if (model.Filters.Any(f => f.GetType() == typeof(AllowAnonymousFilter)))
                return;

            var pagePath = _appName.Replace(".Lib", "") + model.ViewEnginePath.Replace('/','.');
            model.Filters.Add(new AuthorizeFilter(pagePath));
            _config[$"DefaultPolicies:{pagePath}"] = "page";
        }


    }
}
