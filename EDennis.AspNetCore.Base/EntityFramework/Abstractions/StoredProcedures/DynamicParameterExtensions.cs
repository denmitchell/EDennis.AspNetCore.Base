using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace EDennis.AspNetCore.Base.EntityFramework
{

    public static class DynamicParametersExtensions
    {


        public static Dictionary<string, Type> TypeMap = new Dictionary<string, Type>() {

        { "bit", typeof(bool) },
        { "tinyint",  typeof(byte) },
        { "smallint",  typeof(int) },
        { "int",  typeof(int) },
        { "bigint",  typeof(long) },

        { "decimal",  typeof(decimal) },
        { "numeric",  typeof(decimal) },
        { "money",  typeof(decimal) },
        { "smallmoney",  typeof(decimal) },
        { "float",  typeof(double) },
        { "real",  typeof(float) },

        { "varchar", typeof(string) },
        { "nvarchar", typeof(string) },
        { "char", typeof(string) },
        { "nchar", typeof(string) },
        { "text", typeof(string) },
        { "ntext", typeof(string) },

        { "smalldatetime", typeof(DateTime) },
        { "date", typeof(DateTime) },
        { "datetime", typeof(DateTime) },
        { "datetime2", typeof(DateTime) },
        { "datetimeoffset", typeof(DateTimeOffset) },
        { "time", typeof(TimeSpan) },

        {"uniqueidentifier", typeof(Guid) },
    };


        public static void AddRange(this DynamicParameters destination,
            string spName,
            IEnumerable<KeyValuePair<string, string>> parms,
            List<StoredProcedureDef> spDefs) {

            var spDef = spDefs
                .Where(d => d.StoredProcedureName == spName)
                .ToList();
            foreach (var parm in parms) {
                var parmDef = spDef.FirstOrDefault(d => d.ParameterName == parm.Key);
                dynamic value = Convert.ChangeType(parm.Value,TypeMap[parmDef.Type]);
                destination.Add(parmDef.ParameterName, value);

            }
        }
    }

}
