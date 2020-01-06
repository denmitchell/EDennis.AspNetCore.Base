using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.DbContextConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class DbContext1Controller : ControllerBase {

        private readonly ILogger<DbContext1Controller> _logger;
        private readonly DbContext1 _dbContext;
        private readonly Dictionary<string, string> _dbContextProps
            = new Dictionary<string, string>();

        public DbContext1Controller(DbContextProvider<DbContext1> provider, ILogger<DbContext1Controller> logger) {
            _dbContext = provider.Context;
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
