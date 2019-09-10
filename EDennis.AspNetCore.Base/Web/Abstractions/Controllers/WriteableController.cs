using DevExtreme.AspNet.Data;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class WriteableController<TEntity, TContext> : ControllerBase
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
        [ODataQueryFilter]
        [HttpGet("odata")]
        public IEnumerable<TEntity> GetOData(
                [FromQuery]string select,
                [FromQuery]string orderBy,
                [FromQuery]string filter,
                [FromQuery]string expand,
                [FromQuery]int skip,
                [FromQuery]int top
            ) {
            return _repo.Query;
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

            var result = DataSourceLoader.Load(_repo.Query, loadOptions);
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
            return new ObjectResult(_repo.GetFromDynamicLinq(
                where,orderBy,select,skip,take));
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
            return new ObjectResult(await _repo.GetFromDynamicLinqAsync(
                where, orderBy, select, skip, take));
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

        /// <summary>
        /// Get by primary key
        /// </summary>
        /// <param name="id">integer primary key</param>
        /// <returns></returns>
        [HttpGet("{id}/async")]
        public async Task<IActionResult> GetAsync(int id) {
            var rec = await _repo.GetByIdAsync(id);
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

        [HttpPost("async")]
        public async Task<IActionResult> PostAsync([FromBody]TEntity value) {
            var created = await _repo.CreateAsync(value);
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

        [HttpPut("{id}/async")]
        public async Task<IActionResult> PutAsync([FromBody]TEntity value, [FromRoute]int id) {
            if (id != value.Id)
                return new BadRequestObjectResult($"The provided object has an Id ({value.Id}) that differs from the route parameter ({id})");
            try {
                var updated = await _repo.UpdateAsync(value, id);
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

        [HttpDelete("{id}/async")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int id) {
            try {
                await _repo.DeleteAsync(id);
                return NoContent();
            } catch (MissingEntityException ex) {
                return new BadRequestObjectResult(ex.Message);
            }
        }

    }
}
