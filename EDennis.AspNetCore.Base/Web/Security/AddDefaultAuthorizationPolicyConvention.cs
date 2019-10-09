using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace EDennis.AspNetCore.Base.Security {

    /// <summary>
    /// This obviates the need for adding an [Authorize(SomePolicy)] attribute
    /// to the controller and actions
    /// </summary>
    public class AddDefaultAuthorizationPolicyConvention : IControllerModelConvention {

        private readonly string _appName;
        private readonly IConfiguration _config;

        public AddDefaultAuthorizationPolicyConvention(IHostingEnvironment env, IConfiguration config) {
            _appName = env.ApplicationName;
            _config = config;
        }

        public void Apply(ControllerModel controller) {

            //don't add Filter if AllowAnonymousFilter is already added
            if (controller.Filters.Any(f => f.GetType() == typeof(AllowAnonymousFilter)))
                return;

            var controllerPath = _appName + '.' + controller.ControllerName;

            foreach (var action in controller.Actions) {
                var actionPath = controllerPath + '.' + action.ActionName;
                action.Filters.Add(new AuthorizeFilter(actionPath));
                _config[$"DefaultPolicies:{actionPath}"] = "action";
            }
        }

    }
}
