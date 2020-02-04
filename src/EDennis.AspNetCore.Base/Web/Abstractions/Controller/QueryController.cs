using DevExtreme.AspNet.Data;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    [ApiController]
    [Route("api/[controller]")]
    public abstract class QueryController<TEntity, TContext, TRepo> : ControllerBase
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : IQueryRepo<TEntity, TContext> {

        public TRepo Repo { get; set; }

        public JsonSerializerOptions JsonSerializationOptions { get; set; }

        public QueryController(TRepo repo) {
            Repo = repo;
            JsonSerializationOptions = new JsonSerializerOptions();
            JsonSerializationOptions.Converters.Add(new DynamicJsonConverter<TEntity>());
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
        public virtual IActionResult GetWithDynamicLinq(
                [FromQuery]string where = null,
                [FromQuery]string orderBy = null,
                [FromQuery]string select = null,
                [FromQuery]int? skip = null,
                [FromQuery]int? take = null,
                [FromQuery]int? totalRecords = null
                ) {

            try {
                if (select != null) {
                    var dynamicLinqResult = Repo.GetWithDynamicLinq(
                        select, where, orderBy, skip, take, totalRecords);
                    var json = JsonSerializer.Serialize(dynamicLinqResult);
                    return new ContentResult { Content = json, ContentType = "application/json" };
                } else {
                    var dynamicLinqResult = Repo.GetWithDynamicLinq(
                        where, orderBy, skip, take, totalRecords);
                    var json = JsonSerializer.Serialize(dynamicLinqResult);
                    return new ContentResult { Content = json, ContentType = "application/json" };
                }
            } catch (ArgumentException ex) {
                ModelState.AddModelError("", ex.Message);
                return new BadRequestObjectResult(ModelState);
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
        public virtual async Task<IActionResult> GetWithDynamicLinqAsync(
                [FromQuery]string where = null,
                [FromQuery]string orderBy = null,
                [FromQuery]string select = null,
                [FromQuery]int? skip = null,
                [FromQuery]int? take = null,
                [FromQuery]int? totalRecords = null
                ) {
            try {
                if (select != null) {
                    var dynamicLinqResult = await Repo.GetWithDynamicLinqAsync(
                        select, where, orderBy, skip, take, totalRecords);
                    var json = JsonSerializer.Serialize(dynamicLinqResult);
                    return new ContentResult { Content = json, ContentType = "application/json" };
                } else {
                    var dynamicLinqResult = await Repo.GetWithDynamicLinqAsync(
                        where, orderBy, skip, take, totalRecords);
                    var json = JsonSerializer.Serialize(dynamicLinqResult);
                    return new ContentResult { Content = json, ContentType = "application/json" };
                }
            } catch (ArgumentException ex) {
                ModelState.AddModelError("", ex.Message);
                return new BadRequestObjectResult(ModelState);
            }
        }



        /// <summary>
        /// Get from DevExtreme DataSourceLoader query string
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("devextreme")]
        public virtual IActionResult GetWithDevExtreme(
                [FromQuery]string select = null,
                [FromQuery]string sort = null,
                [FromQuery]string filter = null,
                [FromQuery]int? skip = null,
                [FromQuery]int? take = null,
                [FromQuery]string totalSummary = null,
                [FromQuery]string group = null,
                [FromQuery]string groupSummary = null
            ) {
            DataSourceLoadOptions loadOptions = null;
            try {
                loadOptions = DataSourceLoadOptionsBuilder.Build(
                    select, sort, filter, 
                    skip ?? 0, 
                    take ?? int.MaxValue, 
                    totalSummary,
                    group, groupSummary);
            } catch (ArgumentException ex) {
                ModelState.AddModelError("", ex.Message);
                return new BadRequestObjectResult(ModelState);
            }
            try {
                var result = DataSourceLoader.Load(Repo.Query, loadOptions);
                return Ok(result);
            } catch (ArgumentOutOfRangeException) {
                var obj =
                    new {
                        Exception = "Failed executing DevExtreme load expression",
                        ProvidedArguments = new {
                            select, sort, filter, skip, take, totalSummary, group, groupSummary
                        }
                    };
                return new BadRequestObjectResult(obj);
            }
        }


        /// <summary>
        /// Asynchronously get from DevExtreme DataSourceLoader query string
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("devextreme/async")]
        public virtual async Task<IActionResult> GetWithDevExtremeAsync(
                [FromQuery]string select = null,
                [FromQuery]string sort = null,
                [FromQuery]string filter = null,
                [FromQuery]int? skip = null,
                [FromQuery]int? take = null,
                [FromQuery]string totalSummary = null,
                [FromQuery]string group = null,
                [FromQuery]string groupSummary = null
            ) {
            var loadOptions = DataSourceLoadOptionsBuilder.Build(
                select, sort, filter, 
                skip ?? 0, 
                take ?? int.MaxValue, 
                totalSummary,
                group, groupSummary);

            return await Task.Run(() => GetWithDevExtreme(select, sort, filter, skip, take, totalSummary, group, groupSummary));
        }

    }
}
