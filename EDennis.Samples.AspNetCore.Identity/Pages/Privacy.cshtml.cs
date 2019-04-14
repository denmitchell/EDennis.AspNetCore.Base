using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace EDennis.Samples.AspNetCore.Identity.Pages {
    [Authorize]
    public class PrivacyModel : PageModel {
        public void OnGet() {
        }
    }
}