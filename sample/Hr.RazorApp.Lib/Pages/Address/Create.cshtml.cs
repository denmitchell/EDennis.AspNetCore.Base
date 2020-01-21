using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hr.Api.Models;

namespace Hr.RazorApp.AddressPages {
    public class CreateModel : PageModel {
        private readonly HrApi _api;

        public CreateModel(HrApi api) {
            _api = api;
        }

        public IActionResult OnGet() {
            return Page();
        }


        [BindProperty]
        public Address Address { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid)
                return Page();

            await _api.CreateAddressAsync(Address);
            return RedirectToPage("./Index");
        }
    }
}
