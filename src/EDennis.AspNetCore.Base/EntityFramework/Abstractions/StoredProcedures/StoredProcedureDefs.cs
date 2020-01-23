using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace EDennis.AspNetCore.Base.EntityFramework {
    public class StoredProcedureDefs<TContext> : List<StoredProcedureDef> 
        where TContext : DbContext {

        
        public StoredProcedureDefs(DbContextSettings<TContext> settings) {
            var builder = new DbContextOptionsBuilder<TContext>();
            DbContextProvider<TContext>.BuildBuilder(builder, settings);
            var options = builder.Options;

            using var context = (TContext)Activator.CreateInstance(typeof(TContext), new object[] { options });
            if(context.Database.IsSqlServer())
                BuildStoredProcedureDefs(context);
        }


        private void BuildStoredProcedureDefs(TContext context) {

            var cxn = context.Database.GetDbConnection();

            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                AddRange(cxn.Query<StoredProcedureDef>(sql: SQL_SERVER_STORED_PROCEDURE_DEFS,
                    transaction: dbTrans,
                    commandType: CommandType.Text)
                    .ToList());

            } else {
                AddRange(cxn.Query<StoredProcedureDef>(sql: SQL_SERVER_STORED_PROCEDURE_DEFS,
                    commandType: CommandType.Text)
                    .ToList());
            }

        }



        public static string SQL_SERVER_STORED_PROCEDURE_DEFS =
        @"
select  
   schema_name(p1.schema_id) [Schema],
   object_name(p1.object_id) [StoredProcedureName],
   p2.name [ParameterName],  
   parameter_id [Order],  
   type_name(user_type_id) [Type],  
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
