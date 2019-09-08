using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace EDennis.AspNetCore.Base.EntityFramework
{

    public class StoredProcedureWithParameters
    {
        public string StoredProcedureName { get; set; }
        public DynamicParameter[] Parameters { get; set; }
    }

    public class DynamicParameter {
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DbType Type { get; set; }
        public dynamic Value { get; set; }
    }

    public static class DynamicParametersExtensions {
        public static void AddRange(this DynamicParameters destination, 
            DynamicParameter[] source) {
            foreach(var parm in source) {
                switch (parm.Type) {
                    case DbType.Int64:
                        destination.Add(parm.Name, long.Parse(parm.Value), parm.Type);
                        break;
                    case DbType.Int32:
                        destination.Add(parm.Name, int.Parse(parm.Value), parm.Type);
                        break;
                    case DbType.Int16:
                        destination.Add(parm.Name, short.Parse(parm.Value), parm.Type);
                        break;
                    case DbType.Byte:
                        destination.Add(parm.Name, byte.Parse(parm.Value), parm.Type);
                        break;
                    case DbType.Boolean:
                        destination.Add(parm.Name, bool.Parse(parm.Value), parm.Type);
                        break;
                    case DbType.Decimal:
                        destination.Add(parm.Name, decimal.Parse(parm.Value), parm.Type);
                        break;
                    case DbType.DateTime:
                        destination.Add(parm.Name, DateTime.Parse(parm.Value), parm.Type);
                        break;
                    case DbType.Time:
                        destination.Add(parm.Name, TimeSpan.Parse(parm.Value), parm.Type);
                        break;
                    case DbType.String:
                        destination.Add(parm.Name, parm.Value.ToString(), parm.Type);
                        break;
                    default:
                        destination.Add(parm.Name, parm.Value, parm.Type);
                        break;
                }
            }
        }
    }

}
