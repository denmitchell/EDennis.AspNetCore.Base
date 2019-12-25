using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class RepoController : ControllerBase {

        private readonly ILogger<RepoController> _logger;
        private readonly PositionRepo _repo;

        public RepoController(PositionRepo repo, ILogger<RepoController> logger) {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public List<Position> Get() => _repo.Context.Position.ToList();

        [HttpGet("{id}")]
        public Position Get(int id) => _repo.GetById(id);

        [HttpDelete("{id}")]
        public void Delete(int id) => _repo.Delete(id);

        [HttpPut("{id}")]
        public void Update(Position person, int id) => _repo.Update(person,id);

        [HttpPost]
        public void Create(Position person) => _repo.Create(person);


    }
}
