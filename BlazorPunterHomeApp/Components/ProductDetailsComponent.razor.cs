using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using PunterHomeDomain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Components
{
    public partial class ProductDetailsComponent : ComponentBase
    {
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

        public ProductDetailsModel Product { get; set; }
        public ProductQuantity NewProductQuantity { get; private set; } = new ProductQuantity();

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            Product = await ProductService.GetProductById(ProductId);
        }

        private async void HandleDeleteProductQuantity(int id)
        {
            await ProductService.DeleteProductQuantity(id);
            StateHasChanged();
        }

        private async void HandleAddToCart(int quantityId)
        {
            await ShoppingListService.AddToShoppingList(Guid.Empty, quantityId);
        }

        private async void HandleDeleteProduct(ProductDetailsModel vm)
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
    }
}
