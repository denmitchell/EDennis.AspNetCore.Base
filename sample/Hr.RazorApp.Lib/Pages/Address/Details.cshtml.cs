using Hr.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Hr.RazorApp.AddressPages {
    public class DetailsModel : PageModel {
        private readonly HrApi _api;

        public DetailsModel(HrApi api) {
            _api = api;
        }

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
    }
}
