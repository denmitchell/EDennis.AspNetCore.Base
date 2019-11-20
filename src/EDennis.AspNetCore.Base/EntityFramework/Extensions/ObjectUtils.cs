using System.IO;
using System.Text;
using System.Text.Json;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public static class ObjectUtils {

        /// <summary>
        /// Copies an object
        /// </summary>
        /// <typeparam name="T">The class of the object to copy and also the return type</typeparam>
        /// <param name="obj">The object to copy</param>
        /// <returns>A deep copy of the provided object</returns>
        public static T Copy<T>(T obj)
            where T : class, new() {
            var json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Creates a new typed object from a dynamic object,
        /// copying properties from the dynamic object into the new object.
        /// This implementation uses the Merge method, which performs
        /// top-level property replacement only.
        /// </summary>
        /// <typeparam name="T">The type of the result object</typeparam>
        /// <param name="partialObj">A "partial object" containing properties to copy</param>
        /// <returns>a new object of type T, having property values from the partial object</returns>
        public static T CopyFromDynamic<T>(dynamic partialObj)
            where T : class, new() {
            var oldObj = new T();
            return Merge<T>(oldObj, partialObj);
        }

        /// <summary>
        /// Create a new subclass object with common properties
        /// populated from a base class object.
        /// </summary>
        /// <typeparam name="TBase">The type of the base class</typeparam>
        /// <typeparam name="TSub">The type of the subclass</typeparam>
        /// <param name="obj">The base class object</param>
        /// <returns>The subclass object returned with property values from the base class object</returns>
        public static TSub CopyFromBaseClass<TBase, TSub>(TBase obj)
            where TBase : class, new()
            where TSub : TBase {
            var json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<TSub>(json);
        }

        /// <summary>
        /// Uses System.Text.Json methods to merge properties from a dynamic object
        /// (representing a "partial object") into an existing object.  Note that this
        /// implements a top-level merge only -- only top-level properties are overwritten.
        /// If a property from the dynamic object is complex (e.g., nested objects and/or
        /// collections), the entire graph of the property overwrites whatever was present
        /// in the existing object.  This differs from the behavior of Json.NET, which 
        /// performs more complex merging of nested objects (e.g., adding items in a
        /// nested array).
        /// </summary>
        /// <typeparam name="T">The type of the existing and resulting objects</typeparam>
        /// <param name="obj">The existing object</param>
        /// <param name="partialObj">A dynamic object holding properties to copy</param>
        /// <returns>A new object that contains property values from obj, except where
        /// overwritten by property values from partialObj</returns>
        public static T Merge<T>(T obj, dynamic partialObj)
            where T : class {

            using JsonDocument newDoc = JsonDocument.Parse(JsonSerializer.Serialize(partialObj));
            using JsonDocument oldDoc = JsonDocument.Parse(JsonSerializer.Serialize(obj));

            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            foreach (var prop in typeof(T).GetProperties()) {
                writer.WritePropertyName(prop.Name);

                if (newDoc.RootElement.TryGetProperty(prop.Name, out var newProp)) {
                    //special handling for string values that look like numbers or booleans
                    if ((newProp.ValueKind == JsonValueKind.Number ||
                        newProp.ValueKind == JsonValueKind.False ||
                        newProp.ValueKind == JsonValueKind.True
                        ) && prop.PropertyType == typeof(string))
                        writer.WriteStringValue(newProp.GetRawText());
                    else
                        newProp.WriteTo(writer);
                } else if (newDoc.RootElement.TryGetProperty(prop.Name, out var oldProp)) {
                    //special handling for string values that look like numbers or booleans
                    if ((oldProp.ValueKind == JsonValueKind.Number ||
                        oldProp.ValueKind == JsonValueKind.False ||
                        oldProp.ValueKind == JsonValueKind.True
                        ) && prop.PropertyType == typeof(string))
                        writer.WriteStringValue(oldProp.GetRawText());
                    else
                        oldProp.WriteTo(writer);
                }
            }
            writer.WriteEndObject();
            writer.Flush();
            string json = Encoding.UTF8.GetString(stream.ToArray());
            return JsonSerializer.Deserialize<T>(json);
        }

    }
}
