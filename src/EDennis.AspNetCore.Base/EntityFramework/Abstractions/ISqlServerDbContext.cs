using Dapper;
using EDennis.AspNetCore.Base.Serialization;
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
        public static List<TEntity> GetListFromJsonSql<TContext,TEntity>(this ISqlServerDbContext<TContext> context, string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();

            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                json = cxn.ExecuteScalar<string>(sql, transaction: dbTrans);
            } else {
                json = cxn.ExecuteScalar<string>(sql);
            }
            var result = JsonSerializer.Deserialize<List<TEntity>>(json);
            return result;
        }


        /// <summary>
        /// Asynchronously retrieves raw JSON from SQL Server 
        /// using the provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public static async Task<List<TEntity>> GetListFromJsonSqlAsync<TContext,TEntity>(this ISqlServerDbContext<TContext> context, string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                json = await cxn.ExecuteScalarAsync<string>(sql, transaction: dbTrans);
            } else {
                json = await cxn.ExecuteScalarAsync<string>(sql);
            }
            var result = JsonSerializer.Deserialize<List<TEntity>>(json);
            return result;
        }


        /// <summary>
        /// Retrieves raw JSON from SQL Server using the
        /// provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public static PagedResult<dynamic> GetPagedResultFromJsonSql<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();

            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                json = cxn.ExecuteScalar<string>(sql, transaction: dbTrans);
            } else {
                json = cxn.ExecuteScalar<string>(sql);
            }
            var options = new JsonSerializerOptions();
            options.Converters.Add(new DynamicJsonConverter<TEntity>());
            var result = JsonSerializer.Deserialize<PagedResult<dynamic>>(json, options);
            return result;
        }


        /// <summary>
        /// Asynchronously retrieves raw JSON from SQL Server 
        /// using the provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public static async Task<PagedResult<dynamic>> GetPagedResultFromJsonSqlAsync<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                json = await cxn.ExecuteScalarAsync<string>(sql, transaction: dbTrans);
            } else {
                json = await cxn.ExecuteScalarAsync<string>(sql);
            }
            var options = new JsonSerializerOptions();
            options.Converters.Add(new DynamicJsonConverter<TEntity>());
            var result = JsonSerializer.Deserialize<PagedResult<dynamic>>(json, options);
            return result;

        }


        /// <summary>
        /// Retrieves raw JSON from SQL Server using the
        /// provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public static TEntity GetSingleFromJsonSql<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();

            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                json = cxn.ExecuteScalar<string>(sql, transaction: dbTrans);
            } else {
                json = cxn.ExecuteScalar<string>(sql);
            }
            return JsonSerializer.Deserialize<TEntity>(json);

        }


        /// <summary>
        /// Asynchronously retrieves raw JSON from SQL Server 
        /// using the provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public static async Task<TEntity> GetSingleFromJsonSqlAsync<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                json = await cxn.ExecuteScalarAsync<string>(sql, transaction: dbTrans);
            } else {
                json = await cxn.ExecuteScalarAsync<string>(sql);
            }
            return JsonSerializer.Deserialize<TEntity>(json);

        }



        /// <summary>
        /// Retrieves raw JSON from SQL Server using the
        /// provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public static string GetJsonFromJsonSql<TContext>(this ISqlServerDbContext<TContext> context, string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();

            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                json = cxn.ExecuteScalar<string>(sql, transaction: dbTrans);
            } else {
                json = cxn.ExecuteScalar<string>(sql);
            }
            return json;
        }


        /// <summary>
        /// Asynchronously retrieves raw JSON from SQL Server 
        /// using the provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public static async Task<string> GetJsonFromJsonSqlAsync<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string fromJsonSql)
            where TContext : DbContext {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;
            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                json = await cxn.ExecuteScalarAsync<string>(sql, transaction: dbTrans);
            } else {
                json = await cxn.ExecuteScalarAsync<string>(sql);
            }
            return json;
        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static TEntity GetSingleFromJsonStoredProcedure<TContext,TEntity>(this ISqlServerDbContext<TContext> context, string spName,
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

                JsonColumnResult result = cxn.QuerySingle<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = result.Json;

            } else {
                JsonColumnResult result = cxn.QuerySingle<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = result.Json;
            }

            return JsonSerializer.Deserialize<TEntity>(json);

        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static async Task<TEntity> GetSingleFromJsonStoredProcedureAsync<TContext,TEntity>(this ISqlServerDbContext<TContext> context, string spName,
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

                JsonColumnResult result = await cxn.QuerySingleAsync<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = result.Json;

            } else {
                JsonColumnResult result = await cxn.QuerySingleAsync<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = result.Json;
            }

            return JsonSerializer.Deserialize<TEntity>(json);


        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static List<TEntity> GetListFromJsonStoredProcedure<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
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

                 JsonColumnResult result = cxn.QuerySingle<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = result.Json;

            } else {
                JsonColumnResult result = cxn.QuerySingle<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = result.Json;
            }

            

            return JsonSerializer.Deserialize<List<TEntity>>(json);

        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static async Task<List<TEntity>> GetListFromJsonStoredProcedureAsync<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
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

                JsonColumnResult result = await cxn.QuerySingleAsync<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = result.Json;

            } else {
                JsonColumnResult result = await cxn.QuerySingleAsync<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = result.Json;
            }

            return JsonSerializer.Deserialize<List<TEntity>>(json);


        }




        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static PagedResult<dynamic> GetPagedResultFromJsonStoredProcedure<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
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

                JsonColumnResult jsonColumnResult = cxn.QuerySingle<JsonColumnResult>(sql: $"{spName}",
                   param: dynamicParameters,
                   transaction: dbTrans,
                   commandType: CommandType.StoredProcedure);

                json = jsonColumnResult.Json;

            } else {
                JsonColumnResult jsonColumnResult = cxn.QuerySingle<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = jsonColumnResult.Json;
            }


            var options = new JsonSerializerOptions();
            options.Converters.Add(new DynamicJsonConverter<TEntity>());
            var result = JsonSerializer.Deserialize<PagedResult<dynamic>>(json, options);
            return result;

        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static async Task<PagedResult<dynamic>> GetPagedResultFromJsonStoredProcedureAsync<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
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

                JsonColumnResult jsonColumnResult = await cxn.QuerySingleAsync<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = jsonColumnResult.Json;

            } else {
                JsonColumnResult jsonColumnResult = await cxn.QuerySingleAsync<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = jsonColumnResult.Json;
            }

            var options = new JsonSerializerOptions();
            options.Converters.Add(new DynamicJsonConverter<TEntity>());
            var result = JsonSerializer.Deserialize<PagedResult<dynamic>>(json, options);
            return result;

        }




        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static string GetJsonFromJsonStoredProcedure<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
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

                JsonColumnResult jsonColumnResult = cxn.QuerySingle<JsonColumnResult>(sql: $"{spName}",
                   param: dynamicParameters,
                   transaction: dbTrans,
                   commandType: CommandType.StoredProcedure);

                json = jsonColumnResult.Json;

            } else {
                JsonColumnResult jsonColumnResult = cxn.QuerySingle<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = jsonColumnResult.Json;
            }

            return json;

        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public static async Task<string> GetJsonFromJsonStoredProcedureAsync<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
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

                JsonColumnResult jsonColumnResult = await cxn.QuerySingleAsync<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = jsonColumnResult.Json;

            } else {
                JsonColumnResult jsonColumnResult = await cxn.QuerySingleAsync<JsonColumnResult>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                json = jsonColumnResult.Json;
            }

            return json;

        }



        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public static List<TEntity> GetListFromStoredProcedure<TContext, TEntity>(this ISqlServerDbContext<TContext> context,  string spName,
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

            //TODO: populate output parameters

            return result.ToList();

        }



        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public static async Task<List<TEntity>> GetListFromStoredProcedureAsync<TContext,TEntity>(this ISqlServerDbContext<TContext> context,  string spName,
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

            //TODO: populate output parameters

            return result.ToList();

        }




        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public static TEntity GetSingleFromStoredProcedure<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
            IEnumerable<KeyValuePair<string, string>> parms)
            where TContext : DbContext {

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, context.StoredProcedureDefs);


            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            TEntity result;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                result = cxn.QuerySingle<TEntity>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);


            } else {
                result = cxn.QuerySingle<TEntity>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }

            return result;

        }



        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public static async Task<TEntity> GetSingleFromStoredProcedureAsync<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
            IEnumerable<KeyValuePair<string, string>> parms)
            where TContext : DbContext {

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, context.StoredProcedureDefs);


            var cxn = context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            TEntity result;

            if (context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                result = await cxn.QuerySingleAsync<TEntity>(sql: $"{spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);


            } else {
                result = await cxn.QuerySingleAsync<TEntity>(sql: $"{spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }

            return result;

        }





        public static void RebuildStoredProcedureDefs<TContext>(ISqlServerDbContext<TContext> context)
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
