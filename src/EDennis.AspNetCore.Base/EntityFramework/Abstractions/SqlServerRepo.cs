using Dapper;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    public class SqlServerRepo<TEntity, TContext> : RelationalRepo<TEntity, TContext>, ISqlServerRepo<TEntity,TContext> 
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext {

        protected List<StoredProcedureDef> _spDefs;

        public SqlServerRepo(DbContextProvider<TContext> provider,
            IScopeProperties scopeProperties,
            ILogger<Repo<TEntity, TContext>> logger,
            IScopedLogger scopedLogger)
            : base(provider, scopeProperties, logger, scopedLogger) {
        }

        /// <summary>
        /// Retrieves raw JSON from SQL Server using the
        /// provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public virtual string GetFromJsonSql(string fromJsonSql) {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = Context.Database.GetDbConnection();

            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
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
        public virtual async Task<string> GetFromJsonSqlAsync(string fromJsonSql) {

            var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<string>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<string>(sql);
            }
            return result;

        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public virtual string GetJsonColumnFromStoredProcedure(string spName,
            IEnumerable<KeyValuePair<string, string>> parms) {


            if (_spDefs == null) {
                BuildStoredProcedureDefs();
            }


            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, _spDefs);


            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;

            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
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
        public virtual async Task<string> GetJsonColumnFromStoredProcedureAsync(string spName,
            IEnumerable<KeyValuePair<string, string>> parms) {


            if (_spDefs == null) {
                BuildStoredProcedureDefs();
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, _spDefs);


            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;

            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
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
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public virtual dynamic GetFromStoredProcedure(string spName,
            IEnumerable<KeyValuePair<string, string>> parms) {

            if (_spDefs == null) {
                BuildStoredProcedureDefs();
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, _spDefs);


            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            dynamic result;

            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
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

            return result;

        }



        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public virtual async Task<dynamic> GetFromStoredProcedureAsync(string spName,
            IEnumerable<KeyValuePair<string, string>> parms) {

            if (_spDefs == null) {
                BuildStoredProcedureDefs();
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, _spDefs);


            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            dynamic result;

            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
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

            return result;

        }


        protected virtual void BuildStoredProcedureDefs() {

            var cxn = Context.Database.GetDbConnection();

            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                _spDefs = cxn.Query<StoredProcedureDef>(sql: SQL_SERVER_STORED_PROCEDURE_DEFS,
                    transaction: dbTrans,
                    commandType: CommandType.Text)
                    .ToList();

            } else {
                _spDefs = cxn.Query<StoredProcedureDef>(sql: SQL_SERVER_STORED_PROCEDURE_DEFS,
                    commandType: CommandType.Text)
                    .ToList();
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
