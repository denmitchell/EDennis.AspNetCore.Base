using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Hr.BlazorApp.Components {
    public class PagerBase : ComponentBase {

        public const int DEFAULT_PAGE_SIZE = 10;
        public const int DEFAULT_PAGE_SET_SIZE = 5;

        //passed in from parent
        [Parameter] public int? RowCount { get; set; }

        public int PageSize { get; set; } = DEFAULT_PAGE_SIZE;
        public int PageSetSize { get; set; } = DEFAULT_PAGE_SET_SIZE;
        public int CurrentPage { get; set; } = 1;
        public int CurrentPageSet { get; set; } = 1;


        //computed
        public int PageCount => (int)Math.Ceiling((RowCount ?? default) / (double)PageSize);
        public int PageSetCount => (int)Math.Ceiling(PageCount / (double)(PageSetSize));
        public int FromPage => 1 + ((CurrentPageSet - 1) * PageSetSize);
        public int ToPage { 
            get {
                var value = CurrentPageSet  * PageSetSize;
                return (int)Math.Min(value, PageCount);
            }
        }


        [Parameter] public EventCallback<bool> PagerChangedCallback { get; set; }
        [Parameter] public EventCallback<bool> NewCallback { get; set; }


        public bool HasNextPageSet => CurrentPageSet < PageSetCount;
        public bool HasPreviousPageSet => CurrentPageSet > 1;



        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (firstRender)
                await PagerChangedAsync();
        }


        public async Task SetPageAsync(int pageNumber) {
            CurrentPage = pageNumber;
            await PagerChangedAsync();
        }

        public async Task NextPageSetAsync() {
            CurrentPageSet++;
            CurrentPage = FromPage;
            await PagerChangedAsync();
        }

        public async Task PreviousPageSetAsync() {
            CurrentPageSet--;
            CurrentPage = FromPage;
            await PagerChangedAsync();
        }

        public async Task PagerChangedAsync() => await PagerChangedCallback.InvokeAsync(true);
        public async Task NewAsync() => await NewCallback.InvokeAsync(true);

    }
}
