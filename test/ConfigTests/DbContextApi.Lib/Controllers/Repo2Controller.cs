using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.DbContextConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class Repo2Controller : ControllerBase {

        private readonly ILogger<Repo2Controller> _logger;
        private readonly PersonRepo2 _personRepo;
        private readonly PositionRepo2 _positionRepo;
        private readonly Dictionary<string, string> _repoProps
            = new Dictionary<string, string>();

        public Repo2Controller(PersonRepo2 personRepo, PositionRepo2 positionRepo, ILogger<Repo2Controller> logger) {
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
