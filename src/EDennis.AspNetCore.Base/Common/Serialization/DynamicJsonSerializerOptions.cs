using EDennis.AspNetCore.Base.Serialization;
using System.Text.Json;

namespace EDennis.AspNetCore.Base {
    public class DynamicJsonSerializerOptions {
        public static JsonSerializerOptions Create<TEntity>(bool formatted = true, bool caseInsensitive = true) {
            JsonSerializerOptions options = new JsonSerializerOptions();
            if (formatted)
                options.WriteIndented = true;
            if (caseInsensitive)
                options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new DynamicJsonConverter<TEntity>());
            return options;
        }
    }
}
