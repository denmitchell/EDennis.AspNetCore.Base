using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Hr.Api.Models;
using EDennis.AspNetCore.Base.EntityFramework;

namespace Hr.RazorApp.AddressPages
{
    public class IndexModel : PageModel
    {
        private readonly HrApi _api;

        public IndexModel(HrApi api)
        {
            _api = api;
        }

        [BindProperty(SupportsGet = true)] public string Where { get; set; }
        [BindProperty(SupportsGet = true)] public string OrderBy { get; set; }
        [BindProperty(SupportsGet = true)] public string Select { get; set; }
        [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = -1;
        [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)] public int RecordCount { get; set; } = -1;

        [BindProperty] public PagingData PagingData { get; set; }


        public IList<Address> Addresses { get;set; }

        public async Task OnGetAsync()
        {
            PagingData = new PagingData {
                RecordCount = RecordCount,
                PageNumber = PageNumber,
                PageSize = PageSize
            };

            var result = await _api.GetAddressesAsync(
                skip:(PagingData.PageNumber - 1)*PagingData.PageCount,
                take:PagingData.PageSize, 
                pagingData: PagingData
                );

            Addresses = (List<Address>)result.Value;

        }
    }
}
