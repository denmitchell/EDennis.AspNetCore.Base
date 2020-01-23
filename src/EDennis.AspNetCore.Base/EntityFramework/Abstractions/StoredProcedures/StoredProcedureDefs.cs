using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;


namespace EDennis.AspNetCore.Base.EntityFramework {
    public class StoredProcedureDefs<TContext> : List<StoredProcedureDef>
        where TContext : DbContext {

        public StoredProcedureDefs(DbContextSettings<TContext> settings) {
            var builder = new DbContextOptionsBuilder<TContext>();
            DbContextProvider<TContext>.BuildBuilder(builder, settings);
            var options = builder.Options;

            using var context = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { options });
            if (context.Database.IsSqlServer())
                BuildStoredProcedureDefs(this, context);
        }


        internal static void BuildStoredProcedureDefs(StoredProcedureDefs<TContext> spDefs, TContext context) {

            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();


            DbCommand cmd = new Microsoft.Data.SqlClient.SqlCommand();
            cmd.Connection = cxn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = SQL_SERVER_STORED_PROCEDURE_DEFS;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans)
                cmd.Transaction = trans.GetDbTransaction();

            DbDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) {
                while (reader.Read()) {
                    var spDef = new StoredProcedureDef();
                    if (!reader.IsDBNull(0))
                        spDef.Schema = reader.GetString(0);
                    if (!reader.IsDBNull(1))
                        spDef.StoredProcedureName = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        spDef.ParameterName = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        spDef.Order = reader.GetInt32(3);
                    if (!reader.IsDBNull(4))
                        spDef.DbTypeName = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        spDef.IsOutput = reader.GetBoolean(5);
                    if (!reader.IsDBNull(6))
                        spDef.Length = reader.GetInt32(3);
                    if (!reader.IsDBNull(7))
                        spDef.Precision = reader.GetInt32(3);
                    if (!reader.IsDBNull(8))
                        spDef.Scale = reader.GetInt32(3);
                    spDefs.Add(spDef);
                }
            }

        }


        public void PopulateParameters(string spName, DbCommand sqlCommand, Dictionary<string, object> parameters, out Dictionary<string, DbParameter> outParameters) {
            outParameters = new Dictionary<string, DbParameter>();
            foreach (var parmDef in this.Where(sp => sp.StoredProcedureName.Equals(spName, StringComparison.OrdinalIgnoreCase))) {
                foreach (var parm in parameters.Where(p => $"@{p.Key}".Equals(parmDef.ParameterName, StringComparison.OrdinalIgnoreCase))) {
                    var dbParm = parmDef.ToDbParameter(parm.Value);
                    sqlCommand.Parameters.Add(dbParm);
                    if (parmDef.IsOutput)
                        outParameters.Add(parm.Key, dbParm);
                }
                foreach (var parm in parameters.Where(p => !$"@{p.Key}".Equals(parmDef.ParameterName, StringComparison.OrdinalIgnoreCase))) {
                    var dbParm = parmDef.ToDbParameter(null);
                    sqlCommand.Parameters.Add(dbParm);
                    if (parmDef.IsOutput)
                        outParameters.Add(dbParm.ParameterName, dbParm);
                }
            }

        }




        public static string SQL_SERVER_STORED_PROCEDURE_DEFS =
        @"
select  
   schema_name(p1.schema_id) [Schema],
   object_name(p1.object_id) [StoredProcedureName],
   p2.name [ParameterName],  
   parameter_id [Order],  
   type_name(user_type_id) [DbTypeName],  
   is_output [IsOutput],
   max_length [Length],  
   case when type_name(system_type_id) = 'uniqueidentifier' 
              then precision  
              else OdbcPrec(system_type_id, max_length, precision) end
			  [Precision],  
   OdbcScale(system_type_id, scale) [Scale]  
  from sys.procedures p1
  inner join sys.parameters p2 on p1.object_id = p2.object_id
";

    }
}
