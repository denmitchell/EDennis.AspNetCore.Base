using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace EDennis.AspNetCore.Base.EntityFramework
{

    public static class DynamicParametersExtensions
    {

        private const string SPNAME_PATTERN = @"(\[?([A-Za-z0-9_]+)\]?\.)?\[?([A-Za-z0-9_]+)\]?";

        private readonly static Dictionary<string, Type> TypeMap = new Dictionary<string, Type>() {

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

            GetStoredProcedureNameComponents(spName, out string schema, out string name); 
            var spDef = spDefs
                .Where(d => 
                    d.StoredProcedureName.ToLower() == name
                    && d.Schema.ToLower() == schema)
                .ToList();
            foreach (var parm in parms) {
                var parmDef = spDef
                    .FirstOrDefault(d => d.ParameterName.ToLower().Replace("@", "") 
                    == parm.Key.ToLower().Replace("@", ""));
                ParameterDirection direction;
                if (parmDef.IsOutput) {
                    if (parm.Value == "")
                        direction = ParameterDirection.Output;
                    else
                        direction = ParameterDirection.InputOutput;
                } else {
                    direction = ParameterDirection.Input;
                }
                if (direction == ParameterDirection.Output)
                    destination.Add(parmDef.ParameterName, null, direction: direction);
                else {
                    dynamic value = Convert.ChangeType(parm.Value, TypeMap[parmDef.Type]);
                    destination.Add(parmDef.ParameterName, value, direction: direction);
                }
            }
        }

        private static void GetStoredProcedureNameComponents(
            string spName, out string schema, out string name) {
            if (spName.Count(c => c == '.') > 1)
                throw new ApplicationException($"Stored Procedure name {spName} is not supported.  The name cannot contain more than one period (as in schema.table)");
            else if (!spName.Contains(".")) {
                schema = "dbo";
                name = spName.Replace("[", "").Replace("]", "").ToLower();
            } else {
                var components = Regex.Matches(spName, SPNAME_PATTERN)
                    .ToList().SelectMany<Match,Group>(m => m.Groups)
                    .Skip(1)
                    .Select(g => g.Value)
                    .ToList();
                schema = components[0].ToLower();
                name = components[1].ToLower();
            }
        }

    }

}
