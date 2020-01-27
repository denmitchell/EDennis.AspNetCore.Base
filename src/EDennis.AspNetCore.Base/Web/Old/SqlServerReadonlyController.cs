using DevExtreme.AspNet.Data;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Serialization;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using System.Reflection;
using Microsoft.Extensions.Primitives;
using System.Data;

namespace EDennis.AspNetCore.Base.Web {

    [ApiController]
    [Route("api/[controller]")]
    public abstract class SqlServerReadonlyController<TEntity, TContext> : ControllerBase
            where TEntity : class, IHasSysUser, new()
            where TContext : DbContext, ISqlServerDbContext<TContext> {

        public Repo<TEntity, TContext> Repo { get; }
        public ILogger Logger { get; }

        public JsonSerializerOptions JsonSerializationOptions { get; set; }

        public SqlServerReadonlyController(Repo<TEntity, TContext> repo,ILogger logger) {
            Repo = repo;
            Logger = logger;

            JsonSerializationOptions = new JsonSerializerOptions();
            JsonSerializationOptions.Converters.Add(new DynamicJsonConverter<TEntity>());
        }

        /*
         * 
         * ODATA REQUIRES TOO MANY WORKAROUNDS IN 3.1, AND BREAKS SWAGGER UI

        /// <summary>
        /// Get from OData query string
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ODataQueryFilter]
        [HttpGet("odata")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public IEnumerable<TEntity> GetOData(
                [FromQuery]string select,
                [FromQuery]string orderBy,
                [FromQuery]string filter,
                [FromQuery]string expand,
                [FromQuery]int skip,
                [FromQuery]int top
            ) {
            return Repo.Query;
        }

    */

        /// <summary>
        /// Get from DevExtreme DataSourceLoader query string
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("devextreme")]
        public IActionResult GetDevExtreme(
                [FromQuery]string select,
                [FromQuery]string sort,
                [FromQuery]string filter,
                [FromQuery]int skip,
                [FromQuery]int take,
                [FromQuery]string totalSummary,
                [FromQuery]string group,
                [FromQuery]string groupSummary
            ) {
            var loadOptions = DataSourceLoadOptionsBuilder.Build(
                select, sort, filter, skip, take, totalSummary,
                group, groupSummary);

            var result = DataSourceLoader.Load(Repo.Query, loadOptions);
            return Ok(result);
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
        [HttpGet("linq")]
        public IActionResult GetDynamicLinq(
                [FromQuery]string where = null,
                [FromQuery]string orderBy = null,
                [FromQuery]string select = null,
                [FromQuery]int? skip = null,
                [FromQuery]int? take = null,
                [FromQuery]int? totalRecords = null
                ) {

            if(select != null) {
                var pagedResult = Repo.GetFromDynamicLinq(
                    select, where, orderBy, skip, take, totalRecords);
                var json = JsonSerializer.Serialize(pagedResult);
                return new ContentResult { Content = json, ContentType = "application/json" };
            } else {
                var pagedResult = Repo.GetFromDynamicLinq(
                    where, orderBy, skip, take, totalRecords);
                var json = JsonSerializer.Serialize(pagedResult);
                return new ContentResult { Content = json, ContentType = "application/json" };
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
        [HttpGet("linq/async")]
        public async Task<IActionResult> GetDynamicLinqAsync(
                [FromQuery]string where = null,
                [FromQuery]string orderBy = null,
                [FromQuery]string select = null,
                [FromQuery]int? skip = null,
                [FromQuery]int? take = null,
                [FromQuery]int? totalRecords = null
                ) {
            if (select != null) {
                var pagedResult = await Repo.GetFromDynamicLinqAsync(
                    select, where, orderBy, skip, take, totalRecords);
                var json = JsonSerializer.Serialize(pagedResult);
                return new ContentResult { Content = json, ContentType = "application/json" };
            } else {
                var pagedResult = await Repo.GetFromDynamicLinqAsync(
                    where, orderBy, skip, take, totalRecords);
                var json = JsonSerializer.Serialize(pagedResult);
                return new ContentResult { Content = json, ContentType = "application/json" };
            }
        }







        /// <summary>
        /// Executes a stored procedure and returns the result as a json array.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("sp")]
        public IActionResult GetJsonArrayFromStoredProcedure([FromQuery] string spName) {

            var parms = GetParamsFromQuery();
            var result = Repo.Context.GetJsonArrayFromStoredProcedure(spName, parms, null);

            return Ok(result);
        }


        /// <summary>
        /// Asynchronously executes a stored procedure and returns the result as a json array.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("sp/async")]
        public async Task<IActionResult> GetJsonArrayFromStoredProcedureAsync([FromQuery] string spName) {

            var parms = GetParamsFromQuery();
            var result = await Repo.Context.GetJsonArrayFromStoredProcedureAsync(spName, parms, null);

            return Ok(result);
        }



        /// <summary>
        /// Executes a stored procedure and returns the result as a json object.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("sp/obj")]
        public IActionResult GetJsonObjectFromStoredProcedure([FromQuery] string spName) {

            var parms = GetParamsFromQuery();
            var result = Repo.Context.GetJsonObjectFromStoredProcedure(spName, parms, null);

            return Ok(result);
        }


        /// <summary>
        /// Asynchronously executes a stored procedure and returns the result as a json object.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("sp/obj/async")]
        public async Task<IActionResult> GetJsonObjectFromStoredProcedureAsync([FromQuery] string spName) {

            var parms = GetParamsFromQuery();
            var result = await Repo.Context.GetJsonObjectFromStoredProcedureAsync(spName, parms, null);

            return Ok(result);
        }



        /// <summary>
        /// Executes a stored procedure and returns the result as a json object.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("sp/scalar")]
        public IActionResult GetScalarFromStoredProcedure([FromQuery] string spName, [FromQuery] string returnType) {

            var parms = GetParamsFromQuery();

            if (returnType.Equals("int", StringComparison.OrdinalIgnoreCase) || returnType.Equals("integer", StringComparison.OrdinalIgnoreCase))
                return Ok(Repo.Context.GetScalarFromStoredProcedure<TContext, int>(spName, parms));
            else if (returnType.Equals("decimal", StringComparison.OrdinalIgnoreCase) || returnType.Equals("money", StringComparison.OrdinalIgnoreCase))
                return Ok(Repo.Context.GetScalarFromStoredProcedure<TContext, decimal>(spName, parms));
            else if (returnType.Equals("date", StringComparison.OrdinalIgnoreCase) || returnType.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                return Ok(Repo.Context.GetScalarFromStoredProcedure<TContext, DateTime>(spName, parms));
            else if (returnType.Equals("time", StringComparison.OrdinalIgnoreCase) || returnType.Equals("timespan", StringComparison.OrdinalIgnoreCase))
                return Ok(Repo.Context.GetScalarFromStoredProcedure<TContext, TimeSpan>(spName, parms));
            else if (returnType.Equals("bool", StringComparison.OrdinalIgnoreCase) || returnType.Equals("boolean", StringComparison.OrdinalIgnoreCase))
                return Ok(Repo.Context.GetScalarFromStoredProcedure<TContext, bool>(spName, parms));
            else
                return Ok(Repo.Context.GetScalarFromStoredProcedure<TContext, string>(spName, parms));

        }


        /// <summary>
        /// Asynchronously executes a stored procedure and returns the result as a json object.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("sp/scalar/async")]
        public async Task<IActionResult> GetScalarFromStoredProcedureAsync([FromQuery] string spName, [FromQuery] string returnType) {

            var parms = GetParamsFromQuery();

            if (returnType.Equals("int", StringComparison.OrdinalIgnoreCase) || returnType.Equals("integer", StringComparison.OrdinalIgnoreCase))
                return Ok(await Repo.Context.GetScalarFromStoredProcedureAsync<TContext, int>(spName, parms));
            else if (returnType.Equals("decimal", StringComparison.OrdinalIgnoreCase) || returnType.Equals("money", StringComparison.OrdinalIgnoreCase))
                return Ok(await Repo.Context.GetScalarFromStoredProcedureAsync<TContext, decimal>(spName, parms));
            else if (returnType.Equals("date", StringComparison.OrdinalIgnoreCase) || returnType.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                return Ok(await Repo.Context.GetScalarFromStoredProcedureAsync<TContext, DateTime>(spName, parms));
            else if (returnType.Equals("time", StringComparison.OrdinalIgnoreCase) || returnType.Equals("timespan", StringComparison.OrdinalIgnoreCase))
                return Ok(await Repo.Context.GetScalarFromStoredProcedureAsync<TContext, TimeSpan>(spName, parms));
            else if (returnType.Equals("bool", StringComparison.OrdinalIgnoreCase) || returnType.Equals("boolean", StringComparison.OrdinalIgnoreCase))
                return Ok(await Repo.Context.GetScalarFromStoredProcedureAsync<TContext, bool>(spName, parms));
            else
                return Ok(Repo.Context.GetScalarFromStoredProcedureAsync<TContext, string>(spName, parms));

        }



        /// <summary>
        /// Obtains a json result from a stored procedure
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("json")]
        public IActionResult GetJsonFromJsonStoredProcedure([FromQuery] string spName) {

            var parms = GetParamsFromQuery();
            var result = Repo.Context.GetJsonFromJsonStoredProcedure(spName, parms);

            return Ok(result);
        }


        /// <summary>
        /// Asynchronously obtains a json result from a stored procedure
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("json/async")]
        public async Task<IActionResult> GetJsonFromJsonStoredProcedureAsync([FromQuery] string spName) {

            var parms = GetParamsFromQuery();
            var result = await Repo.Context.GetJsonFromJsonStoredProcedureAsync(spName, parms);

            return Ok(result);
        }





        private Dictionary<string,object> GetParamsFromQuery() {
            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .ToDictionary(q => q.Key, q => (object)q.Value);
            return parms;
        }


    }
}
