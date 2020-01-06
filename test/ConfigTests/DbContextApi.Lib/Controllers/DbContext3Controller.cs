﻿using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.DbContextConfigsApi.Lib.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class DbContext3Controller : ControllerBase {

        private readonly ILogger<DbContext3Controller> _logger;
        private readonly DbContext3 _dbContext;
        private readonly Dictionary<string, string> _dbContextProps
            = new Dictionary<string, string>();

        public DbContext3Controller(DbContextProvider<DbContext3> provider, ILogger<DbContext3Controller> logger) {
            _dbContext = provider.Context;
            _dbContext.Database.EnsureCreated();
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
