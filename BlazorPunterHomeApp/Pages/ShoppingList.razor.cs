using BlazorPunterHomeApp.Data;
using DataModels.Measurements;
using Microsoft.AspNetCore.Components;
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

    public class ShoppingListItemViewModel
    {
        public ShoppingListItemViewModel(ShoppingListItemModel model)
        {
            Model = model;
        }

        public bool ShowInfo { get; set; }

        public string Name => Model.Recipe?.RecipeName ?? Model.Product.ProductName;

        public (double amount, EUnitMeasurementType type) GetProductAmount()
        {
            var measurements = Model.Product.Measurements.ToDictionary(p => BaseMeasurement.GetMeasurement(p.Measurement), s => s.Count).ToList();

            var startMeasurements = BaseMeasurement.GetMeasurement(measurements.First().Key.MeasurementType);
            for (int i = 0; i < measurements.Count(); i++)
            {
                startMeasurements.AddMeasurementAmount(measurements[i].Value * measurements[i].Key.ConvertTo(startMeasurements.MeasurementType));
            }
            return (startMeasurements.UnitQuantityTypeVolume, startMeasurements.MeasurementType);
        }

        public ShoppingListItemModel Model { get; }
    }
    public partial class ShoppingList : ComponentBase
    {
        [Inject]
        public BlazorShoppingListService ShoppingListService { get; set; }

        [Inject]
        public IShoppingListApiConnector ShoppingListApiConnector { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await Refresh();
        }

        public List<ShoppingListItemViewModel> RecipeItems { get; set; } = new List<ShoppingListItemViewModel>();
        public List<ShoppingListItemViewModel> ProductItems { get; set; } = new List<ShoppingListItemViewModel>();
        public List<ShoppingListItemViewModel> Items { get; set; } = new List<ShoppingListItemViewModel>();

        public TextInputModel newItemModel { get; set; } = new TextInputModel();

        public async void AddQuantityToItem(ShoppingListItemViewModel item, bool isRecipe, int prodQuenId = -1)
        {
            if (isRecipe)
            {
                await ShoppingListApiConnector.IncreaseShoppingListRecipe(ShoppingListService.ShoppingListId, item.Model.Recipe.RecipeId);
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
                await ShoppingListApiConnector.DecreaseShoppingListRecipe(ShoppingListService.ShoppingListId, item.Model.Recipe.RecipeId);
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
            var items = await ShoppingListService.GetShoppingListItems();

            RecipeItems = items.Where(i => i.Recipe != null).Select(i => new ShoppingListItemViewModel(i)).OrderBy(p => p.Model.Recipe.RecipeName).ToList();
            ProductItems = items.Where(i => i.Product != null).Select(i => new ShoppingListItemViewModel(i)).OrderBy(p => p.Model.Product.ProductName).ToList();
            Items = items.Where(i => !string.IsNullOrEmpty(i.ItemValue)).Select(i => new ShoppingListItemViewModel(i)).ToList();

            ProductItems.ForEach(p => p.Model.Product.Measurements = p.Model.Product.Measurements.OrderBy(m => m.Measurement.ProductQuantityId).ToList());
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
            ProductItems.ForEach(i => i.ShowInfo = false);
            item.ShowInfo = true;
        }

        public async void AddItem()
        {
            await ShoppingListService.AddOneTimeItemToShoppingList(ShoppingListService.ShoppingListId, newItemModel.Text);
            await Refresh();
        }

    }

}
