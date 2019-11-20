using Microsoft.AspNetCore.Mvc;

namespace EDennis.AspNetCore.Base.Web
{

    public static class ObjectResultExtensions {
        public static T Object<T>(this ObjectResult value) {
            return (T)(value.Value);
        }

    }

}
