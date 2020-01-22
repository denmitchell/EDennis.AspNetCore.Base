using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    public abstract class SqlServerWriteableController<TEntity, TContext> : SqlServerReadonlyController<TEntity, TContext>
            where TEntity : class, IHasIntegerId, IHasSysUser, new()
            where TContext : DbContext, ISqlServerDbContext<TContext> {

        public SqlServerWriteableController(Repo<TEntity, TContext> repo, 
            ILogger<SqlServerReadonlyController<TEntity, TContext>> logger)
            : base(repo, logger) { }


        /// <summary>
        /// Get by primary key
        /// </summary>
        /// <param name="id">integer primary key</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            var rec = Repo.GetById(id);
            if (rec == null)
                return NotFound();
            else
                return new ObjectResult(rec);
        }

        /// <summary>
        /// Get by primary key
        /// </summary>
        /// <param name="id">integer primary key</param>
        /// <returns></returns>
        [HttpGet("async/{id}")]
        public async Task<IActionResult> GetAsync(int id) {
            var rec = await Repo.GetByIdAsync(id);
            if (rec == null)
                return NotFound();
            else
                return new ObjectResult(rec);
        }

        /// <summary>
        /// Creates a new object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult Post([FromBody]TEntity value) {
            var created = Repo.Create(value);
            return Ok(created);
        }

        /// <summary>
        /// Creates a new object asynchronously
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("async")]
        public virtual async Task<IActionResult> PostAsync([FromBody]TEntity value) {
            var created = await Repo.CreateAsync(value);
            return Ok(created);
        }


        /// <summary>
        /// Updates an object.  This overload takes a partial object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put([FromBody]TEntity value, [FromRoute]int id) {

            try {
                if (value.Id != id)
                    return new BadRequestObjectResult($"The provided object has an Id ({value.Id}) that differs from the route parameter ({id})");
            } catch (RuntimeBinderException) {
                return new BadRequestObjectResult($"The provided object does not have an Id Property");
            }

            try {
                var updated = Repo.Update(value, id);
                return Ok(updated);// CreatedAtAction("Get", updated.Id, updated);
            } catch (RuntimeBinderException) {
                return new BadRequestObjectResult($"The provided object cannot be serialized into a {typeof(TEntity).Name}.");
            } catch (Exception ex) {
                return new BadRequestObjectResult(ex.Message);
            }

        }

        /// <summary>
        /// Patch version of Put with partial entity
        /// </summary>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public IActionResult Patch([FromRoute]int id) {
            string json;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8)) {
                json = reader.ReadToEndAsync().Result;
            }            
            var partialEntity = JsonSerializer.Deserialize<dynamic>(json, JsonSerializationOptions);
            var updated = Repo.Update(partialEntity, id);
            return Ok(updated);
        }



        /// <summary>
        /// Asynchronously updates an object.  This overload takes a partial object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("async/{id}")]
        public async Task<IActionResult> PutAsync([FromBody]TEntity value, [FromRoute]int id) {
            try {
                if (id != value.Id)
                    return new BadRequestObjectResult($"The provided object has an Id ({value.Id}) that differs from the route parameter ({id})");
            } catch (RuntimeBinderException) {
                return new BadRequestObjectResult($"The provided object does not have an Id Property");
            }

            try {
                var updated = await Repo.UpdateAsync(value, id);
                return Ok(updated);
            } catch (RuntimeBinderException) {
                return new BadRequestObjectResult($"The provided object cannot be serialized into a {typeof(TEntity).Name}.");
            } catch (Exception ex) {
                return new BadRequestObjectResult(ex.Message);
            }

        }

        /// <summary>
        /// Patch version of PutAsync with partial entity
        /// </summary>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("async/{id}")]
        public async Task<IActionResult> PatchAsync([FromRoute]int id) {
            string json;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8)) {
                json = await reader.ReadToEndAsync();
            }
            var partialEntity = JsonSerializer.Deserialize<dynamic>(json, JsonSerializationOptions);
            var updated = await Repo.UpdateAsync(partialEntity, id);
            return Ok(updated);
        }



        /// <summary>
        /// Deletes the object identified by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id) {
            try {
                Repo.Delete(id);
                return NoContent();
            } catch (MissingEntityException ex) {
                return new BadRequestObjectResult(ex.Message);
            }
        }


        /// <summary>
        /// Asynchronously deletes the object identified by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("async/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int id) {
            try {
                await Repo.DeleteAsync(id);
                return NoContent();
            } catch (MissingEntityException ex) {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Helper method that can be used (instead of exception handling)
        /// to ensure that a dynamic object has an Id property.  Because
        /// this method uses reflection, there will be a performance hit
        /// on every update request that uses it. 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        protected static bool PropertyExists(dynamic obj, string property) {
            return ((Type)obj.GetType()).GetProperties().Where(p => p.Name.Equals(property)).Any();
        }

    }
}