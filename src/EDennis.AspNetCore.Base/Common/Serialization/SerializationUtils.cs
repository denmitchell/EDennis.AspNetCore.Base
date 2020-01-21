using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace EDennis.AspNetCore.Base.Serialization {
    public class SerializationUtils {

        public static string ReadToString(Utf8JsonReader reader) {

            using var ms = new MemoryStream();
            using var sr = new StreamReader(ms);
            using var jw = new Utf8JsonWriter(ms);
            while (reader.Read()) {
                if (reader.TokenType == JsonTokenType.StartObject)
                    jw.WriteStartObject();
                else if (reader.TokenType == JsonTokenType.StartArray)
                    jw.WriteStartArray();
                else if (reader.TokenType == JsonTokenType.PropertyName)
                    jw.WritePropertyName(reader.GetString());
                else if (reader.TokenType == JsonTokenType.EndObject)
                    jw.WriteEndObject();
                else if (reader.TokenType == JsonTokenType.EndArray)
                    jw.WriteEndArray();
                else if (reader.TokenType == JsonTokenType.False)
                    jw.WriteBooleanValue(false);
                else if (reader.TokenType == JsonTokenType.True)
                    jw.WriteBooleanValue(true);
                else if (reader.TokenType == JsonTokenType.Null)
                    jw.WriteNullValue();
                else if (reader.TokenType == JsonTokenType.Number)
                    jw.WriteNumberValue(reader.GetDecimal());
                else if (reader.TokenType == JsonTokenType.String)
                    jw.WriteStringValue(reader.GetString());
            }
            jw.Flush();
            var str = sr.ReadToEnd();
            return str;
        }

    }
}
