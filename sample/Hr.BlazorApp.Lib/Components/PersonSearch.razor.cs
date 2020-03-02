using Hr.BlazorApp.Lib.Data.Models;
using Hr.BlazorApp.Lib.Data.Services;
using Hr.BlazorApp.Lib.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Hr.BlazorApp.Components {


    public partial class PersonSearchBase : ComponentBase {

        [Inject] public IPersonService PersonService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        public int PersonId { get; set; }

        protected Pager Pager { get; set; }

        protected string Where {
            get {
                List<string> whereList = new List<string>();
                var p = PersonSearchParameters;
                if (p.Id == null
                    && string.IsNullOrEmpty(p.FirstNamePattern)
                    && string.IsNullOrEmpty(p.LastNamePattern))
                    return null;
                if (p.Id != null)
                    whereList.Add($"Id eq {p.Id}");
                if (!string.IsNullOrEmpty(p.LastNamePattern)) {
                    if (p.LastNamePattern.StartsWith("%") || p.LastNamePattern.StartsWith("*"))
                        whereList.Add($"LastName.Contains(\"{p.LastNamePattern.Substring(1)}\")");
                    else
                        whereList.Add($"LastName.StartsWith(\"{p.LastNamePattern}\")");
                }
                if (!string.IsNullOrEmpty(p.FirstNamePattern)) {
                    if (p.FirstNamePattern.StartsWith("%") || p.FirstNamePattern.StartsWith("*"))
                        whereList.Add($"FirstName.Contains(\"{p.FirstNamePattern.Substring(1)}\")");
                    else
                        whereList.Add($"FirstName.StartsWith(\"{p.FirstNamePattern}\")");

                }

                return string.Join(" and ", whereList);
            }
        }

        public PersonSearchParameters PersonSearchParameters { get; set; } = new PersonSearchParameters();


        public IEnumerable<Person> Persons { get; set; } = new List<Person>();

        protected int? rowCount;


        //protected async override Task OnInitializedAsync() => await ExecuteSearchAsync(true);

        public async Task OnPagerChangedAsync(bool _) => await ExecuteSearchAsync(false);



        public async Task OnSearchAsync() => await ExecuteSearchAsync(true);


        private async Task ExecuteSearchAsync(bool resetRowCount) {
            PagedResult pagedResult;
            if (resetRowCount) {
                pagedResult = await PersonService.GetPageAsync(Where, "Id", 1, Pager.PageSize, null);
            } else {
                pagedResult = await PersonService.GetPageAsync(Where, "Id", Pager.CurrentPage, Pager.PageSize, rowCount);
            }
            Persons = pagedResult.Queryable.Cast<Person>();
            rowCount = pagedResult.RowCount;
        }

        protected void OnNewAsync(bool _) {
            NavigationManager.NavigateTo("/PersonDetail/0?Editable=true");
        }

    }
}
