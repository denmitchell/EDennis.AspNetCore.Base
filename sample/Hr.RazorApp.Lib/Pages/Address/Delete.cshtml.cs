using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Hr.Api.Models;

namespace Hr.RazorApp.AddressPages {
    public class DeleteModel : PageModel {
        private readonly HrApi _api;

        public DeleteModel(HrApi api) {
            _api = api;
        }

        [BindProperty]
        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            var result = await _api.GetAddressDetailsAsync(id.Value);

            if (result.StatusCode == 404)
                return NotFound();
            else
                Address = (Address)result.Value;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            await _api.DeleteAddressAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
