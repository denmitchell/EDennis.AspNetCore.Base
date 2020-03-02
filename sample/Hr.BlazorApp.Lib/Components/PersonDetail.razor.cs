using Hr.BlazorApp.Lib.Data.Models;
using Hr.BlazorApp.Lib.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Hr.BlazorApp.Components {
    public class PersonDetailBase : ComponentBase {
        [Inject] public IPersonService PersonService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] IJSRuntime JsRuntime { get; set; }

        [Parameter] public string Id { get; set; }

        public Person Person { get; set; } = new Person();

        [Parameter] public bool Editable { get; set; }
        public bool Dirty { get; set; }

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        public string InputClass => Editable ? "form-control" : "form-control-plaintext";

        protected override async Task OnInitializedAsync() {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("editable", out var strEditable)
                && bool.TryParse(strEditable, out bool booEditable))
                Editable = booEditable;

            int.TryParse(Id, out var personId);
            if (personId == default)
                Person = new Person();
            else {
                if (personId != default)
                    Person = await PersonService.GetAsync(int.Parse(Id));
            }
        }

        protected async Task HandleValidSubmit() {
            if (Person.Id == default) {
                var addedPerson = await PersonService.CreateAsync(Person);
                if (addedPerson != null) {
                    StatusClass = "alert-success";
                    Message = "New person added successfully.";
                    Person = addedPerson;
                    Dirty = false;
                    Saved = true;
                } else {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new person.  Please try again.";
                    Saved = false;
                }
            } else {
                var updatedPerson = await PersonService.UpdateAsync(Person);
                StatusClass = "alert-success";
                Message = "Person updated successfully.";
                Person = updatedPerson;
                Dirty = false;
                Saved = true;
            }
        }

        protected void HandleInvalidSubmit() {
            StatusClass = "alert-danger";
            Message = "There are some validation errors.  Please try again.";
        }

        protected async Task DeletePerson() {
            bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", $"Do you really want to delete the record for {Person.FirstName} {Person.LastName}?");
            if (confirmed) {
                await PersonService.DeleteAsync(Person.Id);
                Dirty = false;
                NavigationManager.NavigateTo("/PersonSearch");
            }
        }

        protected async Task Close() {
            if (Dirty) {
                bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", $"The current record is not saved.  Do you want to discard all changes?");
                if (!confirmed)
                    return;
            }
            NavigationManager.NavigateTo("/PersonSearch");
        }

    }
}
