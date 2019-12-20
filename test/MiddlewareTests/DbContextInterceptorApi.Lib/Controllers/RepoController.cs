using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class RepoController : ControllerBase {

        private readonly ILogger<RepoController> _logger;
        private readonly PersonRepo _personRepo;
        private readonly PositionRepo _positionRepo;
        private readonly Dictionary<string, string> _repoProps
            = new Dictionary<string, string>();

        public RepoController(PersonRepo personRepo, PositionRepo positionRepo, ILogger<RepoController> logger) {
            _personRepo = personRepo;
            _positionRepo = positionRepo;
            _logger = logger;

            _repoProps.Add("PersonRepoScopedLoggerLevel", _personRepo.ScopedLogger.LogLevel.ToString());
            _repoProps.Add("PersonRepoScopePropertiesUser", _personRepo.ScopeProperties.User);
            _repoProps.Add("PersonCount", _personRepo.Query.Count().ToString());
            _repoProps.Add("PositionRepoScopedLoggerLevel", _personRepo.ScopedLogger.LogLevel.ToString());
            _repoProps.Add("PositionRepoScopePropertiesUser", _positionRepo.ScopeProperties.User);
            _repoProps.Add("PositionCount", _positionRepo.Query.Count().ToString());
        }

        [HttpGet]
        public Dictionary<string, string> Get() => _repoProps;
    }
}
