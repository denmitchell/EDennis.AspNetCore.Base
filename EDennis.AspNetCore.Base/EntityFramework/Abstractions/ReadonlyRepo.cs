using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    public abstract class ReadonlyRepo<TEntity, TContext> : IRepo
            where TEntity : class, new()
            where TContext : DbContext {


        private List<StoredProcedureDef> _spDefs;

        public TContext Context { get; set; }


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public ReadonlyRepo(TContext context) {
            Context = context;
        }


        /// <summary>
        /// Provides direct access to the Query property of the context,
        /// allowing any query to be constructed via Linq expression
        /// </summary>
        public IQueryable<TEntity> Query { get => Context.Query<TEntity>(); }


        /// <summary>
        /// Get by Dynamic Linq Expression
        /// https://github.com/StefH/System.Linq.Dynamic.Core
        /// https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
        /// </summary>
        /// <param name="where">string Where expression</param>
        /// <param name="orderBy">string OrderBy expression (with support for descending)</param>
        /// <param name="select">string Select expression</param>
        /// <param name="skip">int number of records to skip</param>
        /// <param name="take">int number of records to return</param>
        /// <returns>dynamic-typed object</returns>
        public virtual List<dynamic> GetFromDynamicLinq(
                string where = null,
                string orderBy = null,
                string select = null,
                int? skip = null,
                int? take = null) {

            IQueryable qry = Query;

            if (where != null)
                qry = qry.Where(where);
            if (orderBy != null)
                qry = qry.OrderBy(orderBy);
            if (select != null)
                qry = qry.Select(select);
            if (skip != null)
                qry = qry.Skip(skip.Value);
            if (take != null)
                qry = qry.Take(take.Value);

            return qry.ToDynamicList();

        }


        /// <summary>
        /// Get by Dynamic Linq Expression
        /// https://github.com/StefH/System.Linq.Dynamic.Core
        /// https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
        /// </summary>
        /// <param name="where">string Where expression</param>
        /// <param name="orderBy">string OrderBy expression (with support for descending)</param>
        /// <param name="select">string Select expression</param>
        /// <param name="skip">int number of records to skip</param>
        /// <param name="take">int number of records to return</param>
        /// <returns>dynamic-typed object</returns>
        public virtual async Task<List<dynamic>> GetFromDynamicLinqAsync(
                string where = null,
                string orderBy = null,
                string select = null,
                int? skip = null,
                int? take = null) {

            IQueryable qry = Query;

            if (where != null)
                qry = qry.Where(where);
            if (orderBy != null)
                qry = qry.OrderBy(orderBy);
            if (select != null)
                qry = qry.Select(select);
            if (skip != null)
                qry = qry.Skip(skip.Value);
            if (take != null)
                qry = qry.Take(take.Value);

            return await qry.ToDynamicListAsync();

        }



        /// <summary>
        /// Gets a list of TEntity using the provided
        /// SQL select statement.  Note: because this is
        /// a read-only query, Entity Framework will 
        /// throw an error if you attempt to perform
        /// a write operation (e.g, INSERT, UPDATE, or DELETE)
        /// </summary>
        /// <param name="sql">A valid SQL SELECT statement</param>
        /// <returns></returns>
        public virtual List<TEntity> GetFromSql(string sql){

            //var parms = new DynamicParameters();
            //parms.Add()

            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            List<TEntity> result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.Query<TEntity>(sql, transaction: dbTrans).AsList();
            } else {
                result = cxn.Query<TEntity>(sql).AsList();
            }
            return result;
        }


        /// <summary>
        /// Asynchronously gets a list of TEntity using the provided
        /// SQL select statement.  Note: because this is
        /// a read-only query, Entity Framework will 
        /// throw an error if you attempt to perform
        /// a write operation (e.g, INSERT, UPDATE, or DELETE)
        /// </summary>
        /// <param name="sql">A valid SQL SELECT statement</param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetFromSqlAsync(string sql){
            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            List<TEntity> result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = (await cxn.QueryAsync<TEntity>(sql, transaction: dbTrans)).AsList();
            } else {
                result = (await cxn.QueryAsync<TEntity>(sql)).AsList();
            }
            return result;
        }


        /// <summary>
        /// Retrieves a scalar value from the database using
        /// the provided SQL SELECT statement
        /// </summary>
        /// <typeparam name="T">The type of object to return</typeparam>
        /// <param name="sql">Valid SQL SELECT statement returning a scalar</param>
        /// <returns></returns>
        public virtual T GetScalarFromSql<T>(string sql) {
            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            T result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = cxn.ExecuteScalar<T>(sql, transaction: dbTrans);
            } else {
                result = cxn.ExecuteScalar<T>(sql);
            }
            return result;
        }


        /// <summary>
        /// Asynchronously retrieves a scalar value from the database
        /// using the provided SQL SELECT statement
        /// </summary>
        /// <typeparam name="T">The type of object to return</typeparam>
        /// <param name="sql">Valid SQL SELECT statement returning a scalar</param>
        /// <returns></returns>
        public virtual async Task<T> GetScalarFromSqlAsync<T>(string sql) {
            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            T result;
            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();
                result = await cxn.ExecuteScalarAsync<T>(sql, transaction: dbTrans);
            } else {
                result = await cxn.ExecuteScalarAsync<T>(sql);
            }
            return result;
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
        public virtual string GetJsonColumnFromStoredProcedure(
            string spName,
            IEnumerable<KeyValuePair<string, string>> parms) {

            if (_spDefs == null)
                BuildStoredProcedureDefs();

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, _spDefs);

            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;

            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                dynamic result = cxn.QuerySingle<dynamic>(sql: $"exec {spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = result.Json;

            } else {
                dynamic result = cxn.QuerySingle<dynamic>(sql: $"exec {spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);
                json = result.Json;
            }

            return json;
        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public virtual async Task<string> GetJsonColumnFromStoredProcedureAsync(
            string spName,
            IEnumerable<KeyValuePair<string, string>> parms) {

            if (_spDefs == null)
                BuildStoredProcedureDefs();

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, _spDefs);

            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();
            string json;

            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                dynamic result = await cxn.QuerySingleAsync<dynamic>(sql: $"exec {spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);

                json = result.Json;

            } else {
                dynamic result = await cxn.QuerySingleAsync<dynamic>(sql: $"exec {spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);
                json = result.Json;
            }

            return json;
        }




        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public virtual dynamic GetFromStoredProcedure(
            string spName,
            IEnumerable<KeyValuePair<string,string>> parms) {

            if (_spDefs == null)
                BuildStoredProcedureDefs();

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms,_spDefs);

            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            dynamic result;

            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                result = cxn.Query<dynamic>(sql: $"exec {spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);


            } else {
                result = cxn.Query<dynamic>(sql: $"exec {spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }

            return result;
        }




        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public virtual async Task<dynamic> GetFromStoredProcedureAsync(
            string spName,
            IEnumerable<KeyValuePair<string, string>> parms) {

            if (_spDefs == null)
                BuildStoredProcedureDefs();

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, _spDefs);

            var cxn = Context.Database.GetDbConnection();
            if (cxn.State == ConnectionState.Closed)
                cxn.Open();

            dynamic result;

            if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                var dbTrans = trans.GetDbTransaction();

                result = await cxn.QueryAsync<dynamic>(sql: $"exec {spName}",
                    param: dynamicParameters,
                    transaction: dbTrans,
                    commandType: CommandType.StoredProcedure);


            } else {
                result = await cxn.QueryAsync<dynamic>(sql: $"exec {spName}",
                    param: dynamicParameters,
                    commandType: CommandType.StoredProcedure);
            }

            return result;
        }


        public virtual void BuildStoredProcedureDefs() {

            _spDefs = new List<StoredProcedureDef>();

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
   p1.name [ParameterName],  
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

