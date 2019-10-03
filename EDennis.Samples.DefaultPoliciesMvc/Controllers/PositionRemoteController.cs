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
    public class PositionRemoteController : Controller
    {
        private readonly IDefaultPoliciesApiClient _api;

        public PositionRemoteController(IDefaultPoliciesApiClient api)
        {
            _api = api;
        }

        // GET: Position
        public IActionResult Index()
        {
            var positions = _api.GetPositions().Value;
            return View(positions);
        }

        // GET: Position/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = _api.GetPositionDetails(id);

            if (result.StatusCode == (int)HttpStatusCode.NotFound)
                return NotFound();
            else if (result.StatusCode >= 400)
                return new StatusCodeResult((int)result.StatusCode);

            var position = result.Value;
            return View(position);
        }

        // GET: Position/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Position/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] Position position)
        {
            if (ModelState.IsValid)
            {
                var result = _api.CreatePosition(position);

                if (result.StatusCode >= 400)
                    return new StatusCodeResult((int)result.StatusCode);

                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }


        // GET: Position/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _api.GetPositionDetails(id);

            if (result.StatusCode == (int)HttpStatusCode.NotFound)
                return NotFound();

            var position = result.Value;

            return View(position);
        }

        // POST: Position/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] Position position)
        {
            if (id != position.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = _api.EditPosition(id, position);

                if (result.StatusCode >= 400)
                    return new StatusCodeResult((int)result.StatusCode);

                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        // GET: Position/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _api.GetPositionDetails(id);

            if (result.StatusCode == (int)HttpStatusCode.NotFound)
                return NotFound();

            var position = result.Value;

            return View(position);
        }

        // POST: Position/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _api.DeletePosition(id);

            if (result.StatusCode >= 400)
                return result;

            return RedirectToAction(nameof(Index));
        }

        private bool PositionExists(int id)
        {
            var result = _api.PositionExists(id);
            return result.StatusCode == (int)HttpStatusCode.Found;
        }
    }
}
