using BlazorPunterHomeApp.Components;
using BlazorPunterHomeApp.Data;
using DataModels.Measurements;
using Microsoft.AspNetCore.Components;
using PunterHomeApiConnector;
using PunterHomeApiConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Enums;

namespace BlazorPunterHomeApp.Pages
{
    public abstract class BaseShopViewModel
    {
        public BaseShopViewModel(int id)
        {
            Id = id;
        }
        public int Id { get; }
        public bool IsChecked { get; set; }
        public abstract string Name { get; }
        public abstract Task Check();

        public abstract Task UnCheck();
    }

    public class ShopProductViewModel : BaseShopViewModel
    {
        private readonly IShoppingListApiConnector shoppingListApiConnector;
        private readonly Guid shoppingListId;
        private readonly Guid productId;

        public ShopProductViewModel(int id, IShoppingListApiConnector shoppingListApiConnector, Guid shoppingListId, ShoppingListProductItemDto productDto) : base(id)
        {
            this.shoppingListApiConnector = shoppingListApiConnector;
            this.shoppingListId = shoppingListId;
            this.productId = productDto.ProductId;

            Enums.EUnitMeasurementType mType = (Enums.EUnitMeasurementType)productDto.MeasurementType;
            Name = $"{productDto.Amount} {mType} {productDto.ProductName}";
            IsChecked = productDto.IsCheck;
        }

        override public string Name { get; }

        public override async Task Check()
        {
            await shoppingListApiConnector.SetProductChecked(shoppingListId, productId, true);
        }

        public override async Task UnCheck()
        {
            await shoppingListApiConnector.SetProductChecked(shoppingListId, productId, false);
        }
    }

    public class ShopItemViewModel : BaseShopViewModel
    {
        private readonly IShoppingListApiConnector shoppingListApiConnector;
        private readonly Guid shoppingListId;
        private readonly string text;

        public ShopItemViewModel(int id, IShoppingListApiConnector shoppingListApiConnector, Guid shopping, string text, bool isChecked) : base(id)
        {
            this.shoppingListApiConnector = shoppingListApiConnector;
            this.text = text;
            Name = text;
            shoppingListId = shopping;
            IsChecked = isChecked;
        }

        override public string Name { get; }
        public override async Task Check()
        {
            await shoppingListApiConnector.SetItemChecked(shoppingListId, text, true);
        }

        public override async Task UnCheck()
        {
            await shoppingListApiConnector.SetItemChecked(shoppingListId, text, false);
        }
    }

    public partial class ShoppingListShopView : ComponentBase
    {
        [Parameter]
        public Guid Id { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Inject]
        public IShoppingListApiConnector ShoppingListApiConnector { get; set;}

        [Inject]
        public BlazorShoppingListService ShoppingListService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        protected async override Task OnParametersSetAsync()
        {
            await Refresh();
        }

        public List<BaseShopViewModel> ListItems { get; set; } = new List<BaseShopViewModel>();
        public List<BaseShopViewModel> UnCheckedListItems { get; set; } = new List<BaseShopViewModel>();
        public List<BaseShopViewModel> CheckedListItems { get; set; } = new List<BaseShopViewModel>();
        public SelectMeasurementsForShopItem SelectModal;
        public List<SelectableMeasurement> MeasurementOptions = new List<SelectableMeasurement>();
        public double ModalRequiredAmount { get; set; }
        public Enums.EUnitMeasurementType ModalMeasurementType { get; set; }
        public async Task Refresh()
        {
            //var items = await ShoppingListService.GetShoppingListToShopItems();
            //ListItems = items.Select(i => new SelectableShoppingListItem(i)).ToList();
            //UnCheckedListItems = ListItems.Where(li => !li.Item.IsChecked).ToList();
            //CheckedListItems = ListItems.Where(li => li.Item.IsChecked).ToList();

            ListItems.Clear();
            UnCheckedListItems.Clear();

            CheckedListItems.Clear();

            var products = await ShoppingListApiConnector.GetProductIems(Id);
            var items = await ShoppingListApiConnector.GetTextIems(Id);

            int index = 0;
            ListItems.AddRange(items.Select(s => new ShopItemViewModel(index++, ShoppingListApiConnector, Id, s.Value, s.IsChecked)));
            ListItems.AddRange(products.Select(s => new ShopProductViewModel(index++ + 200, ShoppingListApiConnector, Id, s)));

            UnCheckedListItems.AddRange(ListItems.Where(l => !l.IsChecked));
            CheckedListItems.AddRange(ListItems.Where(l => l.IsChecked));

            StateHasChanged();
        }
        public async void ModalOnConfirm()
        {
            SelectModal.Hide();

            // do some
            ShoppingListService.AddMeasurementsToShoppingItem(SelectModal.MeasurementsOptions, SelectedItem.Item.ProductId);

            SelectedItem.Item.IsChecked = !SelectedItem.Item.IsChecked;
            await ShoppingListService.UpdateCheckedForItem(SelectedItem.Item.ProductId, SelectedItem.Item.IsChecked);
            if (true)
            {
                StateHasChanged();
                await Task.Run(() => System.Threading.Thread.Sleep(80));

            }
            await Refresh();

        }
        private SelectableShoppingListItem SelectedItem;
        public async void ItemClicked(SelectableShoppingListItem item, bool skipSleep = false)
        {
            SelectedItem = item;
            if (!item.Item.IsChecked)
            {
                if (item.Item.IsOneTimeItem)
                {
                    await ShoppingListService.UpdateCheckedForItem(SelectedItem.Item.ProductId, !SelectedItem.Item.IsChecked);
                    await Refresh();
                    return;
                }
                ModalMeasurementType = item.Item.Measurement.MeasurementType;
                ModalRequiredAmount = item.Item.Measurement.UnitQuantityTypeVolume;
                var options = await ShoppingListService.GetMeasurementsForProduct(item.Item.ProductId);
                MeasurementOptions = options.Select(m => new SelectableMeasurement(m)).ToList();
                SelectModal.MeasurementsOptions = options.Select(m => new SelectableMeasurement(m)).ToList();
                SelectModal.Show();
                return;
            }
            SelectedItem.Item.IsChecked = !SelectedItem.Item.IsChecked;
            await ShoppingListService.UpdateCheckedForItem(SelectedItem.Item.ProductId, SelectedItem.Item.IsChecked);
            if (true)
            {
                StateHasChanged();
                await Task.Run(() => System.Threading.Thread.Sleep(80));

            }
            await Refresh();
        }

        public async void UpdateCheckedItems()
        {
            await ShoppingListApiConnector.CloseShoppingList(Id);
            await Refresh();

            NavigationManager.NavigateTo("shoppinglistoverview");
        }

        public async void ItemChecked(BaseShopViewModel item)
        {
            await item.Check();
            await Refresh();
        }


        public async void ItemUnchecked(BaseShopViewModel item)
        {
            await item.UnCheck();
            await Refresh();
        }
    }
}
