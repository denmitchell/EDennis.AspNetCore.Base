using System;

namespace EDennis.AspNetCore.Base.Web {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ReadonlyControllerAttribute : Attribute
    {
        public ReadonlyControllerAttribute(string route) {
            Route = route;
        }

        public string Route { get; set; }
    }
}
