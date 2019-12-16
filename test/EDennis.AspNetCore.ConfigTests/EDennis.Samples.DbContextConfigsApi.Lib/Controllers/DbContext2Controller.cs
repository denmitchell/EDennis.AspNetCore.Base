using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.DbContextConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class DbContext2Controller : ControllerBase {

        private readonly ILogger<DbContext2Controller> _logger;
        private readonly DbContext2 _dbContext;
        private readonly Dictionary<string, string> _dbContextProps
            = new Dictionary<string, string>();

        public DbContext2Controller(DbContext2 dbContext, ILogger<DbContext2Controller> logger) {
            _dbContext = dbContext;
            _logger = logger;

            _dbContextProps.Add("DatabaseProviderName", _dbContext.Database.ProviderName);
            _dbContextProps.Add("EntityTypes", string.Join(',', _dbContext.Model.GetEntityTypes().ToArray().Select(x => x.Name)));
            _dbContextProps.Add("PersonCount", _dbContext.Person.Count().ToString());
            _dbContextProps.Add("PositionCount", _dbContext.Position.Count().ToString());
        }

        [HttpGet]
        public Dictionary<string, string> Get() => _dbContextProps;
    }
}
