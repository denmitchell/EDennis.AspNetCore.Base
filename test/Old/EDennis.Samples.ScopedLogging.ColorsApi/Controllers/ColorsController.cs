using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using EDennis.Samples.ScopedLogging.ColorsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EDennis.Samples.ScopedLogging.ColorsApi.Controllers {


    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : LoggableControllerBase<ColorsController> {

        private readonly ColorDbContext _context;
        public ColorsController(ColorDbContext context, 
            IEnumerable<ILogger<ColorsController>> loggers, 
            ScopeProperties22 scopeProperties) : base (loggers,scopeProperties) {
            _context = context;
        }

        // GET: api/Colors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> GetColors()
        {
            Logger.LogInformation("For {User}, {Path}", U, P);
            Logger.LogDebug("For {User}, {Controller}.{Action}", U, C, A);

            var colors = await _context.Color.ToListAsync();

            Logger.LogTrace("For {User}, {Controller}.{Action},\n\tReturning: {Return}", U, C, A, D(colors));

            return colors;
        }

        // GET: api/Colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetColor(int id)
        {
            Logger.LogInformation("For {User}, {Path}", U, P);
            Logger.LogDebug("For {User}, {Controller}.{Action}, id={id}", U, C, A, id);

            var color = await _context.Color.FindAsync(id);

            if (color == null)
            {
                return NotFound();
            }

            Logger.LogTrace("For {User}, {Controller}.{Action}, id={id},\n\tReturning: {Return}", U, C, A, id, D(color));

            return color;
        }

        // PUT: api/Colors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColor(int id, Color color)
        {
            Logger.LogInformation("For {User}, {Path}", U, P);
            Logger.LogDebug("For {User}, {Controller}.{Action}, id={id}, color={color}", U, C, A, id, D(color));

            if (id != color.Id)
            {
                Logger.LogWarning("For {User}, {Controller}.{Action}, id={id}, color={color}, warning={warning}", U, C, A, id, D(color), "ids don't match");
                return BadRequest();
            }

            _context.Entry(color).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ColorExists(id))
                {
                    Logger.LogWarning("For {User}, {Controller}.{Action}, id={id}, color={color}, warning={warning}", U, C, A, id, D(color), "id not found");
                    return NotFound();
                }
                else
                {
                    Logger.LogError(ex,"For {User}, {Controller}.{Action}, id={id}, color={color}, error={error}", U, C, A, id, D(color), ex.Message);
                    throw;
                }
            }

            Logger.LogTrace("For {User}, {Controller}.{Action}, id={id},\n\tReturning: {StatusCode}", U, C, A, id, HttpStatusCode.NoContent);

            return NoContent();
        }

        // POST: api/Colors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Color>> PostColor(Color color)
        {
            Logger.LogInformation("For {User}, {Path}", U, P);
            Logger.LogDebug("For {User}, {Controller}.{Action}, color={color}", U, C, A, D(color));

            _context.Color.Add(color);
            await _context.SaveChangesAsync();

            Logger.LogTrace("For {User}, {Controller}.{Action},\n\tReturning: {Return}", U, C, A, D(color));

            return CreatedAtAction("GetColor", new { id = color.Id }, color);
        }

        // DELETE: api/Colors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Color>> DeleteColor(int id)
        {
            Logger.LogInformation("For {User}, {Path}", U, P);
            Logger.LogDebug("For {User}, {Controller}.{Action}, id={id}", U, C, A, id);

            var color = await _context.Color.FindAsync(id);
            if (color == null)
            {
                Logger.LogWarning("For {User}, {Controller}.{Action}, id={id}, color={color}, warning={warning}", U, C, A, id, D(color), "id not found");
                return NotFound();
            }

            _context.Color.Remove(color);
            await _context.SaveChangesAsync();

            Logger.LogTrace("For {User}, {Controller}.{Action},\n\tReturning: {Return}", U, C, A, D(color));

            return color;
        }

        private bool ColorExists(int id)
        {
            Logger.LogInformation("For {User}, {Path}", U, P);
            Logger.LogDebug("For {User}, {Controller}.{Action}, id={id}", U, C, A, id);

            var result = _context.Color.Any(e => e.Id == id);

            Logger.LogTrace("For {User}, {Controller}.{Action},\n\tReturning: {Return}", U, C, A, result);
            return result;
        }
    }
}
