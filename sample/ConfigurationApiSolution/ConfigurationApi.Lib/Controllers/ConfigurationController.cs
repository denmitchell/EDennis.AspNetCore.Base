using ConfigurationApi.Lib.Filters;
using ConfigurationApi.Lib.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationApi.Lib.Controllers {
    [ApiKeyAuth]
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase {
        
        readonly ConfigurationDbContext _context;

        public ConfigurationController(ConfigurationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string appId) {
            var configs = _context.ProjectSettings
                .Where(p => p.ProjectName == appId)
                .Select(p=> new { p.SettingKey, p.SettingValue });
            if (configs.Count() == 0)
                return NotFound();
            else
                return Ok(configs);
        } 

    }
}
