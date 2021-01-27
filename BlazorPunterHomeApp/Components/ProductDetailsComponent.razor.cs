using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorPunterHomeApp.Data;
using DataModels.Measurements;
using Microsoft.AspNetCore.Components;
using PunterHomeDomain.Models;
using RazorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Components
{
    public partial class ProductDetailsComponent : ComponentBase
    {
        private bool myIsAddingTag;

        [Parameter]
        public Guid ProductId { get; set; }

        [Inject]
        public ProductService ProductService { get; set; }

        [Inject]
        public BlazorShoppingListService ShoppingListService { get; set; }

        [Inject]
        public IModalService Modal { get; set; }

        [Inject]
        public IBarcodeScannerService barcodeScannerService { get; set; }

        [Inject]
        public IBlazorTagService TagService { get; set; }

        public ProductDetailsViewModel Product { get; set; }
        public ProductQuantity NewProductQuantity { get; private set; } = new ProductQuantity();
        public List<TagModel> AllTags { get; set; } = new List<TagModel>();

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            Product = await ProductService.GetProductById(ProductId);
            AllTags = await TagService.GetAllTags();
        }

        private async void HandleDeleteProductQuantity(int id)
        {
            await ProductService.DeleteProductQuantity(id);
            StateHasChanged();
        }

        private async void HandleAddToCart(BaseMeasurement measurement)
        {
            await ShoppingListService.AddToShoppingList(Guid.Empty, new PunterHomeDomain.ApiModels.AddProductToShoppingListRequest
            {
                MeasurementAmount = Convert.ToInt32(measurement.UnitQuantityTypeVolume),
                MeasurementType = measurement.MeasurementType,
                ProductId = ProductId,
                Reason = Enums.EShoppingListReason.Manual
            });
        }

        private async void HandleDeleteProduct(ProductDetailsViewModel vm)
        {
            await ProductService.DeleteProduct(vm);
            StateHasChanged();
        }

        private async void ShowAddProductQuantityModal()
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(NewEditProductQuantityComponent.Product), Product);

            var adddQuantityModal = Modal.Show<NewEditProductQuantityComponent>("Add product quantity", parameters);

            var result = await adddQuantityModal.Result;

            if (!result.Cancelled)
            {
                StateHasChanged();
            }
        }

        private async void HandleAddQuantity()
        {
            await ProductService.AddQuantityToProduct(NewProductQuantity, Product);
            NewProductQuantity = new PunterHomeDomain.Models.ProductQuantity();
            StateHasChanged();
        }


        private async void IncreaseQuantity(int prodQuanId)
        {
            await ProductService.IncreaseProductQuantity(prodQuanId);

            var productDetails = await ProductService.GetProductById(ProductId);
            Product = productDetails;
            StateHasChanged();
        }

        private async void DecreaseQuantity(int prodQuanId)
        {
            await ProductService.DecreaseProductQuantity(prodQuanId);

            var productDetails = await ProductService.GetProductById(ProductId);
            Product = productDetails;
            StateHasChanged();
        }

        private async void HandleAddBarcode(int prodQuanId)
        {
            var barcode = await barcodeScannerService.ScanBarcode(Modal);
            if (barcode == null)
            {
                return;
            }
            ProductService.AddBarcodeToQuantity(prodQuanId, barcode.Barcode);
        }

        public void HandleAddTagClicked()
        {
            myIsAddingTag = true;
            StateHasChanged();
        }
        public async void NewTagClicked(ChangeEventArgs args)
        {
            if (!(args.Value is string selected))
            {
                return;
            }
            var newTag = AllTags.FirstOrDefault(t => t.Name.Equals(selected));

            if (newTag == null)
            {
                return;
            }

            TagService.AddTagToProduct(ProductId, newTag.Id);
            Product = await ProductService.GetProductById(ProductId);
            myIsAddingTag = false;
            NewTag = new ProductTagModel();
            StateHasChanged();
        }

        public async void TagChanged()
        {
            var newTag = AllTags.FirstOrDefault(t => t.Name.Equals(NewTag?.Name));

            if (newTag == null)
            {
                return;
            }

            TagService.AddTagToProduct(ProductId, newTag.Id);
            Product = await ProductService.GetProductById(ProductId);
            myIsAddingTag = false;
            NewTag = new ProductTagModel();
            StateHasChanged();
        }
        private async void MyValueChangeHandler(string theUserInput)
        {
            var newTag = AllTags.FirstOrDefault(t => t.Name.Equals(theUserInput));

            if (newTag == null)
            {
                return;
            }

            TagService.AddTagToProduct(ProductId, newTag.Id);
            Product = await ProductService.GetProductById(ProductId);
            myIsAddingTag = false;
            NewTag = new ProductTagModel();
            StateHasChanged();
        }
        ProductTagModel NewTag = new ProductTagModel();
    }
}
