using EDennis.Samples.DefaultPoliciesApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.DefaultPoliciesApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly PositionRepo _repo;

        public PositionController(PositionRepo repo) {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<Position> GetAll()
        {
            return _repo;
        }

        [HttpGet("{id}")]
        public Position Get(int id)
        {
            return _repo.Where(p=>p.Id==id).FirstOrDefault();
        }

        [HttpPost]
        public void Post([FromBody] Position position)
        {
            _repo.Add(position);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Position position)
        {
            for(int i = 0; i < _repo.Count; i++) {
                if (_repo[i].Id == id) {
                    _repo[i] = position;
                    return;
                }
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var index = -1;
            for (int i = 0; i < _repo.Count; i++) {
                if (_repo[i].Id == id) {
                    index = i;
                    break;
                }
            }
            _repo.RemoveAt(index);
        }
    }
}
