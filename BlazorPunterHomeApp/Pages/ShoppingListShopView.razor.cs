using BlazorPunterHomeApp.Components;
using BlazorPunterHomeApp.Data;
using DataModels.Measurements;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Enums;

namespace BlazorPunterHomeApp.Pages
{
    public partial class ShoppingListShopView : ComponentBase
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        public BlazorShoppingListService ShoppingListService { get; set; }
        protected async override Task OnParametersSetAsync()
        {
            await Refresh();
        }

        public List<SelectableShoppingListItem> ListItems { get; set; } = new List<SelectableShoppingListItem>();
        public List<SelectableShoppingListItem> UnCheckedListItems { get; set; } = new List<SelectableShoppingListItem>();
        public List<SelectableShoppingListItem> CheckedListItems { get; set; } = new List<SelectableShoppingListItem>();
        public SelectMeasurementsForShopItem SelectModal;
        public List<SelectableMeasurement> MeasurementOptions = new List<SelectableMeasurement>();
        public double ModalRequiredAmount { get; set; }
        public EUnitMeasurementType ModalMeasurementType { get; set; }
        public Guid SelectedItemId { get; set; }
        public async Task Refresh()
        {
            var items = await ShoppingListService.GetShoppingListToShopItems();
            ListItems = items.Select(i => new SelectableShoppingListItem(i)).ToList();
            UnCheckedListItems = ListItems.Where(li => !li.Item.IsChecked).ToList();
            CheckedListItems = ListItems.Where(li => li.Item.IsChecked).ToList();
            StateHasChanged();
        }
        public async void ModalOnConfirm()
        {
            SelectModal.Hide();

            // do some
            ShoppingListService.AddMeasurementsToShoppingItem(SelectModal.MeasurementsOptions, SelectedItemId);

            SelectedItem.Item.IsChecked = !SelectedItem.Item.IsChecked;
            await ShoppingListService.UpdateCheckedForItem(SelectedItem.Item.Id, SelectedItem.Item.IsChecked);
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
                ModalMeasurementType = item.Item.Measurement.MeasurementType;
                ModalRequiredAmount = item.Item.Measurement.UnitQuantityTypeVolume * item.Item.Amount;
                var options = await ShoppingListService.GetMeasurementsForProduct(item.Item.ProductId);
                MeasurementOptions = options.Select(m => new SelectableMeasurement(m)).ToList();
                SelectModal.MeasurementsOptions = options.Select(m => new SelectableMeasurement(m)).ToList();
                SelectedItemId = item.Item.Id;
                SelectModal.Show();
                return;
            }
            SelectedItem.Item.IsChecked = !SelectedItem.Item.IsChecked;
            await ShoppingListService.UpdateCheckedForItem(SelectedItem.Item.Id, SelectedItem.Item.IsChecked);
            if (true)
            {
                StateHasChanged();
                await Task.Run(() => System.Threading.Thread.Sleep(80));

            }
            await Refresh();
        }
    }
}
