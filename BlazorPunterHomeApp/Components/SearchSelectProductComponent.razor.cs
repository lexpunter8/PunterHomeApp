using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using static Enums;

namespace BlazorPunterHomeApp.Components
{
    public partial class SearchSelectProductComponent : ComponentBase
    {
        private List<IngredientModel> myProductsToAdd = new List<IngredientModel>();

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }
        
        [Parameter]
        public Guid RecipeId { get; set; }
        [Parameter]
        public Guid[] ExistingProductIds { get; set; }
        public List<EUnitMeasurementType> SelectableUnitQuantityTypes => Enum.GetValues(typeof(EUnitMeasurementType)).Cast<EUnitMeasurementType>().ToList();

        public SelectableProduct CurrentSelectedProduct { get; set; }
        public string SearchTextString { get; set; }
        public List<SelectableProduct> Products { get; private set; } = new List<SelectableProduct>();
        

        public async void Search()
        {
            var products = await ProductService.SearchProducts(SearchTextString);

            Products = products.Select(p => {
                return new SelectableProduct(p)
                {
                };
                
                }).ToList();
            StateHasChanged();
        }

        [Parameter]
        public EventCallback<IngredientModel> OnIngredientAdded { get; set; }
        public async void AddProduct(SelectableProduct prod)
        {
            IngredientModel ingredient = new IngredientModel
            {
                ProductName = prod.Product.Name,
                ProductId = prod.Product.Id,
                UnitQuantity = prod.CurrentUnitQuantityTypeVolume,
                UnitQuantityType = prod.UnitQuantityType,
                RecipeId = RecipeId
            };

            await RecipeService.InsertIngredient(ingredient);
            await OnIngredientAdded.InvokeAsync(ingredient);
            prod.IsAdding = false;
        }

        public async void Ok()
        {
            await RecipeService.InsertIngredients(myProductsToAdd);
            await BlazoredModal.CloseAsync(ModalResult.Ok(myProductsToAdd));
        }
        public async void Cancel()
        {
            await BlazoredModal.CancelAsync();
        }
        public void SetIsAdding(SelectableProduct cnt)
        {
            Products.ForEach(p => p.IsAdding = false);
            cnt.IsAdding = true;
        }
    }

    public class SelectableProduct
    {
        public SelectableProduct(ProductModel product)
        {
            Product = product;
        }

        public string Name => Product.Name;
        public bool IsSelected { get; set; }
        public ProductModel Product { get; }
        public bool IsAdding { get; set; }


        public int CurrentUnitQuantityTypeVolume { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
    }
}
