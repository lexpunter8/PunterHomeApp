using BlazorPunterHomeApp.Data;
using DataModels.Measurements;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PunterHomeApiConnector;
using PunterHomeApiConnector.Interfaces;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Enums;

namespace BlazorPunterHomeApp.Pages
{
    public class ShoppingListDetailsViewModel
    {
        public ShoppingListDetailsViewModel(ShoppingListItemDetailsModel model)
        {
            Model = model;
        }

        public bool ShowInfo { get; set; }
        public ShoppingListItemDetailsModel Model { get; }
    }

    public class ShoppingListProductViewModel
    {
        public ShoppingListProductViewModel(ShoppingListProductItemDto model)
        {
            Model = model;
        }

        public PunterHomeDomain.Enums.EUnitMeasurementType EUnitMeasurementType => (PunterHomeDomain.Enums.EUnitMeasurementType)Model.MeasurementType;
        public ShoppingListProductItemDto Model { get; }
    }

    public class ShoppingListItemViewModel
    {
        public ShoppingListItemViewModel(ShoppingListRecipeItemDto model)
        {
            Model = model;
        }

        public bool ShowInfo { get; set; }

        public string Name => Model.RecipeName;

        //public (double amount, EUnitMeasurementType type) GetProductAmount()
        //{
        //    var measurements = Model.Product.Measurements.ToDictionary(p => BaseMeasurement.GetMeasurement(p.Measurement), s => s.Count).ToList();

        //    var startMeasurements = BaseMeasurement.GetMeasurement(measurements.First().Key.MeasurementType);
        //    for (int i = 0; i < measurements.Count(); i++)
        //    {
        //        startMeasurements.AddMeasurementAmount(measurements[i].Value * measurements[i].Key.ConvertTo(startMeasurements.MeasurementType));
        //    }
        //    return (startMeasurements.UnitQuantityTypeVolume, startMeasurements.MeasurementType);
        //}

        public ShoppingListRecipeItemDto Model { get; }
    }
    public partial class ShoppingList : ComponentBase
    {
        [Inject]
        public BlazorShoppingListService ShoppingListService { get; set; }

        [Inject]
        public IShoppingListApiConnector ShoppingListApiConnector { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public ElementReference BottomDiv;
        protected override async Task OnInitializedAsync()
        {
            await Refresh();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            BottomDivHeight = await JsRuntime.InvokeAsync<double>("getElementHeight", "myBottomDiv");
            StateHasChanged();
            await base.OnAfterRenderAsync(firstRender);
        }

        public List<ShoppingListItemViewModel> RecipeItems { get; set; } = new List<ShoppingListItemViewModel>();
        public List<ShoppingListProductViewModel> ProductItems { get; set; } = new List<ShoppingListProductViewModel>();
        public List<ShoppingListItemDto> Items { get; set; } = new List<ShoppingListItemDto>();

        public TextInputModel newItemModel { get; set; } = new TextInputModel();
        public double BottomDivHeight { get; private set; }

        public async void SetShopping()
        {
            await ShoppingListApiConnector.SetShoppingListShopping(Id);

            NavigationManager.NavigateTo($"shoppinglist/{Id}/shop/{Name}");
        }

        public async void AddQuantityToItem(ShoppingListItemViewModel item, bool isRecipe, int prodQuenId = -1)
        {
            if (isRecipe)
            {
                await ShoppingListApiConnector.IncreaseShoppingListRecipe(ShoppingListService.ShoppingListId, item.Model.RecipeId);
            }
            else 
            {
                await ShoppingListApiConnector.IncreaseShoppingListProduct(ShoppingListService.ShoppingListId, prodQuenId);
            }
            await Refresh();
        }

        public async void DecreaseQuantityToItem(ShoppingListItemViewModel item, bool isRecipe, int prodQuenId = -1)
        {
            if (isRecipe)
            {
                await ShoppingListApiConnector.DecreaseShoppingListRecipe(ShoppingListService.ShoppingListId, item.Model.RecipeId);
            }
            else 
            { 
                await ShoppingListApiConnector.DecreaseShoppingListProduct(ShoppingListService.ShoppingListId, prodQuenId);
            }

            await Refresh();
        }

        public async void DeleteItem(ShoppingListItemViewModel item)
        {
            //await ShoppingListService.DeleteItem(item.Id);
            //ListItems.Remove(ListItems.FirstOrDefault(i => i.Id == item.Id));
            StateHasChanged();
        }

        public async Task Refresh()
        {
            //var items = await ShoppingListService.GetShoppingListItems(Id);

            //RecipeItems = items.Where(i => i.Recipe != null).Select(i => new ShoppingListItemViewModel(i)).OrderBy(p => p.Model.Recipe.RecipeName).ToList();
            //ProductItems = items.Where(i => i.Product != null).Select(i => new ShoppingListItemViewModel(i)).OrderBy(p => p.Model.Product.ProductName).ToList();
            var textItems = await ShoppingListApiConnector.GetTextIems(Id);
            var productItems = await ShoppingListApiConnector.GetProductIems(Id);
            var recipeItems = await ShoppingListApiConnector.GetRecipeIems(Id);
            Items = textItems.ToList();
            RecipeItems = recipeItems.Select(s => new ShoppingListItemViewModel(s)).ToList();
            ProductItems = productItems.Select(s => new ShoppingListProductViewModel(s)).ToList();

            //ProductItems.ForEach(p => p.Model.Product.Measurements = p.Model.Product.Measurements.OrderBy(m => m.Measurement.ProductQuantityId).ToList());
            StateHasChanged();
        }

        public void ShowInfo(ShoppingListDetailsViewModel item)
        {
            if (item.ShowInfo)
            {
                item.ShowInfo = false;
                return;
            }
            RecipeItems.ForEach(i => i.ShowInfo = false);
            //ProductItems.ForEach(i => i.ShowInfo = false);
            item.ShowInfo = true;
        }

        public async void AddItem()
        {
            await ShoppingListApiConnector.AddTextItem(Id, newItemModel.Text);
            newItemModel.Text = string.Empty;
            await Refresh();
        }

        public async void RemoveItem(string value)
        {
            await ShoppingListApiConnector.RemoveTextItem(Id, value); 
             await Refresh();
        }


        public async void RemoveProduct(string value)
        {
        }

        public string SelectedTab { get; set; } = "LosseItems";

        private Task OnSelectedTabChanged(string name)
        {
            SelectedTab = name;

            return Task.CompletedTask;
        }
    }

}
