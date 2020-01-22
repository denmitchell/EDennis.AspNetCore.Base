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
            var pagedList = Repo.GetFromDynamicLinq(
                where, orderBy, select, skip, take, totalRecords);
            var json = JsonSerializer.Serialize(pagedList);
            //var pePagedList = PartialEntity<TEntity>.CreatePagedResult(pagedList);
            //var json = JsonSerializer.Serialize(pePagedList, _jsonSerializerOptions);
            return new ContentResult { Content = json, ContentType = "application/json" };
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
            var pagedList = await Repo.GetFromDynamicLinqAsync(
                where, orderBy, select, skip, take, totalRecords);
            var json = JsonSerializer.Serialize(pagedList);
            //var json = Newtonsoft.Json.Linq.JToken.FromObject(pagedList).ToString();
            //var pePagedList = PartialEntity<TEntity>.CreatePagedResult(pagedList);
            //var json = JsonSerializer.Serialize(pePagedList, _jsonSerializerOptions);
            return new ContentResult { Content = json, ContentType = "application/json" };
        }


        /// <summary>
        /// Executes a stored procedure and returns the result.
        /// Note: there are no application-level constraints
        /// that limit repos to executing read-only 
        /// stored procedures.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("sp")]
        public IActionResult GetListFromStoredProcedure([FromQuery] string spName) {

            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));

            var result = Repo.Context.GetListFromStoredProcedure<TContext, TEntity>(
                spName, parms);

            return Ok(result);
        }


        /// <summary>
        /// Asynchronously executes a stored procedure and returns the result.
        /// Note: there are no application-level constraints
        /// that limit repos to executing read-only 
        /// stored procedures.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("sp/async")]
        public async Task<IActionResult> GetListFromStoredProcedureAsync([FromQuery] string spName) {
            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));

            var result = await Repo.Context.GetListFromStoredProcedureAsync<TContext, TEntity>(
                spName, parms);

            return Ok(result);
        }



        /// <summary>
        /// Executes a stored procedure and returns the result.
        /// Note: there are no application-level constraints
        /// that limit repos to executing read-only 
        /// stored procedures.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("sp/single")]
        public IActionResult GetSingleFromStoredProcedure([FromQuery] string spName) {

            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));

            var result = Repo.Context.GetSingleFromStoredProcedure<TContext, TEntity>(
                spName, parms);

            return Ok(result);
        }


        /// <summary>
        /// Asynchronously executes a stored procedure and returns the result.
        /// Note: there are no application-level constraints
        /// that limit repos to executing read-only 
        /// stored procedures.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("sp/single/async")]
        public async Task<IActionResult> GetSingleFromStoredProcedureAsync([FromQuery] string spName) {
            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));

            var result = await Repo.Context.GetSingleFromStoredProcedureAsync<TContext, TEntity>(
                spName, parms);

            return Ok(result);
        }


        /// <summary>
        /// Obtains a json result from a stored procedure
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("json")]
        public IActionResult GetListFromJsonStoredProcedure([FromQuery] string spName) {

            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));

            var result = Repo.Context.GetListFromJsonStoredProcedure<TContext,TEntity>(
                spName, parms);

            return Ok(result);
        }


        /// <summary>
        /// Asynchrously obtains a json result from a stored procedure
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("json/async")]
        public async Task<IActionResult> GetListFromJsonStoredProcedureAsync([FromQuery] string spName) {

            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));

            var result = await Repo.Context.GetListFromJsonStoredProcedureAsync<TContext, TEntity>(
                spName, parms);

            return Ok(result);
        }




        /// <summary>
        /// Obtains a json result from a stored procedure
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("json/single")]
        public IActionResult GetSingleFromJsonStoredProcedure([FromQuery] string spName) {

            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));

            var result = Repo.Context.GetSingleFromJsonStoredProcedure<TContext, TEntity>(
                spName, parms);

            return Ok(result);
        }


        /// <summary>
        /// Asynchrously obtains a json result from a stored procedure
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("json/single/async")]
        public async Task<IActionResult> GetSingleFromJsonStoredProcedureAsync([FromQuery] string spName) {

            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));

            var result = await Repo.Context.GetSingleFromJsonStoredProcedureAsync<TContext, TEntity>(
                spName, parms);

            return Ok(result);
        }


    }
}
