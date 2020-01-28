using DevExtreme.AspNet.Data;
using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core.Exceptions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    [ApiController]
    [Route("api/[controller]")]
    public abstract class RepoController<TEntity, TContext, TRepo> : ControllerBase
        where TEntity : class, IHasSysUser, new()
        where TContext : DbContext
        where TRepo : IRepo<TEntity, TContext> {

        public TRepo Repo { get; set; }

        public static PropertyInfo[] Properties;
        public static Func<string, object[]> ParseId;
        public static Func<dynamic, object[]> GetPrimaryKeyDynamic;
        public static Func<TEntity, object[]> GetPrimaryKey;
        public static IReadOnlyList<IProperty> KeyProperties;

        static RepoController() {

            Properties = typeof(TEntity).GetProperties();

            ParseId = (s) => {
                var key = s.Split('~');
                var id = new object[KeyProperties.Count];
                try {
                    for (int i = 0; i < id.Length; i++)
                        id[i] = Convert.ChangeType(key[i], KeyProperties[i].ClrType);
                } catch {
                    throw new ArgumentException($"The provided path parameters ({key}) cannot be converted into a key for {typeof(TEntity).Name}");
                }
                return id;
            };

            GetPrimaryKeyDynamic = (dyn) => {
                var id = new object[KeyProperties.Count];
                Type dynType = dyn.GetType();
                PropertyInfo[] props = dynType.GetProperties();
                for (int i = 0; i < KeyProperties.Count; i++) {
                    var keyName = KeyProperties[i].Name;
                    var keyType = KeyProperties[i].ClrType;
                    var prop = props.FirstOrDefault(p => p.Name == keyName);
                    if (prop == null)
                        throw new ArgumentException($"The provided input does not contain a {keyName} property");
                    var keyValue = prop.GetValue(dyn);
                    id[i] = Convert.ChangeType(keyValue, keyType);
                }
                return id;
            };

            GetPrimaryKey = (entity) => {
                var id = new object[KeyProperties.Count];
                for (int i = 0; i < KeyProperties.Count; i++) {
                    var keyName = KeyProperties[i].Name;
                    var keyType = KeyProperties[i].ClrType;
                    var prop = Properties.FirstOrDefault(p => p.Name == keyName);
                    var keyValue = prop.GetValue(entity);
                    id[i] = Convert.ChangeType(keyValue, keyType);
                }
                return id;
            };


        }


        const string IDREGEX = "{id:regex(((?:[[^~]]+~[[^~]]+(?:~[[^~]]+)*)|(?:^-?[[0-9]]+$)))}";
        const string ASYNC_IDREGEX = "async/{id:regex(((?:[[^~]]+~[[^~]]+(?:~[[^~]]+)*)|(?:^-?[[0-9]]+$)))}";

        public JsonSerializerOptions JsonSerializationOptions { get; set; }

        public RepoController(TRepo repo) {
            Repo = repo;
            if (KeyProperties == null)
                KeyProperties = Repo.Context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties;

            JsonSerializationOptions = new JsonSerializerOptions();
            JsonSerializationOptions.Converters.Add(new DynamicJsonConverter<TEntity>());
        }


        [HttpGet(IDREGEX)]
        public virtual IActionResult Get([FromRoute]string id) {
            object[] iPk;
            try {
                iPk = ParseId(id);
            } catch (Exception ex) {
                return BadRequest(ex);
            }

            var entity = Repo.GetWithId(iPk);
            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }


        [HttpGet(ASYNC_IDREGEX)]
        public virtual async Task<IActionResult> GetAsync([FromRoute]string id) {
            object[] iPk;
            try {
                iPk = ParseId(id);
            } catch (Exception ex) {
                return BadRequest(ex);
            }
            var entity = await Repo.GetWithIdAsync(iPk);
            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }

        [HttpPost]
        public virtual IActionResult Post([FromBody]TEntity entity) {
            var pk = GetPrimaryKey(entity);
            try {
                var created = Repo.Create(entity);
                return Ok(created);
            } catch (DbUpdateException) {
                if (Repo.Exists(pk)) {
                    ModelState.AddModelError("", $"A {typeof(TEntity).Name} instance with the specified id {pk.ToTildaDelimited()} already exists");
                    return Conflict(ModelState);
                } else {
                    throw;
                }
            }
            //return CreatedAtAction("GetById", new { id = pk.ToTildaDelimited() }, entity);
        }

        [HttpPost("async")]
        public virtual async Task<IActionResult> PostAsync([FromBody] TEntity entity) {
            var pk = GetPrimaryKey(entity);
            try {
                var created = await Repo.CreateAsync(entity);
                return Ok(created);
            } catch (DbUpdateException) {
                if (Repo.Exists(pk)) {
                    ModelState.AddModelError("", $"A {typeof(TEntity).Name} instance with the specified id {pk.ToTildaDelimited()} already exists");
                    return Conflict(ModelState);
                } else {
                    throw;
                }
            }
            //return CreatedAtAction("GetById", new { id = pk.ToTildaDelimited() }, entity);
        }


        [HttpPut(IDREGEX)]
        public virtual IActionResult Put([FromBody]TEntity entity, [FromRoute]string id) {

            object[] ePk;
            object[] iPk;
            try {
                ePk = GetPrimaryKey(entity);
                iPk = ParseId(id);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }

            if (!ePk.EqualsAll(iPk))
                return BadRequest($"The path parameter id ({id}) does not match the provided object's id ({ePk.ToTildaDelimited()})");

            try {
                var updated = Repo.Update(entity, iPk);
                return Ok(updated);
            } catch (DbUpdateConcurrencyException) {
                if (!Repo.Exists(iPk))
                    return NotFound();
                else
                    throw;
            }
        }


        [HttpPut(ASYNC_IDREGEX)]
        public virtual async Task<IActionResult> PutAsync([FromBody]TEntity entity, [FromRoute]string id) {
            object[] ePk;
            object[] iPk;
            try {
                ePk = GetPrimaryKey(entity);
                iPk = ParseId(id);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }

            if (!ePk.EqualsAll(iPk))
                return BadRequest($"The path parameter id ({id}) does not match the provided object's id ({ePk.ToTildaDelimited()})");

            try {
                var updated = await Repo.UpdateAsync(entity, iPk);
                return Ok(updated);
            } catch (DbUpdateConcurrencyException) {
                if (!Repo.Exists(iPk))
                    return NotFound();
                else
                    throw;
            }
        }


        [HttpPatch(IDREGEX)]
        public virtual IActionResult Patch([FromRoute]string id) {

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

            object[] ePk;
            object[] iPk;
            try {
                ePk = GetPrimaryKeyDynamic(partialEntity);
                iPk = ParseId(id);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }

            if (!ObjectArrayExtensions.EqualsAll(ePk, iPk))
                return BadRequest($"The path parameter id ({id}) does not match the provided object's id ({ObjectArrayExtensions.ToTildaDelimited(ePk)})");

            try {
                var updated = Repo.Update(partialEntity, iPk);
                return Ok(updated);
            } catch (DbUpdateConcurrencyException) {
                if (!Repo.Exists(iPk))
                    return NotFound();
                else
                    throw;
            }
            }


        [HttpPatch(ASYNC_IDREGEX)]
        public virtual async Task<IActionResult> PatchAsync([FromRoute]string id) {

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

            object[] ePk;
            object[] iPk;
            try {
                ePk = GetPrimaryKeyDynamic(partialEntity);
                iPk = ParseId(id);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }

            if (!ObjectArrayExtensions.EqualsAll(ePk, iPk))
                return BadRequest($"The path parameter id ({id}) does not match the provided object's id ({ObjectArrayExtensions.ToTildaDelimited(ePk)})");

            try {
                var updated = await Repo.UpdateAsync(partialEntity, iPk);
                return Ok(updated);
            } catch (DbUpdateConcurrencyException) {
                if (!Repo.Exists(iPk))
                    return NotFound();
                else
                    throw;
            } catch (Exception ex) {
                throw;
            }
        }


        [HttpDelete(IDREGEX)]
        public virtual IActionResult Delete(string id) {
            object[] iPk;
            try {
                iPk = ParseId(id);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }

            try {
                Repo.Delete(iPk);
            } catch (MissingEntityException) {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete(ASYNC_IDREGEX)]
        public async virtual Task<IActionResult> DeleteAsync(string id) {
            object[] iPk;
            try {
                iPk = ParseId(id);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            try {
                await Repo.DeleteAsync(iPk);
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
