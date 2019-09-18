using Newtonsoft.Json.Linq;

namespace EDennis.AspNetCore.Base.EntityFramework {
    internal class D {
        private readonly dynamic _data;
        public D(dynamic data) {
            _data = data;
        }
        public override string ToString() {
            var str = JToken.FromObject(_data).ToString();
            return str;
        }
    }

}
