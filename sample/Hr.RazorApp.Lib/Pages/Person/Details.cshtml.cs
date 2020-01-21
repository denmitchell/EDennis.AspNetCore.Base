using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Hr.Api.Models;

namespace Hr.RazorApp.PersonPages
{
    public class DetailsModel : PageModel
    {
        private readonly HrApi _api;

        public DetailsModel(HrApi api)
        {
            _api = api;
        }

        public Person Person { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var result = await _api.GetAddressDetailsAsync(id.Value);

            if (result.StatusCode == 404)
                return NotFound();
            else
                Person = (Person)result.Value;
            return Page();
        }
    }
}
