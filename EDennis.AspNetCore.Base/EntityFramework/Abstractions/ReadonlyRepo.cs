﻿using Dapper;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
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
        public IScopeProperties ScopeProperties { get; set; }


        protected ILogger Logger;

        #region logging support
        /// <summary>
        /// Method Name (qualified by Class Name)
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected string M(string m) => $"{GetType().Name}.{m}";

        /// <summary>
        /// User (from ScopeProperties)
        /// </summary>
        protected string U;

        /// <summary>
        /// Data that serializes to Json upon call to ToString()
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public AutoJson D(object data) => new AutoJson(data);
        #endregion

        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public ReadonlyRepo(TContext context,
                IScopeProperties scopeProperties,
                IEnumerable<ILogger<ReadonlyRepo<TEntity, TContext>>> loggers) {

            Context = context;
            ScopeProperties = scopeProperties;
            U = ScopeProperties.User;

            Logger = loggers.ElementAt(scopeProperties.LoggerIndex);

        }


        /// <summary>
        /// Provides direct access to the Query property of the context,
        /// allowing any query to be constructed via Linq expression
        /// </summary>
        public IQueryable<TEntity> Query {
            get {
                #region logging
                Logger.LogDebug("For {User}, {Method}", U, M("Query"));
                #endregion
                return Context.Set<TEntity>().AsNoTracking();
            }
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
        public virtual List<dynamic> GetFromDynamicLinq(
                string where = null, string orderBy = null, string select = null,
                int? skip = null, int? take = null) {

            #region logging
            Logger.LogDebug("For {User}, {Method}\n\twhere={where}\n\torderBy={orderBy}\n\tselect={select}\n\tskip={skip}\n\ttake={take}",
                U, M("GetFromDynamicLinq"), where, orderBy, select, skip, take);
            #endregion
            IQueryable qry = Query;

            try {
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

                var returnVal = qry.ToDynamicList();

                #region logging
                Logger.LogTrace("For {User}, {Method}\n\twhere={where}\n\torderBy={orderBy}\n\tselect={select}\n\tskip={skip}\n\ttake={take}\n\treturning {Return}",
                    U, M("GetFromDynamicLinq"), where, orderBy, select, skip, take, D(returnVal));
                #endregion
                return returnVal;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\twhere={where}\n\torderBy={orderBy}\n\tselect={select}\n\tskip={skip}\n\ttake={take}\n\tError: {Error}",
                    U, M("GetFromDynamicLinq"), where, orderBy, select, skip, take, ex.Message);
                #endregion
                throw;
            }


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
                string where = null, string orderBy = null, string select = null,
                int? skip = null, int? take = null) {

            #region logging
            Logger.LogDebug("For {User}, {Method}\n\twhere={where}\n\torderBy={orderBy}\n\tselect={select}\n\tskip={skip}\n\ttake={take}",
                U, M("GetFromDynamicLinqAsync"), where, orderBy, select, skip, take);
            #endregion
            IQueryable qry = Query;

            try {
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

                var returnVal = await qry.ToDynamicListAsync();

                #region logging
                Logger.LogTrace("For {User}, {Method}\n\twhere={where}\n\torderBy={orderBy}\n\tselect={select}\n\tskip={skip}\n\ttake={take}\n\treturning {Return}",
                    U, M("GetFromDynamicLinqAsync"), where, orderBy, select, skip, take, D(returnVal));
                #endregion
                return returnVal;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\twhere={where}\n\torderBy={orderBy}\n\tselect={select}\n\tskip={skip}\n\ttake={take}\n\tError: {Error}",
                    U, M("GetFromDynamicLinqAsync"), where, orderBy, select, skip, take, ex.Message);
                #endregion
                throw;
            }

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
        public virtual List<TEntity> GetFromSql(string sql) {

            #region logging
            Logger.LogDebug("For {User}, {Method}\n\tsql={sql}", U, M("GetFromSql"), sql);
            #endregion
            try {
                var cxn = Context.Database.GetDbConnection();
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tOpening Connection {ConnectionString}",
                    U, M("GetFromSql"), cxn.ConnectionString);
                #endregion
                if (cxn.State == ConnectionState.Closed)
                    cxn.Open();
                List<TEntity> result;
                if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                    var dbTrans = trans.GetDbTransaction();
                    result = cxn.Query<TEntity>(sql, transaction: dbTrans).AsList();
                } else {
                    result = cxn.Query<TEntity>(sql).AsList();
                }

                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tsql={sql}\n\tReturning {Return}",
                    U, M("GetFromSql"), sql, D(result));
                #endregion
                return result;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\tsql={sql}\n\tError: {Error}",
                    U, M("GetFromSql"), sql, ex.Message);
                #endregion
                throw;
            }
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
        public virtual async Task<List<TEntity>> GetFromSqlAsync(string sql) {

            #region logging
            Logger.LogDebug("For {User}, {Method}\n\tsql={sql}", U, M("GetFromSqlAsync"), sql);
            #endregion

            try {
                var cxn = Context.Database.GetDbConnection();
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tOpening Connection {ConnectionString}",
                    U, M("GetFromSqlAsync"), cxn.ConnectionString);
                #endregion
                if (cxn.State == ConnectionState.Closed)
                    cxn.Open();
                List<TEntity> result;
                if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                    var dbTrans = trans.GetDbTransaction();
                    result = (await cxn.QueryAsync<TEntity>(sql, transaction: dbTrans)).AsList();
                } else {
                    result = (await cxn.QueryAsync<TEntity>(sql)).AsList();
                }
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tsql={sql}\n\tReturning {Return}",
                    U, M("GetFromSqlAsync"), sql, D(result));
                #endregion
                return result;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\tsql={sql}\n\tError: {Error}",
                    U, M("GetFromSqlAsync"), sql, ex.Message);
                #endregion
                throw;
            }
        }


        /// <summary>
        /// Retrieves a scalar value from the database using
        /// the provided SQL SELECT statement
        /// </summary>
        /// <typeparam name="T">The type of object to return</typeparam>
        /// <param name="sql">Valid SQL SELECT statement returning a scalar</param>
        /// <returns></returns>
        public virtual T GetScalarFromSql<T>(string sql) {

            #region logging
            Logger.LogDebug("For {User}, {Method}\n\tsql={sql}", U, M("GetScalarFromSql"), sql);
            #endregion

            try {
                var cxn = Context.Database.GetDbConnection();
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tOpening Connection {ConnectionString}",
                    U, M("GetScalarFromSql"), cxn.ConnectionString);
                #endregion
                if (cxn.State == ConnectionState.Closed)
                    cxn.Open();
                T result;
                if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                    var dbTrans = trans.GetDbTransaction();
                    result = cxn.ExecuteScalar<T>(sql, transaction: dbTrans);
                } else {
                    result = cxn.ExecuteScalar<T>(sql);
                }
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tsql={sql}\n\tReturning {Return}",
                    U, M("GetScalarFromSql"), sql, D(result));
                #endregion
                return result;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\tsql={sql}\n\tError: {Error}",
                    U, M("GetScalarFromSql"), sql, ex.Message);
                #endregion
                throw;
            }
        }


        /// <summary>
        /// Asynchronously retrieves a scalar value from the database
        /// using the provided SQL SELECT statement
        /// </summary>
        /// <typeparam name="T">The type of object to return</typeparam>
        /// <param name="sql">Valid SQL SELECT statement returning a scalar</param>
        /// <returns></returns>
        public virtual async Task<T> GetScalarFromSqlAsync<T>(string sql) {

            #region logging
            Logger.LogDebug("For {User}, {Method}\n\tsql={sql}", U, M("GetScalarFromSqlAsync"), sql);
            #endregion

            try {
                var cxn = Context.Database.GetDbConnection();
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tOpening Connection {ConnectionString}",
                    U, M("GetScalarFromSqlAsync"), cxn.ConnectionString);
                #endregion
                if (cxn.State == ConnectionState.Closed)
                    cxn.Open();
                T result;
                if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                    var dbTrans = trans.GetDbTransaction();
                    result = await cxn.ExecuteScalarAsync<T>(sql, transaction: dbTrans);
                } else {
                    result = await cxn.ExecuteScalarAsync<T>(sql);
                }
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tsql={sql}\n\tReturning {Return}",
                    U, M("GetScalarFromSqlAsync"), sql, D(result));
                #endregion
                return result;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\tsql={sql}\n\tError: {Error}",
                    U, M("GetScalarFromSqlAsync"), sql, ex.Message);
                #endregion
                throw;
            }
        }



        /// <summary>
        /// Retrieves raw JSON from SQL Server using the
        /// provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public virtual string GetFromJsonSql(string fromJsonSql) {

            #region logging
            Logger.LogDebug("For {User}, {Method}\n\tsql={sql}", U, M("GetFromJsonSql"), fromJsonSql);
            #endregion

            try {
                var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
                var cxn = Context.Database.GetDbConnection();
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tOpening Connection {ConnectionString}",
                    U, M("GetFromJsonSql"), cxn.ConnectionString);
                #endregion
                if (cxn.State == ConnectionState.Closed)
                    cxn.Open();
                string result;
                if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                    var dbTrans = trans.GetDbTransaction();
                    result = cxn.ExecuteScalar<string>(sql, transaction: dbTrans);
                } else {
                    result = cxn.ExecuteScalar<string>(sql);
                }
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tsql={sql}\n\tReturning {Return}",
                    U, M("GetFromJsonSql"), sql, D(result));
                #endregion
                return result;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\tsql={sql}\n\tError: {Error}",
                    U, M("GetFromJsonSql"), fromJsonSql, ex.Message);
                #endregion
                throw;
            }
        }


        /// <summary>
        /// Asynchronously retrieves raw JSON from SQL Server 
        /// using the provided FOR JSON SQL SELECT statement
        /// </summary>
        /// <param name="fromJsonSql">Valid SQL SELECT statement with FOR JSON clause</param>
        /// <returns></returns>
        public virtual async Task<string> GetFromJsonSqlAsync(string fromJsonSql) {

            #region logging
            Logger.LogDebug("For {User}, {Method}\n\tsql={sql}", U, M("GetFromJsonSqlAsync"), fromJsonSql);
            #endregion

            try {
                var sql = $"declare @j varchar(max) = ({fromJsonSql}); select @j json;";
                var cxn = Context.Database.GetDbConnection();
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tOpening Connection {ConnectionString}",
                    U, M("GetFromJsonSqlAsync"), cxn.ConnectionString);
                #endregion
                if (cxn.State == ConnectionState.Closed)
                    cxn.Open();
                string result;
                if (Context.Database.CurrentTransaction is IDbContextTransaction trans) {
                    var dbTrans = trans.GetDbTransaction();
                    result = await cxn.ExecuteScalarAsync<string>(sql, transaction: dbTrans);
                } else {
                    result = await cxn.ExecuteScalarAsync<string>(sql);
                }
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tsql={sql}\n\tReturning {Return}",
                    U, M("GetFromJsonSqlAsync"), sql, D(result));
                #endregion
                return result;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\tsql={sql}\n\tError: {Error}",
                    U, M("GetFromJsonSqlAsync"), fromJsonSql, ex.Message);
                #endregion
                throw;
            }
        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public virtual string GetJsonColumnFromStoredProcedure(
            string spName, IEnumerable<KeyValuePair<string, string>> parms) {


            #region logging
            Logger.LogDebug("For {User}, {Method}\n\tspName={spName}\n\tparms={parms}",
                U, M("GetJsonColumnFromStoredProcedure"), spName, D(parms));
            #endregion

            if (_spDefs == null) {
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tspName={spName},\n\tinvoking {Invoking}",
                    U, M("GetJsonColumnFromStoredProcedure"), spName, "BuildStoredProcedureDefs()");
                #endregion
                BuildStoredProcedureDefs();
            }

            #region logging
            Logger.LogTrace("For {User}, {Method}\n\tspName={spName},\n\tinvoking {Invoking}",
                U, M("GetJsonColumnFromStoredProcedure"), spName, "DynamicParameters.AddRange(...)");
            #endregion

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, _spDefs);

            #region logging
            Logger.LogTrace("For {User}, {Method}\n\tspName={spName},\n\tDynamic Parameters: {DynamicParameters}",
                U, M("GetJsonColumnFromStoredProcedure"), spName, D(dynamicParameters));
            #endregion

            try {

                var cxn = Context.Database.GetDbConnection();
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tOpening Connection {ConnectionString}",
                    U, M("GetJsonColumnFromStoredProcedure"), cxn.ConnectionString);
                #endregion
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

                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tspName={spName}\n\tparms={parms}\n\tReturning: {Return}",
                    U, M("GetJsonColumnFromStoredProcedure"), spName, D(parms), json);
                #endregion
                return json;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\tspName={spName}\n\tparms={parms}\n\tError: {Error}",
                    U, M("GetJsonColumnFromStoredProcedure"), spName, D(parms), ex.Message);
                #endregion
                throw;
            }
        }



        /// <summary>
        /// Retrieves a string-typed column named "Json" from a database.
        /// This can be used to return a flat or hierarchical JSON-structured
        /// result (e.g., using FOR JSON with SQL Server)
        /// </summary>
        public virtual async Task<string> GetJsonColumnFromStoredProcedureAsync(
            string spName,
            IEnumerable<KeyValuePair<string, string>> parms) {

            #region logging
            Logger.LogDebug("For {User}, {Method}\n\tspName={spName}\n\tparms={parms}",
                U, M("GetJsonColumnFromStoredProcedureAsync"), spName, D(parms));
            #endregion

            if (_spDefs == null) {
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tspName={spName},\n\tinvoking {Invoking}",
                    U, M("GetJsonColumnFromStoredProcedureAsync"), spName, "BuildStoredProcedureDefs()");
                #endregion
                BuildStoredProcedureDefs();
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, _spDefs);

            #region logging
            Logger.LogTrace("For {User}, {Method}\n\tspName={spName},\n\tDynamic Parameters: {DynamicParameters}",
                U, M("GetJsonColumnFromStoredProcedureAsync"), spName, D(dynamicParameters));
            #endregion

            try {

                var cxn = Context.Database.GetDbConnection();
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tOpening Connection {ConnectionString}",
                    U, M("GetJsonColumnFromStoredProcedureAsync"), cxn.ConnectionString);
                #endregion
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

                #region logging
                Logger.LogDebug("For {User}, {Method}\n\tspName={spName}\n\tparms={parms}\n\tReturning: {Return}",
                    U, M("GetJsonColumnFromStoredProcedureAsync"), spName, D(parms), json);
                #endregion
                return json;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\tspName={spName}\n\tparms={parms}\n\tError: {Error}",
                    U, M("GetJsonColumnFromStoredProcedureAsync"), spName, D(parms), ex.Message);
                #endregion
                throw;
            }

        }




        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public virtual dynamic GetFromStoredProcedure(
            string spName, IEnumerable<KeyValuePair<string, string>> parms) {

            #region logging
            Logger.LogDebug("For {User}, {Method}\n\tspName={spName}\n\tparms={parms}",
                U, M("GetFromStoredProcedure"), spName, D(parms));
            #endregion

            if (_spDefs == null) {
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tspName={spName},\n\tinvoking {Invoking}",
                    U, M("GetFromStoredProcedure"), spName, "BuildStoredProcedureDefs()");
                #endregion
                BuildStoredProcedureDefs();
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, _spDefs);

            #region logging
            Logger.LogTrace("For {User}, {Method}\n\tspName={spName},\n\tDynamic Parameters: {DynamicParameters}",
                U, M("GetFromStoredProcedure"), spName, D(dynamicParameters));
            #endregion


            try {

                var cxn = Context.Database.GetDbConnection();
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tOpening Connection {ConnectionString}",
                    U, M("GetFromStoredProcedure"), cxn.ConnectionString);
                #endregion

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

                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tspName={spName}\n\tparms={parms}\n\tReturning: {Return}",
                    U, M("GetFromStoredProcedure"), spName, D(parms), D((object)result));
                #endregion
                return result;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\tspName={spName}\n\tparms={parms}\n\tError: {Error}",
                    U, M("GetFromStoredProcedure"), spName, D(parms), ex.Message);
                #endregion
                throw;
            }
        }




        /// <summary>
        /// Retrieves a result via a parameterized stored procedure.
        /// </summary>
        public virtual async Task<dynamic> GetFromStoredProcedureAsync(
            string spName,
            IEnumerable<KeyValuePair<string, string>> parms) {

            #region logging
            Logger.LogDebug("For {User}, {Method}\n\tspName={spName}\n\tparms={parms}",
                U, M("GetFromStoredProcedureAsync"), spName, D(parms));
            #endregion

            if (_spDefs == null) {
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tspName={spName},\n\tinvoking {Invoking}",
                    U, M("GetFromStoredProcedureAsync"), spName, "BuildStoredProcedureDefs()");
                #endregion
                BuildStoredProcedureDefs();
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.AddRange(spName, parms, _spDefs);


            #region logging
            Logger.LogTrace("For {User}, {Method}\n\tspName={spName},\n\tDynamic Parameters: {DynamicParameters}",
                U, M("GetFromStoredProcedureAsync"), spName, D(dynamicParameters));
            #endregion


            try {

                var cxn = Context.Database.GetDbConnection();
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tOpening Connection {ConnectionString}",
                    U, M("GetFromStoredProcedure"), cxn.ConnectionString);
                #endregion

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

                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tspName={spName}\n\tparms={parms}\n\tReturning: {Return}",
                    U, M("GetFromStoredProcedureAsync"), spName, D(parms), D((object)result));
                #endregion
                return result;

            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\tspName={spName}\n\tparms={parms}\n\tError: {Error}",
                    U, M("GetFromStoredProcedureAsync"), spName, D(parms), ex.Message);
                #endregion
                throw;
            }
        }


        public virtual void BuildStoredProcedureDefs() {

            #region logging
            Logger.LogDebug("For {User}, {Method}", U, M("BuildStoredProcedureDefs"));
            #endregion

            _spDefs = new List<StoredProcedureDef>();

            try {

                var cxn = Context.Database.GetDbConnection();
                #region logging
                Logger.LogTrace("For {User}, {Method}\n\tOpening Connection {ConnectionString}",
                    U, M("BuildStoredProcedureDefs"), cxn.ConnectionString);
                #endregion

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



            } catch (Exception ex) {
                #region logging
                Logger.LogError(ex, "For {User}, {Method}\n\tError: {Error}",
                    U, M("BuildStoredProcedureDefs"), ex.Message);
                #endregion
                throw;
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

