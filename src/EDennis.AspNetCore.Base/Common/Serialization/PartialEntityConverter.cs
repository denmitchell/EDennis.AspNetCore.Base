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

            /// <summary>
            /// Flat implementation only.  For deep read, use PartialEntity.Deserialize,
            /// which does two passes, one of which is very quick.
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            public override PartialEntity<TEntity> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

                var partialEntity = new PartialEntity<TEntity>();
                partialEntity.Entity = new TEntity();

                PropertyInfo propertyInfo = null;
                bool justReadPropName = false;
                while (reader.Read()) {
                    if (reader.TokenType == JsonTokenType.PropertyName) {
                        var propertyNameLowerCase = reader.GetString().ToLower();
                        if(PartialEntity<TEntity>.ClassPropertiesLowerCase.TryGetValue(propertyNameLowerCase, out PropertyInfo pInfo)) {
                            partialEntity.ObjectProperties.Add(pInfo.Name);
                            propertyInfo = pInfo;
                        }
                        justReadPropName = true;
                    } else if (justReadPropName) {
                        if (propertyInfo.PropertyType == typeof(bool))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetBoolean());
                        else if (propertyInfo.PropertyType == typeof(byte))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetByte());
                        else if (propertyInfo.PropertyType == typeof(sbyte))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetSByte());
                        else if (propertyInfo.PropertyType == typeof(short))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetInt16());
                        else if (propertyInfo.PropertyType == typeof(ushort))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetUInt16());
                        else if (propertyInfo.PropertyType == typeof(int))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetInt32());
                        else if (propertyInfo.PropertyType == typeof(uint))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetUInt32());
                        else if (propertyInfo.PropertyType == typeof(long))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetInt64());
                        else if (propertyInfo.PropertyType == typeof(ulong))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetUInt64());
                        else if (propertyInfo.PropertyType == typeof(float))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetSingle());
                        else if (propertyInfo.PropertyType == typeof(double))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetDouble());
                        else if (propertyInfo.PropertyType == typeof(decimal))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetDecimal());
                        else if (propertyInfo.PropertyType == typeof(DateTime))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetDateTime());
                        else if (propertyInfo.PropertyType == typeof(DateTimeOffset))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetDateTimeOffset());
                        else if (propertyInfo.PropertyType == typeof(TimeSpan)) {
                            var timeString = reader.GetString();
                            propertyInfo.SetValue(partialEntity.Entity, TimeSpan.Parse(timeString));
                        } else if (propertyInfo.PropertyType == typeof(Guid))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetGuid());
                        else if (propertyInfo.PropertyType == typeof(string))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetString());
                        else if (propertyInfo.PropertyType == typeof(byte[]))
                            propertyInfo.SetValue(partialEntity.Entity, reader.GetBytesFromBase64());

                        justReadPropName = false;

                    }

                }

                return partialEntity;
                //TEntity entity = JsonSerializer.Deserialize<TEntity>(ref reader, injectedOptions ?? options);
                //return new PartialEntity<TEntity> { Entity = entity };
                
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
