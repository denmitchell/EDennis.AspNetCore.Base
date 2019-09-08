using System;

namespace EDennis.AspNetCore.Base.Web {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WriteableControllerAttribute : Attribute
    {
        public WriteableControllerAttribute(string route) {
            Route = route;
        }

        public string Route { get; set; }
    }
}
