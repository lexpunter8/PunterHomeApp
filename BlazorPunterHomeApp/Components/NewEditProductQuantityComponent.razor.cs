using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorPunterHomeApp.Data;
using BlazorPunterHomeApp.ViewModels;
using Microsoft.AspNetCore.Components;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Enums;

namespace BlazorPunterHomeApp.Components
{
    public partial class NewEditProductQuantityComponent : ComponentBase
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }

        [Parameter]
        public ProductModel Product { get; set; }

        public ProductQuantity NewProductQuantity { get; set; } = new ProductQuantity();

        public List<EUnitQuantityType> SelectableUnitQuantityTypes => Enum.GetValues(typeof(EUnitQuantityType)).Cast<EUnitQuantityType>().ToList();

        public async void Save()
        {
            await ProductService.AddQuantityToProduct(NewProductQuantity, Product);
            await BlazoredModal.Close(ModalResult.Ok(NewProductQuantity));
        }

        public async void Cancel()
        {
            await BlazoredModal.Cancel();
        }
    }
}
