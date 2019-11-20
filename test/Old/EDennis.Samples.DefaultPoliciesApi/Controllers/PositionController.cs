using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EDennis.Samples.DefaultPoliciesApi.Models;
using System.Net;

namespace EDennis.Samples.DefaultPoliciesApi.Controllers {

    [Route("api/[controller]")]//controller route
    [ApiController]
    public class PositionController : ControllerBase {

        private readonly AppDbContext _context;

        public PositionController(AppDbContext context) {
            _context = context;
        }

        // GET: api/Position
        [HttpGet]
        public async Task<IActionResult> Index() {
            return new ObjectResult(await _context.Position.ToListAsync());
        }


        [HttpGet("{id}")]
        // GET: api/Position/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var position = await _context.Position
                .FirstOrDefaultAsync(m => m.Id == id);
            if (position == null) {
                return NotFound();
            }

            return new ObjectResult(position);
        }


        // POST: api/Position
        [HttpPost]
        public async Task<IActionResult> Create(Position position) {
            if (ModelState.IsValid) {
                _context.Add(position);
                await _context.SaveChangesAsync();
                return new ObjectResult(position);
            }
            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }


        // Put: api/Position/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, Position position) {
            if (id != position.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(position);
                    await _context.SaveChangesAsync();
                } catch (Exception ex) {
                    if (!await ExistsInternal(position.Id)) {
                        return NotFound();
                    } else {
                        return new ObjectResult(ex) { StatusCode = (int)HttpStatusCode.BadRequest };
                    }
                }
                return new ObjectResult(position);
            }
            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }


        // DELETE: api/Position/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            try {
                var position = await _context.Position.FindAsync(id);
                _context.Position.Remove(position);
                await _context.SaveChangesAsync();
                return new StatusCodeResult((int)HttpStatusCode.NoContent);
            } catch (Exception ex) {
                return new ObjectResult(ex) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        // GET: api/Position/Exists/5
        [HttpGet("Exists/{id}")]
        public async Task<IActionResult> Exists(int id) {
            var exists = await ExistsInternal(id);
            return new StatusCodeResult(exists ? (int)HttpStatusCode.Found : (int)HttpStatusCode.NotFound)
            ;
        }


        private async Task<bool> ExistsInternal(int id) {
            return await _context.Position.AnyAsync(e => e.Id == id);
        }


    }
}
