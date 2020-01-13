using DevExtreme.AspNet.Data;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Serialization;
using EDennis.NetCoreTestingUtilities.Extensions;
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

namespace EDennis.AspNetCore.Base.Web {

    [ApiController]
    [Route("api/[controller]")]
    public abstract class SqlServerReadonlyController<TEntity, TContext> : ControllerBase
            where TEntity : class, IHasSysUser, new()
            where TContext : DbContext, ISqlServerDbContext<TContext> {

        public Repo<TEntity, TContext> Repo { get; }
        public ILogger<SqlServerReadonlyController<TEntity, TContext>> Logger { get; }

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public SqlServerReadonlyController(Repo<TEntity, TContext> repo,
            ILogger<SqlServerReadonlyController<TEntity, TContext>> logger) {
            Repo = repo;
            Logger = logger;

            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.Converters.Add(new PartialEntityConverter());
        }

        public abstract Dictionary<string, Type> StoredProcedureReturnTypes { get; } 



        /// <summary>
        /// Get from OData query string
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
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
                [FromQuery]int? take = null
                ) {
            var dList = Repo.GetFromDynamicLinq(
                where, orderBy, select, skip, take);
            var pe = PartialEntity<TEntity>.CreateList(dList);
            var json = JsonSerializer.Serialize(pe, _jsonSerializerOptions);
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
                [FromQuery]int? take = null
                ) {
            var dList = await Repo.GetFromDynamicLinqAsync(
                where, orderBy, select, skip, take);
            var pe = PartialEntity<TEntity>.CreateList(dList);
            var json = JsonSerializer.Serialize(pe, _jsonSerializerOptions);
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
        public IActionResult GetFromStoredProcedure([FromQuery] string spName) {

            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));

            MethodInfo method = typeof(ISqlServerDbContext<TContext>)
                .GetMethod("GetFromStoredProcedure",BindingFlags.Static);

            Type returnType = typeof(object);
            if (StoredProcedureReturnTypes.TryGetValue(spName, out Type retType))
                returnType = retType;
            MethodInfo generic = method.MakeGenericMethod(returnType);

            var result = generic.Invoke(null, new object[] { spName, parms });

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
        public async Task<IActionResult> GetFromStoredProcedureAsync([FromQuery] string spName) {

            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));


            return Ok(await Repo.Context.GetFromStoredProcedureAsync(
                spName, parms));
        }


        /// <summary>
        /// Obtains a json result from a stored procedure
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("json")]
        public ActionResult<string> GetJsonColumnFromStoredProcedure([FromQuery] string spName) {

            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));

            var json = Repo.Context.GetJsonColumnFromStoredProcedure(
                spName, parms);


            return Content(json, "application/json");

        }

        /// <summary>
        /// Asynchrously obtains a json result from a stored procedure
        /// </summary>
        /// <param name="spName">The name of the stored procedure to execute</param>
        /// <returns></returns>
        [HttpGet("json/async")]
        public async Task<IActionResult> GetJsonColumnFromStoredProcedureAsync([FromQuery] string spName) {

            var parms = HttpContext.Request.Query
                .Where(q => q.Key != "spName")
                .Select(q => new KeyValuePair<string, string>(q.Key, q.Value[0]));


            var json = await Repo.Context.GetJsonColumnFromStoredProcedureAsync(
                spName, parms);


            return Content(json, "application/json");
        }


    }
}
