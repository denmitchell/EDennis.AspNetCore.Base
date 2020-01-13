using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EDennis.AspNetCore.Base.Serialization {

    /// <summary>
    /// Serializes and Deserializes PartialEntity objects
    /// </summary>
    public class PartialEntityConverter : JsonConverterFactory {
        public override bool CanConvert(Type typeToConvert) {
            if (!typeToConvert.IsGenericType) {
                return false;
            }

            if (typeToConvert.GetGenericTypeDefinition() != typeof(PartialEntity<>)) {
                return false;
            }

            return typeToConvert.GetGenericArguments()[0].IsClass;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
            Type valueType = typeToConvert.GetGenericArguments()[0];

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(PartialEntityConverterInner<>).MakeGenericType(
                    new Type[] { valueType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null);

            return converter;
        }

        private class PartialEntityConverterInner<TEntity> :
            JsonConverter<PartialEntity<TEntity>> where TEntity : class, new() {

            readonly JsonSerializerOptions injectedOptions;

            public PartialEntityConverterInner(JsonSerializerOptions options) {
                injectedOptions = options;
            }

            public override PartialEntity<TEntity> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                TEntity entity = JsonSerializer.Deserialize<TEntity>(ref reader, injectedOptions ?? options);
                return new PartialEntity<TEntity> { Entity = entity };
            }

            public override void Write(Utf8JsonWriter writer, PartialEntity<TEntity> value, JsonSerializerOptions options) {
                writer.WriteStartObject();
                foreach (var propName in value.ObjectProperties) {
                    writer.WritePropertyName(propName);
                    var prop = PartialEntity<TEntity>.ClassProperties[propName];
                    var entityValue = prop.GetValue(value.Entity);
                    JsonSerializer.Serialize(writer, entityValue, prop.PropertyType, injectedOptions ?? options);
                }
                writer.WriteEndObject();
            }
        }
    }
}
