using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;

namespace EDennis.AspNetCore.Base.EntityFramework
{
    public class DynamicParameter {
        public string Name { get; set; }
        public dynamic Value { get; set; }
    }

}
