using Hr.BlazorApp.Lib.Data.Models;
using Hr.BlazorApp.Lib.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Hr.BlazorApp.Components {

    /// <summary>
    /// Modeled after Gill Cleeran's Pluralsight course, 
    /// Blazor: Getting Started (EmployeeEdit component)
    /// </summary>
    public class AddressPopupBase : ComponentBase {
        public Address Address { get; set; } = new Address();

        public string PopupTitle { get; set; }

        [Parameter] public int AddressId { get; set; }
        [Parameter] public int PersonId { get; set; }
        [Parameter] public EventCallback<bool> CloseEventCallback { get; set; }

        [Inject] public IAddressService AddressService { get; set; }
        [Inject] public IStateService StateService { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }

        public IEnumerable<State> States { get; set; } = new List<State>();

        protected string State = string.Empty;

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected bool Dirty { get; set; }



        protected override async Task OnInitializedAsync() {
            States = await StateService.GetAllAsync();
            if (AddressId == default) {
                PopupTitle = "Add Address";
                Address = new Address();
            } else {
                PopupTitle = "Edit Address";
                Address = await AddressService.GetAsync(AddressId);
            }
            //StateHasChanged();
        }

        protected async Task HandleValidSubmit() {
            Address.State = State;
            Address.PersonId = PersonId;

            if (Address.Id == default) {
                var addedAddress = await AddressService.CreateAsync(Address);
                if (addedAddress != null) {
                    StatusClass = "alert-success";
                    Message = "New address added successfully";
                    Dirty = false;
                    Saved = true;
                    StateHasChanged();
                    await CloseEventCallback.InvokeAsync(true);
                } else {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new address.  Please try again.";
                    Saved = false;
                }
            } else {
                await AddressService.UpdateAsync(Address);
                StatusClass = "alert-success";
                Message = "Address updated successfully";
                Dirty = false;
                Saved = true;
                StateHasChanged();
                await CloseEventCallback.InvokeAsync(true);
            }
        }

        protected void HandleInvalidSubmit() {
            StatusClass = "alert-danger";
            Message = "There are some validation errors.  Please try again.";
        }




        protected async Task DeleteAddress() {
            bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", $"Do you really want to delete the record for {Address.StreetAddress}, {Address.City}?");
            if (confirmed) {
                await AddressService.DeleteAsync(Address.Id);
                StatusClass = "alert-success";
                Message = "Deleted successfully";
                Dirty = false;
                Saved = true;
                await CloseEventCallback.InvokeAsync(true);
            }
        }

        public async Task Close() {
            if (Dirty) {
                bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", $"The current record is not saved.  Do you want to discard all changes?");
                if (!confirmed)
                    return;
            }
            await CloseEventCallback.InvokeAsync(true);
        }


    }
}
