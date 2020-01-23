using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.EntityFramework {
    public interface ISqlServerDbContext<TContext>
        where TContext : DbContext {
        DatabaseFacade Database { get; }
        StoredProcedureDefs<TContext> StoredProcedureDefs { get; set; }
    }

    public static class ISqlServerDbContextExtensions {


        /// <summary>
        /// Executes a FOR JSON stored procedure and returns json
        /// </summary>
        public static string GetJsonFromJsonStoredProcedure<TContext>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters)
            where TContext : DbContext => StoredProcedureExecutor.ExecuteFromJson(context, spName, context.StoredProcedureDefs, parameters);



        /// <summary>
        /// Asynchronously executes a FOR JSON stored procedure and returns json
        /// </summary>
        public static async Task<string> GetJsonFromJsonStoredProcedureAsync<TContext>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters)
            where TContext : DbContext => await StoredProcedureExecutor.ExecuteFromJsonAsync(context, spName, context.StoredProcedureDefs, parameters);


        /// <summary>
        /// Executes a regular stored procedure and returns a json array.
        /// </summary>
        public static string GetJsonArrayFromStoredProcedure<TContext>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters, params string[] jsonPropertiesToInclude)
            where TContext : DbContext => StoredProcedureExecutor.ExecuteToJsonArray(context, spName, context.StoredProcedureDefs, parameters, jsonPropertiesToInclude);



        /// <summary>
        /// Asynchronously executes a regular stored procedure and returns a json array.
        /// </summary>
        public static async Task<string> GetJsonArrayFromStoredProcedureAsync<TContext>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters, params string[] jsonPropertiesToInclude)
            where TContext : DbContext => await StoredProcedureExecutor.ExecuteToJsonArrayAsync(context, spName, context.StoredProcedureDefs, parameters, jsonPropertiesToInclude);



        /// <summary>
        /// Executes a regular stored procedure and returns a json object.
        /// </summary>
        public static string GetJsonObjectFromStoredProcedure<TContext>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters, params string[] jsonPropertiesToInclude)
            where TContext : DbContext => StoredProcedureExecutor.ExecuteToJsonObject(context, spName, context.StoredProcedureDefs, parameters, jsonPropertiesToInclude);



        /// <summary>
        /// Asynchronously executes a regular stored procedure and returns a json object.
        /// </summary>
        public static async Task<string> GetJsonObjectFromStoredProcedureAsync<TContext>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters, params string[] jsonPropertiesToInclude)
            where TContext : DbContext => await StoredProcedureExecutor.ExecuteToJsonObjectAsync(context, spName, context.StoredProcedureDefs, parameters, jsonPropertiesToInclude);



        /// <summary>
        /// Executes a stored procedure and returns a scalar.
        /// </summary>
        public static string GetScalarFromStoredProcedure<TContext, TResult>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters)
            where TContext : DbContext => StoredProcedureExecutor.ExecuteToScalar<TContext,TResult>(context, spName, context.StoredProcedureDefs, parameters);


        /// <summary>
        /// Asynchronously executes a stored procedure and returns a scalar.
        /// </summary>
        public static async Task<string> GetScalarFromStoredProcedureAsync<TContext, TResult>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters)
            where TContext : DbContext => await StoredProcedureExecutor.ExecuteToScalarAsync<TContext, TResult>(context, spName, context.StoredProcedureDefs, parameters);


        /// <summary>
        /// Executes a stored procedure and returns a list.
        /// </summary>
        public static List<TEntity> GetListFromStoredProcedure<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters)
            where TContext : DbContext => StoredProcedureExecutor.ExecuteToList<TContext, TEntity>(context, spName, context.StoredProcedureDefs, parameters);



        /// <summary>
        /// Asynchronously, executes a stored procedure and returns a list.
        /// </summary>
        public static async Task<List<TEntity>> GetListFromStoredProcedureAsync<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters)
            where TContext : DbContext => await StoredProcedureExecutor.ExecuteToListAsync<TContext, TEntity>(context, spName, context.StoredProcedureDefs, parameters);



        /// <summary>
        /// Executes a stored procedure and returns an object.
        /// </summary>
        public static TEntity GetObjectFromStoredProcedure<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters)
            where TContext : DbContext => StoredProcedureExecutor.ExecuteToObject<TContext, TEntity>(context, spName, context.StoredProcedureDefs, parameters);



        /// <summary>
        /// Asynchronously, executes a stored procedure and returns an object.
        /// </summary>
        public static async Task<TEntity> GetObjectFromStoredProcedureAsync<TContext, TEntity>(this ISqlServerDbContext<TContext> context, string spName,
            Dictionary<string, object> parameters)
            where TContext : DbContext => await StoredProcedureExecutor.ExecuteToObjectAsync<TContext, TEntity>(context, spName, context.StoredProcedureDefs, parameters);



    }
}
