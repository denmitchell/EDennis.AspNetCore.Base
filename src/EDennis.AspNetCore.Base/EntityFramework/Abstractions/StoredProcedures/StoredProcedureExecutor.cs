using EDennis.AspNetCore.Base.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    public class StoredProcedureExecutor {


        public static List<TEntity> ExecuteToList<TContext, TEntity>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var json = ExecuteToJsonArray(context, spName, spDefs, parameters, typeof(TEntity).GetProperties().Select(p => p.Name).ToArray());
            var result = JsonSerializer.Deserialize<List<TEntity>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }


        public static async Task<List<TEntity>> ExecuteToListAsync<TContext, TEntity>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var json = await ExecuteToJsonArrayAsync(context, spName, spDefs, parameters, typeof(TEntity).GetProperties().Select(p => p.Name).ToArray());
            var result = JsonSerializer.Deserialize<List<TEntity>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }


        public static TEntity ExecuteToObject<TContext, TEntity>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var json = ExecuteToJsonObject(context, spName, spDefs, parameters, typeof(TEntity).GetProperties().Select(p => p.Name).ToArray());
            var result = JsonSerializer.Deserialize<TEntity>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }


        public static async Task<TEntity> ExecuteToObjectAsync<TContext, TEntity>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var json = await ExecuteToJsonObjectAsync(context, spName, spDefs, parameters, typeof(TEntity).GetProperties().Select(p => p.Name).ToArray());
            var result = JsonSerializer.Deserialize<TEntity>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }


        public static dynamic ExecuteToDynamicObject<TContext, TEntity>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var json = ExecuteToJsonObject(context, spName, spDefs, parameters, typeof(TEntity).GetProperties().Select(p => p.Name).ToArray());
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new DynamicJsonConverter<TEntity>());
            var result = JsonSerializer.Deserialize<dynamic>(json, options);
            return result;
        }


        public static async Task<dynamic> ExecuteToDynamicObjectAsync<TContext, TEntity>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var json = await ExecuteToJsonObjectAsync(context, spName, spDefs, parameters, typeof(TEntity).GetProperties().Select(p => p.Name).ToArray());
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new DynamicJsonConverter<TEntity>());
            var result = JsonSerializer.Deserialize<dynamic>(json, options);
            return result;
        }



        public static List<dynamic> ExecuteToDynamicList<TContext, TEntity>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var json = ExecuteToJsonArray(context, spName, spDefs, parameters, typeof(TEntity).GetProperties().Select(p => p.Name).ToArray());
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new DynamicJsonConverter<TEntity>());
            var result = JsonSerializer.Deserialize<List<dynamic>>(json, options);
            return result;
        }


        public static async Task<List<dynamic>> ExecuteToDynamicListAsync<TContext, TEntity>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var json = await ExecuteToJsonArrayAsync(context, spName, spDefs, parameters, typeof(TEntity).GetProperties().Select(p => p.Name).ToArray());
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new DynamicJsonConverter<TEntity>());
            var result = JsonSerializer.Deserialize<List<dynamic>>(json, options);
            return result;
        }




        public static dynamic ExecuteToScalar<TContext, TResult>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();


            DbCommand cmd = new Microsoft.Data.SqlClient.SqlCommand();
            cmd.Connection = cxn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans)
                cmd.Transaction = trans.GetDbTransaction();

            spDefs.PopulateParameters(spName, cmd, parameters, out Dictionary<string, DbParameter> _);

            var obj = cmd.ExecuteScalar();
            var result = Convert.ChangeType(obj, typeof(TResult));

            return result;

        }


        public static async Task<dynamic> ExecuteToScalarAsync<TContext, TResult>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();


            DbCommand cmd = new Microsoft.Data.SqlClient.SqlCommand();
            cmd.Connection = cxn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans)
                cmd.Transaction = trans.GetDbTransaction();

            spDefs.PopulateParameters(spName, cmd, parameters, out Dictionary<string, DbParameter> _);

            var obj = await cmd.ExecuteScalarAsync();
            var result = Convert.ChangeType(obj, typeof(TResult));

            return result;

        }




        public static string ExecuteToJsonObject<TContext>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters,
            params string[] jsonPropertiesToInclude)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();


            DbCommand cmd = new Microsoft.Data.SqlClient.SqlCommand();
            cmd.Connection = cxn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans)
                cmd.Transaction = trans.GetDbTransaction();

            spDefs.PopulateParameters(spName, cmd, parameters, out Dictionary<string, DbParameter> outParameters);


            DbDataReader reader = cmd.ExecuteReader();

            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            if (reader.HasRows) {
                Dictionary<string, FieldInfo> fieldInfo = new Dictionary<string, FieldInfo>();
                foreach (var prop in jsonPropertiesToInclude) {
                    try {
                        var info =
                            new FieldInfo {
                                JsonPropertyName = prop,
                                Ordinal = reader.GetOrdinal(prop),
                            };
                        info.ClrType = reader.GetFieldType(info.Ordinal);
                        info.DbTypeName = reader.GetDataTypeName(info.Ordinal);
                        fieldInfo.Add(prop, info);
                    } catch (System.IndexOutOfRangeException) {
                        throw new ArgumentException($"The stored procedure {spName} does not return a column called {prop}.");
                    }
                }

                while (reader.Read()) {
                    writer.WriteStartObject();
                    foreach (var prop in jsonPropertiesToInclude) {
                        var info = fieldInfo[prop];
                        writer.WritePropertyName(prop);
                        if (reader.IsDBNull(info.Ordinal))
                            writer.WriteNullValue();
                        else
                            WriteValue(writer, info.ClrType, info.DbTypeName, reader.GetValue(info.Ordinal));
                    }
                    writer.WriteEndObject();
                    break; //single result
                }

                parameters.Clear();
                foreach (var key in outParameters.Keys)
                    parameters.Add(key, cmd.Parameters[outParameters[key].ParameterName].Value);

                writer.Flush();
                string json = Encoding.UTF8.GetString(stream.ToArray());
                return json;


            } else
                return "{}";

        }




        public static async Task<string> ExecuteToJsonObjectAsync<TContext>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters,
            params string[] jsonPropertiesToInclude)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();


            DbCommand cmd = new Microsoft.Data.SqlClient.SqlCommand();
            cmd.Connection = cxn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans)
                cmd.Transaction = trans.GetDbTransaction();

            spDefs.PopulateParameters(spName, cmd, parameters, out Dictionary<string, DbParameter> outParameters);


            DbDataReader reader = await cmd.ExecuteReaderAsync();

            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            if (reader.HasRows) {
                Dictionary<string, FieldInfo> fieldInfo = new Dictionary<string, FieldInfo>();
                foreach (var prop in jsonPropertiesToInclude) {
                    try {
                        var info =
                            new FieldInfo {
                                JsonPropertyName = prop,
                                Ordinal = reader.GetOrdinal(prop),
                            };
                        info.ClrType = reader.GetFieldType(info.Ordinal);
                        info.DbTypeName = reader.GetDataTypeName(info.Ordinal);
                        fieldInfo.Add(prop, info);
                    } catch (System.IndexOutOfRangeException) {
                        throw new ArgumentException($"The stored procedure {spName} does not return a column called {prop}.");
                    }
                }

                while (reader.Read()) {
                    writer.WriteStartObject();
                    foreach (var prop in jsonPropertiesToInclude) {
                        var info = fieldInfo[prop];
                        writer.WritePropertyName(prop);
                        if (reader.IsDBNull(info.Ordinal))
                            writer.WriteNullValue();
                        else
                            WriteValue(writer, info.ClrType, info.DbTypeName, reader.GetValue(info.Ordinal));
                    }
                    writer.WriteEndObject();
                    break; //single result
                }

                parameters.Clear();
                foreach (var key in outParameters.Keys)
                    parameters.Add(key, cmd.Parameters[outParameters[key].ParameterName].Value);

                writer.Flush();
                string json = Encoding.UTF8.GetString(stream.ToArray());
                return json;


            } else
                return "{}";

        }




        public static string ExecuteToJsonArray<TContext>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters,
            params string[] jsonPropertiesToInclude)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();


            DbCommand cmd = new Microsoft.Data.SqlClient.SqlCommand();
            cmd.Connection = cxn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans)
                cmd.Transaction = trans.GetDbTransaction();

            spDefs.PopulateParameters(spName, cmd, parameters, out Dictionary<string, DbParameter> outParameters);


            DbDataReader reader = cmd.ExecuteReader();

            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            if (reader.HasRows) {
                Dictionary<string, FieldInfo> fieldInfo = new Dictionary<string, FieldInfo>();
                foreach (var prop in jsonPropertiesToInclude) {
                    try {
                        var info =
                            new FieldInfo {
                                JsonPropertyName = prop,
                                Ordinal = reader.GetOrdinal(prop),
                            };
                        info.ClrType = reader.GetFieldType(info.Ordinal);
                        info.DbTypeName = reader.GetDataTypeName(info.Ordinal);
                        fieldInfo.Add(prop, info);
                    } catch (System.IndexOutOfRangeException) {
                        throw new ArgumentException($"The stored procedure {spName} does not return a column called {prop}.");
                    }
                }

                writer.WriteStartArray();
                while (reader.Read()) {
                    writer.WriteStartObject();
                    foreach (var prop in jsonPropertiesToInclude) {
                        var info = fieldInfo[prop];
                        writer.WritePropertyName(prop);
                        if (reader.IsDBNull(info.Ordinal))
                            writer.WriteNullValue();
                        else
                            WriteValue(writer, info.ClrType, info.DbTypeName, reader.GetValue(info.Ordinal));
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();

                parameters.Clear();
                foreach (var key in outParameters.Keys)
                    parameters.Add(key, cmd.Parameters[outParameters[key].ParameterName].Value);

                writer.Flush();
                string json = Encoding.UTF8.GetString(stream.ToArray());
                return json;


            } else
                return "[]";

        }





        public static async Task<string> ExecuteToJsonArrayAsync<TContext>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters,
            params string[] jsonPropertiesToInclude)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();


            DbCommand cmd = new Microsoft.Data.SqlClient.SqlCommand();
            cmd.Connection = cxn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans)
                cmd.Transaction = trans.GetDbTransaction();

            spDefs.PopulateParameters(spName, cmd, parameters, out Dictionary<string, DbParameter> outParameters);


            DbDataReader reader = await cmd.ExecuteReaderAsync();

            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            if (reader.HasRows) {
                Dictionary<string, FieldInfo> fieldInfo = new Dictionary<string, FieldInfo>();
                foreach (var prop in jsonPropertiesToInclude) {
                    try {
                        var info =
                            new FieldInfo {
                                JsonPropertyName = prop,
                                Ordinal = reader.GetOrdinal(prop),
                            };
                        info.ClrType = reader.GetFieldType(info.Ordinal);
                        info.DbTypeName = reader.GetDataTypeName(info.Ordinal);
                        fieldInfo.Add(prop, info);
                    } catch (System.IndexOutOfRangeException) {
                        throw new ArgumentException($"The stored procedure {spName} does not return a column called {prop}.");
                    }
                }

                writer.WriteStartArray();
                while (reader.Read()) {
                    writer.WriteStartObject();
                    foreach (var prop in jsonPropertiesToInclude) {
                        var info = fieldInfo[prop];
                        writer.WritePropertyName(prop);
                        if (reader.IsDBNull(info.Ordinal))
                            writer.WriteNullValue();
                        else
                            WriteValue(writer, info.ClrType, info.DbTypeName, reader.GetValue(info.Ordinal));
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();

                parameters.Clear();
                foreach (var key in outParameters.Keys)
                    parameters.Add(key, cmd.Parameters[outParameters[key].ParameterName].Value);

                writer.Flush();
                string json = Encoding.UTF8.GetString(stream.ToArray());
                return json;


            } else
                return "[]";

        }




        public static string ExecuteFromJson<TContext>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();


            DbCommand cmd = new Microsoft.Data.SqlClient.SqlCommand();
            cmd.Connection = cxn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans)
                cmd.Transaction = trans.GetDbTransaction();

            spDefs.PopulateParameters(spName, cmd, parameters, out Dictionary<string, DbParameter> outParameters);


            DbDataReader reader = cmd.ExecuteReader();

            string json = "";

            if (reader.HasRows) {
                while (reader.Read()) {
                    if (!reader.IsDBNull(0))
                        json = reader.GetString(0);
                    break;
                }

                parameters.Clear();
                foreach (var key in outParameters.Keys)
                    parameters.Add(key, cmd.Parameters[outParameters[key].ParameterName].Value);

                return json;


            } else
                return null;

        }



        public static async Task<string> ExecuteFromJsonAsync<TContext>(ISqlServerDbContext<TContext> context, string spName,
            StoredProcedureDefs<TContext> spDefs, Dictionary<string, object> parameters)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();


            DbCommand cmd = new Microsoft.Data.SqlClient.SqlCommand();
            cmd.Connection = cxn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans)
                cmd.Transaction = trans.GetDbTransaction();

            spDefs.PopulateParameters(spName, cmd, parameters, out Dictionary<string, DbParameter> outParameters);


            DbDataReader reader = await cmd.ExecuteReaderAsync();

            string json = "";

            if (reader.HasRows) {
                while (reader.Read()) {
                    if (!reader.IsDBNull(0))
                        json = reader.GetString(0);
                    break;
                }

                parameters.Clear();
                foreach (var key in outParameters.Keys)
                    parameters.Add(key, cmd.Parameters[outParameters[key].ParameterName].Value);

                return json;


            } else
                return null;

        }


        private static void WriteValue(Utf8JsonWriter writer, Type type, string dataTypeName, dynamic value) {
            if (value == null)
                writer.WriteNullValue();
            else if (type == typeof(bool))
                writer.WriteBooleanValue((bool)value);
            else if (type == typeof(byte[]) && dataTypeName != null && dataTypeName.Equals("varbinary", StringComparison.OrdinalIgnoreCase))
                writer.WriteBase64StringValue((byte[])value);
            else if (type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(byte) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong))
                writer.WriteNumberValue((long)value);
            else if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
                writer.WriteNumberValue((decimal)value);
            else if (type == typeof(TimeSpan))
                writer.WriteStringValue(((TimeSpan)value).ToString("HH:mm:ss"));
            else if (type == typeof(DateTime)) {
                if (dataTypeName != null && (dataTypeName.Equals("date", StringComparison.OrdinalIgnoreCase)
                    || dataTypeName.Equals("smalldate", StringComparison.OrdinalIgnoreCase)))
                    writer.WriteStringValue(((DateTime)value).ToString("yyyy-mm-dd"));
                else if (dataTypeName != null && dataTypeName.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                    writer.WriteStringValue(((DateTime)value).ToString("yyyy-mm-dd HH:mm:ss"));
                else if (dataTypeName != null && (dataTypeName.Equals("datetime2", StringComparison.OrdinalIgnoreCase)
                    || dataTypeName.Equals("datetimeoffset", StringComparison.OrdinalIgnoreCase)))
                    writer.WriteStringValue(((DateTimeOffset)value).ToString("yyyy-mm-dd HH:mm:ss.0000000"));
            } else
                writer.WriteStringValue((string)value);
        }


    }


    internal class FieldInfo {
        public string JsonPropertyName { get; set; }
        public int Ordinal { get; set; }
        public Type ClrType { get; set; }
        public string DbTypeName { get; set; }
    }


}
