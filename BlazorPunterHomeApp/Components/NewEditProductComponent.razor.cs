using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using PunterHomeApp.ApiModels;

namespace BlazorPunterHomeApp.Components
{ 
    public partial class NewEditProductComponent : ComponentBase
    {
        public NewProductApiModel Product = new NewProductApiModel();
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }

        public async void Save()
        {
            await ProductService.AddProduct(Product);
            await BlazoredModal.Close(ModalResult.Ok(Product));
        }

        public async void Cancel()
        {
            await BlazoredModal.Cancel();
        }
    }
}
