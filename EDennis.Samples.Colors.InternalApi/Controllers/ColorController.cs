using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.Samples.Colors.InternalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.Colors.InternalApi.Controllers {
    [Route("iapi/Color")]
    [ApiController]
    public class ColorController : ControllerBase {

        public ColorRepo _repo;
        ILogger _logger;

        public ColorController(ColorRepo repo, ILogger<ColorRepo> logger) {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Color>> GetAll() {
            return _repo.Query.OrderBy(x=>x.Name).ToPagedList();
        }

        [HttpGet("{id}")]
        public ActionResult<Color> Get(int id) {
            var color = _repo.GetById(id);
            if (color == null)
                return NotFound();
            else
                return color;
        }

        [HttpPost]
        public ActionResult Post(Color color) {
            _repo.Create(color);
            return NoContent();
        }


        [HttpPut("{id}")]
        public ActionResult Put(Color color, int id) {
            if (color.Id != id)
                return BadRequest($"Id of Color ({color.Id}) not equal to route parameter ({id})");
            try {
                _repo.Update(color, id);
            } catch (MissingEntityException) {
                return NotFound();
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id) {
            try {
                _repo.Delete(id);
            } catch (MissingEntityException) {
                return NotFound();
            }
            return NoContent();
        }


    }
}