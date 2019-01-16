using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This obviates the need for adding an [Authorize(SomePolicy)] attribute
    /// to the controller and actions
    /// </summary>
    public class AddDefaultAuthorizationPolicyConvention : IControllerModelConvention {

        private readonly string _appName;

        public AddDefaultAuthorizationPolicyConvention(IHostingEnvironment env) {
            _appName = env.ApplicationName;
        }

        public void Apply(ControllerModel controller) {
            var controllerPath = _appName + '.' + controller.ControllerName;
            controller.Filters.Add(new AuthorizeFilter(controllerPath));

            foreach(var action in controller.Actions) {
                var actionPath = controllerPath + '.' + action.ActionName;
                action.Filters.Add(new AuthorizeFilter(actionPath));
            }
        }

    }
}
