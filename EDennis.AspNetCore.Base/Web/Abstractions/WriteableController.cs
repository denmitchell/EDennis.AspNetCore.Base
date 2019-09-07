using DevExtreme.AspNet.Data;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace EDennis.AspNetCore.Base.Web.Abstractions
{
    [ApiController]
    [Route("api/[controller]")]
    public class WriteableController<TEntity, TContext> : ControllerBase
            where TEntity : class, IHasSysUser, IHasIntegerId, new()
            where TContext : DbContext {

        private readonly WriteableRepo<TEntity,TContext> _repo;

        public WriteableController(WriteableRepo<TEntity, TContext> repo) {
            _repo = repo;
        }

        /// <summary>
        /// Get from OData query string
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("odata")]
        public IEnumerable<TEntity> GetOData() {
            return _repo.Query;
        }


        /// <summary>
        /// Get from DevExtreme DataSourceLoader query string
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("devextreme")]
        public IActionResult GetDevExtreme(DataSourceLoadOptionsBase loadOptions) {
            return Ok(DataSourceLoader.Load<TEntity>(_repo.Query,loadOptions));
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
        [HttpGet("dynamic")]
        public IActionResult GetDynamicLinq(
                [FromBody]string where = null,
                [FromBody]string orderBy = null,
                [FromBody]string select = null,
                [FromBody]int? skip = null,
                [FromBody]int? take = null
                ) {
            IQueryable qry = _repo.Query;

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

            var result = qry.ToDynamicList();

            return new ObjectResult(result);
        }


        /// <summary>
        /// Get by primary key
        /// </summary>
        /// <param name="id">integer primary key</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            var rec = _repo.GetById(id);
            if (rec == null)
                return NotFound();
            else
                return new ObjectResult(rec);
        }

        [HttpPost]
        public IActionResult Post([FromBody]TEntity value) {
            var created = _repo.Create(value);
            return CreatedAtAction("Get", created.Id, created);
        }


        [HttpPut("{id}")]
        public IActionResult Put([FromBody]TEntity value, [FromRoute]int id) {
            if (id != value.Id)
                return new BadRequestObjectResult($"The provided object has an Id ({value.Id}) that differs from the route parameter ({id})");
            try {
                var updated = _repo.Update(value,id);
                return CreatedAtAction("Get", updated.Id, updated);
            } catch (MissingEntityException ex) {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id) {
            try {
                _repo.Delete(id);
                return NoContent();
            } catch (MissingEntityException ex) {
                return new BadRequestObjectResult(ex.Message);
            }
        }


    }
}
