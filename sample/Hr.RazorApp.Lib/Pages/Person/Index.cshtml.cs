using EDennis.AspNetCore.Base.EntityFramework;
using Hr.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.RazorApp.PersonPages {
    public class IndexModel : PageModel {
        private readonly HrApi _api;

        public IndexModel(HrApi api) {
            _api = api;
        }

        [BindProperty(SupportsGet = true)] public string Where { get; set; }
        [BindProperty(SupportsGet = true)] public string OrderBy { get; set; }
        [BindProperty(SupportsGet = true)] public string Select { get; set; }
        [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = -1;
        [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)] public int RowCount { get; set; } = -1;

        [BindProperty] public PagingData PagingData { get; set; }


        public IList<Person> Persons { get; set; }

        public async Task OnGetAsync() {
            PagingData = new PagingData {

                RowCount = RowCount,
                CurrentPage = CurrentPage,
                PageSize = PageSize
            };

            DynamicLinqResult<Person> result = await _api.GetPersonsAsync(
                skip: (PagingData.CurrentPage - 1) * PagingData.PageCount,
                take: PagingData.PageSize
                );

            Persons = result.Data;

        }
    }
}
