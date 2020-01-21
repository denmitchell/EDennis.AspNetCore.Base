using Hr.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Hr.RazorApp.AddressPages {
    public class EditModel : PageModel {
        private readonly HrApi _api;

        public EditModel(HrApi api) {
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            var result = await _api.EditAddressAsync(Address,Address.Id);

            if (result.StatusCode > 399)
                return result;
            else
                Address = (Address)result.Value;

            return RedirectToPage("./Index");
        }

    }
}
