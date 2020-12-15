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
    public partial class SearchSelectProductComponent
    {
        private List<IngredientModel> myProductsToAdd = new List<IngredientModel>();

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }
        
        [Parameter]
        public Guid RecipeId { get; set; }
        [Parameter]
        public Guid[] ExistingProductIds { get; set; }
        public int CurrentUnitQuantityTypeVolume { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
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
                    IsSelected = myProductsToAdd.Any(x => x.ProductId == p.Id) || ExistingProductIds.Any(x => p.Id == x)
                };
                
                }).ToList();
            StateHasChanged();
        }

        public void AddProduct()
        {
            IngredientModel ingredient = new IngredientModel
            {
                ProductId = CurrentSelectedProduct.Product.Id,
                UnitQuantity = CurrentUnitQuantityTypeVolume,
                UnitQuantityType = UnitQuantityType,
                RecipeId = RecipeId
            };

            myProductsToAdd.Add(ingredient);
            CurrentSelectedProduct.IsSelected = true;
        }

        public void ProductSelected(SelectableProduct product)
        {
            CurrentSelectedProduct = product;
            StateHasChanged();
        }

        public async void Ok()
        {
            await RecipeService.InsertIngredients(myProductsToAdd);
            await BlazoredModal.Close(ModalResult.Ok(myProductsToAdd));
        }
        public async void Cancel()
        {
            await BlazoredModal.Cancel();
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
    }
}
