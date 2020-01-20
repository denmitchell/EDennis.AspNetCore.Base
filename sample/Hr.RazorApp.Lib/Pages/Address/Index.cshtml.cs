using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Hr.Api.Models;

namespace Hr.RazorApp.AddressPages
{
    public class IndexModel : PageModel
    {
        private readonly AddressRepo _repo;

        public IndexModel(AddressRepo repo)
        {
            _repo = repo;
        }

        public IList<Address> Address { get;set; }

        public async Task OnGetAsync()
        {
            Address = await _repo.Query.ToListAsync();
        }
    }
}
