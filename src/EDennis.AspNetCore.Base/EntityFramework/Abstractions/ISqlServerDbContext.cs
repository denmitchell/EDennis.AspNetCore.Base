using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface ISqlServerDbContext<TContext> : IRelationalDbContext<TContext>
        where TContext : DbContext {
        StoredProcedureDefs<TContext> StoredProcedureDefs { get; set; }
    }

    public static class ISqlServerDbContextExtensions {


        /// <summary>
        /// Retrieves raw JSON from SQL Server using the
        /// provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public static string GetFromJsonSql<TContext>(this ISqlServerDbContext<TContext> context, string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();

            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.ExecuteScalar<string>(sql, transaction: dbTrans);
            } else {
                result = cxn.ExecuteScalar<string>(sql);
            }
            return result;

        }


        /// <summary>
        /// Asynchronously retrieves raw JSON from SQL Server 
        /// using the provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public static async Task<string> GetFromJsonSqlAsync<TContext>(this ISqlServerDbContext<TContext> context,  string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<string>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<string>(sql);
            }
            return result;

        }



        /// <summary>
        /// Retrieves raw JSON from SQL Server using the
        /// provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public static List<TEntity> GetFromJsonSql<TContext,TEntity>(this ISqlServerDbContext<TContext> context, string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();

            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.ExecuteScalar<string>(sql, transaction: dbTrans);
            } else {
                result = cxn.ExecuteScalar<string>(sql);
            }
            return JsonSerializer.Deserialize<List<TEntity>>(result);

        }


        /// <summary>
        /// Asynchronously retrieves raw JSON from SQL Server 
        /// using the provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public static async Task<List<TEntity>> GetFromJsonSqlAsync<TContext,TEntity>(this ISqlServerDbContext<TContext> context, string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<string>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<string>(sql);
            }
            return JsonSerializer.Deserialize<List<TEntity>>(result);

        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static string GetJsonColumnFromStoredProcedure<TContext>(this ISqlServerDbContext<TContext> context,  string spName,
            IEnumerable<KeyValuePair<string, string>> parms)
            where TContext : DbContext {


            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, context.StoredProcedureDefs);


            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                dynamic result = cxn.QuerySingle<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = result.Json ?? result.json ?? result.JSON;

            } else {
                dynamic result = cxn.QuerySingle<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = result.Json ?? result.json ?? result.JSON;
            }

            return json;

        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static async Task<string> GetJsonColumnFromStoredProcedureAsync<TContext>(this ISqlServerDbContext<TContext> context,  string spName,
            IEnumerable<KeyValuePair<string, string>> parms)
            where TContext : DbContext {

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, context.StoredProcedureDefs);


            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                dynamic result = await cxn.QuerySingleAsync<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = result.Json ?? result.json ?? result.JSON;

            } else {
                dynamic result = await cxn.QuerySingleAsync<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = result.Json ?? result.json ?? result.JSON;
            }

            return json;


        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static TEntity GetJsonColumnFromStoredProcedure<TContext,TEntity>(this ISqlServerDbContext<TContext> context, string spName,
            IEnumerable<KeyValuePair<string, string>> parms)
            where TContext : DbContext {


            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, context.StoredProcedureDefs);


            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                dynamic result = cxn.QuerySingle<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = result.Json ?? result.json ?? result.JSON;

            } else {
                dynamic result = cxn.QuerySingle<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = result.Json ?? result.json ?? result.JSON;
            }

            return JsonSerializer.Deserialize<TEntity>(json);

        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static async Task<TEntity> GetJsonColumnFromStoredProcedureAsync<TContext,TEntity>(this ISqlServerDbContext<TContext> context, string spName,
            IEnumerable<KeyValuePair<string, string>> parms)
            where TContext : DbContext {

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, context.StoredProcedureDefs);


            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                dynamic result = await cxn.QuerySingleAsync<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = result.Json ?? result.json ?? result.JSON;

            } else {
                dynamic result = await cxn.QuerySingleAsync<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = result.Json ?? result.json ?? result.JSON;
            }

            return JsonSerializer.Deserialize<TEntity>(json);


        }




        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public static List<TEntity> GetFromStoredProcedure<TContext, TEntity>(this ISqlServerDbContext<TContext> context,  string spName,
            IEnumerable<KeyValuePair<string, string>> parms)
            where TContext : DbContext {

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, context.StoredProcedureDefs);


            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            IEnumerable<TEntity> result;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                result = cxn.Query<TEntity>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);


            } else {
                result = cxn.Query<TEntity>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }

            return result.ToList();

        }



        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public static async Task<List<TEntity>> GetFromStoredProcedureAsync<TContext,TEntity>(this ISqlServerDbContext<TContext> context,  string spName,
            IEnumerable<KeyValuePair<string, string>> parms)
            where TContext : DbContext {

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, context.StoredProcedureDefs);


            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            IEnumerable<TEntity> result;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                result = await cxn.QueryAsync<TEntity>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);


            } else {
                result = await cxn.QueryAsync<TEntity>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }

            return result.ToList();

        }



        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public static List<dynamic> GetFromStoredProcedure<TContext>(this ISqlServerDbContext<TContext> context, string spName,
            IEnumerable<KeyValuePair<string, string>> parms)
            where TContext : DbContext {

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, context.StoredProcedureDefs);


            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            IEnumerable<dynamic> result;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                result = cxn.Query<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);


            } else {
                result = cxn.Query<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }

            return result.ToList();

        }



        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public static async Task<List<dynamic>> GetFromStoredProcedureAsync<TContext>(this ISqlServerDbContext<TContext> context, string spName,
            IEnumerable<KeyValuePair<string, string>> parms)
            where TContext : DbContext {

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, context.StoredProcedureDefs);


            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            IEnumerable<dynamic> result;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                result = await cxn.QueryAsync<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);


            } else {
                result = await cxn.QueryAsync<dynamic>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }

            return result.ToList();

        }






        public static void RebuildBuildStoredProcedureDefs<TContext>(ISqlServerDbContext<TContext> context)
            where TContext : DbContext {

            var cxn = context.Database.GetDbConnection();

            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                context.StoredProcedureDefs.Clear();
                context.StoredProcedureDefs.AddRange(cxn.Query<StoredProcedureDef>(sql: SQL_SERVER_STORED_PROCEDURE_DEFS,
                    transaction: dbTrans,
                    commandType: CommandType.Text)
                    .ToList());

            } else {
                context.StoredProcedureDefs.Clear();
                context.StoredProcedureDefs.AddRange(cxn.Query<StoredProcedureDef>(sql: SQL_SERVER_STORED_PROCEDURE_DEFS,
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
