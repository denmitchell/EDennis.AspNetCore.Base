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
    public class PersonController : ControllerBase {

        private readonly AppDbContext _context;

        public PersonController(AppDbContext context) {
            _context = context;
        }

        // GET: api/Person
        [HttpGet]
        public async Task<IActionResult> Index() {
            return new ObjectResult(await _context.Person.ToListAsync());
        }


        [HttpGet("{id}")]
        // GET: api/Person/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null) {
                return NotFound();
            }

            return new ObjectResult(person);
        }


        // POST: api/Person
        [HttpPost]
        public async Task<IActionResult> Create(Person person) {
            if (ModelState.IsValid) {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return new ObjectResult(person);
            }
            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }


        // Put: api/Person/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, Person person) {
            if (id != person.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                } catch (Exception ex) {
                    if (!await ExistsInternal(person.Id)) {
                        return NotFound();
                    } else {
                        return new ObjectResult(ex) { StatusCode = (int)HttpStatusCode.BadRequest };
                    }
                }
                return new ObjectResult(person);
            }
            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }


        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            try {
                var person = await _context.Person.FindAsync(id);
                _context.Person.Remove(person);
                await _context.SaveChangesAsync();
                return new StatusCodeResult((int)HttpStatusCode.NoContent);
            } catch (Exception ex) {
                return new ObjectResult(ex) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        // GET: api/Person/Exists/5
        [HttpGet("Exists/{id}")]
        public async Task<IActionResult> Exists(int id) {
            var exists = await ExistsInternal(id);
            return new StatusCodeResult(exists ? (int)HttpStatusCode.Found : (int)HttpStatusCode.NotFound)
            ;
        }


        private async Task<bool> ExistsInternal(int id) {
            return await _context.Person.AnyAsync(e => e.Id == id);
        }


    }
}
