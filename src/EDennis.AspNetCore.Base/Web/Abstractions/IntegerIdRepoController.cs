using DevExtreme.AspNet.Data;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    [ApiController]
    [Route("api/[controller]")]
    public abstract class IntegerIdRepoController<TEntity, TContext, TRepo> : ControllerBase
        where TEntity : class, IHasSysUser, IHasIntegerId, new()
        where TContext : DbContext
        where TRepo : IRepo<TEntity, TContext> {

        public TRepo Repo { get; set; }

        public static Func<dynamic, int> GetPrimaryKeyDynamic;

        static IntegerIdRepoController() {

            GetPrimaryKeyDynamic = (dyn) => {
                try {
                    return dyn.Id;
                } catch {
                    throw new ArgumentException($"The provided input does not contain an Id property");
                }
            };
        }


        public JsonSerializerOptions JsonSerializationOptions { get; set; }

        public IntegerIdRepoController(TRepo repo) {
            Repo = repo;
            JsonSerializationOptions = new JsonSerializerOptions();
            JsonSerializationOptions.Converters.Add(new DynamicJsonConverter<TEntity>());
        }


        [HttpGet("{id}")]
        public virtual IActionResult GetById([FromRoute] int id) {
            var entity = Repo.Get(id);
            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }


        [HttpGet("async/{id}")]
        public virtual async Task<IActionResult> GetByIdAsync([FromRoute] int id) {
            var entity = await Repo.GetAsync(id);
            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }

        [HttpPost]
        public virtual IActionResult Create([FromBody]TEntity entity) {
            try {
                var created = Repo.Create(entity);
                return Ok(created);
            } catch (DbUpdateException) {
                if (Repo.Exists(entity.Id)) {
                    ModelState.AddModelError("", $"A {typeof(TEntity).Name} instance with the specified id {entity.Id} already exists");
                    return Conflict(ModelState);
                } else {
                    throw;
                }
            } catch (Exception) {
                throw;
            }
            //return CreatedAtAction("GetById", new { id = entity.Id }, entity);
        }

        [HttpPost("async")]
        public virtual async Task<IActionResult> CreateAsync([FromBody]TEntity entity) {
            try {
                var created = await Repo.CreateAsync(entity);
                return Ok(created);
            } catch (DbUpdateException) {
                if (Repo.Exists(entity.Id)) {
                    ModelState.AddModelError("", $"A {typeof(TEntity).Name} instance with the specified id {entity.Id} already exists");
                    return Conflict(ModelState);
                } else {
                    throw;
                }
            } catch (Exception) {
                throw;
            }
            //return CreatedAtAction("GetById", new { id = entity.Id }, entity);
        }


        [HttpPut("{id}")]
        public virtual IActionResult Update([FromBody]TEntity entity, [FromRoute]int id) {

            if (entity.Id != id)
                return BadRequest($"The path parameter id ({id}) does not match the provided object's id ({entity.Id})");

            try {
                var updated = Repo.Update(entity, id);
                return Ok(updated);
            } catch (DbUpdateConcurrencyException) {
                if (!Repo.Exists(id))
                    return NotFound();
                else
                    throw;
            } catch (Exception) {
                throw;
            }
            //return NoContent();
        }


        [HttpPut("async/{id}")]
        public virtual async Task<IActionResult> UpdateAsync([FromBody]TEntity entity, [FromRoute]int id) {

            if (entity.Id != id)
                return BadRequest($"The path parameter id ({id}) does not match the provided object's id ({entity.Id})");

            try {
                var updated = await Repo.UpdateAsync(entity, id);
                return Ok(updated);
            } catch (DbUpdateConcurrencyException) {
                if (!Repo.Exists(id))
                    return NotFound();
                else
                    throw;
            } catch (Exception) {
                throw;
            }
            //return NoContent();
        }


        [HttpPatch("{id}")]
        public virtual IActionResult Patch([FromRoute]int id) {

            string json;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8)) {
                json = reader.ReadToEndAsync().Result;
            }

            dynamic partialEntity;
            try {
                partialEntity = JsonSerializer.Deserialize<dynamic>(json, JsonSerializationOptions);
            } catch {
                return BadRequest($"The provided json ({json}) could not be deserialized into a partial ({typeof(TEntity).Name} object)");
            }

            var dId = GetPrimaryKeyDynamic(partialEntity);

            if (dId != id)
                return BadRequest($"The path parameter id ({id}) does not match the provided object's id ({dId})");

            try {
                var updated = Repo.Update(partialEntity, id);
                return Ok(updated);
            } catch (DbUpdateConcurrencyException) {
                if (!Repo.Exists(id))
                    return NotFound();
                else
                    throw;
            } catch (Exception) {
                throw;
            }
            //return NoContent();
        }


        [HttpPatch("async/{id}")]
        public virtual async Task<IActionResult> PatchAsync([FromRoute] int id) {

            string json;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8)) {
                json = reader.ReadToEndAsync().Result;
            }

            dynamic partialEntity;
            try {
                partialEntity = JsonSerializer.Deserialize<dynamic>(json, JsonSerializationOptions);
            } catch {
                return BadRequest($"The provided json ({json}) could not be deserialized into a partial ({typeof(TEntity).Name} object)");
            }
            var dId = GetPrimaryKeyDynamic(partialEntity);

            if (dId != id)
                return BadRequest($"The path parameter id ({id}) does not match the provided object's id ({dId})");

            try {
                var updated = await Repo.Update(partialEntity, id);
                return Ok(updated);
            } catch (DbUpdateConcurrencyException) {
                if (!Repo.Exists(id))
                    return NotFound();
                else
                    throw;
            } catch (Exception) {
                throw;
            }
            //return NoContent();
        }

        [HttpDelete("{id}")]
        public virtual IActionResult Delete([FromRoute]int id) {
            try {
                Repo.Delete(id);
            } catch (MissingEntityException) {
                return NotFound();
            }
            return NoContent();
        }


        [HttpDelete("async/{id}")]
        public async virtual Task<IActionResult> DeleteAsync([FromRoute]int id) {
            try {
                await Repo.DeleteAsync(id);
            } catch (MissingEntityException) {
                return NotFound();
            }
            return NoContent();
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
            } catch (ParseException ex) {
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
            } catch (ParseException ex) {
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
                [FromQuery]string select,
                [FromQuery]string sort,
                [FromQuery]string filter,
                [FromQuery]int skip,
                [FromQuery]int take,
                [FromQuery]string totalSummary,
                [FromQuery]string group,
                [FromQuery]string groupSummary
            ) {
            DataSourceLoadOptions loadOptions = null;
            try {
                loadOptions = DataSourceLoadOptionsBuilder.Build(
                    select, sort, filter, skip, take, totalSummary,
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

            return await Task.Run(() => GetWithDevExtreme(select, sort, filter, skip, take, totalSummary, group, groupSummary));
        }


    }
}
