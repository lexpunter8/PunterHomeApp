using Blazored.Modal;
using Blazored.Modal.Services;
using Blazorise;
using BlazorPunterHomeApp.Data;
using DataModels.Measurements;
using Microsoft.AspNetCore.Components;
using PunterHomeApiConnector;
using PunterHomeApiConnector.Interfaces;
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

        [Inject]
        IShoppingListApiConnector ShoppingListApiConnector { get; set; }


        // reference to the modal component
        public Modal modalRef;
        public string SelectedShoppingList { get; set; }

        public IEnumerable<ShoppingListDto> AllShoppingLists { get; private set; }
        public async Task ShowModal()
        {
            var allLists = await ShoppingListApiConnector.GetItems();
            AllShoppingLists = allLists.Where(x => x.Status == EShoppingListStatus.Active);
            await modalRef.Show();
        }

        public Task HideAndSaveModal()
        {
            ShoppingListApiConnector.AddProductItem(Guid.Parse(SelectedShoppingList), ProductId, SelectedMeasurement.UnitQuantityTypeVolume, (int)SelectedMeasurement.MeasurementType);
            return modalRef.Hide();
        }

        public Task HideModal()
        {
            return modalRef.Hide();
        }

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

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public ProductDetailsViewModel Product { get; set; }
        public PunterHomeDomain.Models.ProductQuantity NewProductQuantity { get; private set; } = new PunterHomeDomain.Models.ProductQuantity();
        public List<TagModel> AllTags { get; set; } = new List<TagModel>();

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            Refresh();
        }

        private async void HandleDeleteProductQuantity(int id)
        {
            await ProductService.DeleteProductQuantity(id);
            Refresh();
        }

        private async void HandleAddToCart(BaseMeasurement measurement)
        {
            SelectedMeasurement = measurement;
            await ShowModal();
            //await ShoppingListService.AddToShoppingList(Guid.Empty, new PunterHomeDomain.ApiModels.AddProductToShoppingListRequest
            //{
            //    MeasurementAmount = 1,
            //    ProductMeasurementId = measurement.ProductQuantityId
            //});
        }

        private async void HandleDeleteProduct(ProductDetailsViewModel vm)
        {
            await ProductService.DeleteProduct(vm);
            NavigationManager.NavigateTo("/products");
            StateHasChanged();
        }

        private async void Refresh()
        {
            Product = await ProductService.GetProductById(ProductId);
            AllTags = await TagService.GetAllTags();
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
                Refresh();
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
        private BaseMeasurement SelectedMeasurement;
    }


}
