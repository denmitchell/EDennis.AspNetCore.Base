using Newtonsoft.Json.Linq;

namespace EDennis.AspNetCore.Base.Logging {

    /// <summary>
    /// This class is used for logging purposes.  When you wrap any
    /// class in AutoJson, it defers serialization to JSON
    /// until ToString() is called by the logger.  This is a 
    /// performance optimization.
    /// </summary>
    public class AutoJson {
        private readonly dynamic _data;
        public AutoJson(dynamic data) { _data = data; }
        public override string ToString() => JToken.FromObject(_data).ToString();
    }

}
