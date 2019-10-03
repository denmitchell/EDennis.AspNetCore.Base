using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EDennis.Samples.DefaultPoliciesMvc.Models;
using EDennis.Samples.DefaultPoliciesMvc.ApiClients;
using System.Net;

namespace EDennis.Samples.DefaultPoliciesMvc.Controllers
{
    public class PersonRemoteController : Controller
    {
        private readonly IDefaultPoliciesApiClient _api;

        public PersonRemoteController(IDefaultPoliciesApiClient api)
        {
            _api = api;
        }

        // GET: Person
        public IActionResult Index()
        {
            var persons = _api.GetPersons().Value;
            return View(persons);
        }

        // GET: Person/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = _api.GetPersonDetails(id);

            if (result.StatusCode == (int)HttpStatusCode.NotFound)
                return NotFound();
            else if (result.StatusCode >= 400)
                return new StatusCodeResult((int)result.StatusCode);

            var person = result.Value;
            return View(person);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] Person person)
        {
            if (ModelState.IsValid)
            {
                var result = _api.CreatePerson(person);

                if (result.StatusCode >= 400)
                    return new StatusCodeResult((int)result.StatusCode);

                return RedirectToAction(nameof(Index));
            }
            return View("Create",person);
        }


        // GET: Person/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _api.GetPersonDetails(id);

            if (result.StatusCode == (int)HttpStatusCode.NotFound)
                return NotFound();

            var person = result.Value;

            return View(person);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = _api.EditPerson(id, person);

                if (result.StatusCode >= 400)
                    return new StatusCodeResult((int)result.StatusCode);

                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Person/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _api.GetPersonDetails(id);

            if (result.StatusCode == (int)HttpStatusCode.NotFound)
                return NotFound();

            var person = result.Value;

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _api.DeletePerson(id);

            if (result.StatusCode >= 400)
                return result;

            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            var result = _api.PersonExists(id);
            return result.StatusCode == (int)HttpStatusCode.Found;
        }
    }
}
