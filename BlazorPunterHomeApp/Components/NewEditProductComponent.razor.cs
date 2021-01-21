using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using PunterHomeApp.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Enums;

namespace BlazorPunterHomeApp.Components
{ 
    public partial class NewEditProductComponent : ComponentBase
    {
        public NewProductApiModel Product = new NewProductApiModel();
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }

        [Parameter]
        public string Barcode { get; set; } = string.Empty;

        public bool ShowBarcodeInput => !string.IsNullOrEmpty(Barcode);

        public ProductQuantity NewProductQuantity { get; set; } = new ProductQuantity();

        protected override Task OnParametersSetAsync()
        {
            StateHasChanged();
            return base.OnParametersSetAsync();
        }

        public List<EUnitMeasurementType> SelectableUnitQuantityTypes => Enum.GetValues(typeof(EUnitMeasurementType)).Cast<EUnitMeasurementType>().ToList();

            public async void Save()
        {
            Product.UnitQuantity = NewProductQuantity.UnitQuantityTypeVolume;
            Product.UnitQuantityType = NewProductQuantity.MeasurementType;
            await ProductService.AddProduct(Product);
            await BlazoredModal.CloseAsync(ModalResult.Ok(Product));
        }

        public async void Cancel()
        {
            await BlazoredModal.CancelAsync();
        }
    }
}
