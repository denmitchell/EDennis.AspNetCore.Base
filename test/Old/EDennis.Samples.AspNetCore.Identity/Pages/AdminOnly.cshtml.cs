using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDennis.Samples.AspNetCore.Identity.Pages
{
    [Authorize("Admin")]
    public class AdminOnlyModel : PageModel
    {

        [BindProperty]
        public IEnumerable<Claim> Claims { get; set; }

        [BindProperty]
        public string UserName { get; set; }

        public void OnGet()
        {
            UserName = User.Identity.Name;
            Claims = User.Claims;
        }
    }
}