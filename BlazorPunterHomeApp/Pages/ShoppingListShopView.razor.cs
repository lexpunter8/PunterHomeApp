using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task Refresh()
        {
            var items = await ShoppingListService.GetShoppingListItems();
            ListItems = items.Select(i => new SelectableShoppingListItem(i)).ToList();
            UnCheckedListItems = ListItems.Where(li => !li.IsChecked).ToList();
            CheckedListItems = ListItems.Where(li => li.IsChecked).ToList();
            StateHasChanged();
        }

        public async void ItemClicked(SelectableShoppingListItem item, bool skipSleep = false)
        {
            item.IsChecked = !item.IsChecked;
            await ShoppingListService.UpdateCheckedForItem(item.Item.Id, item.IsChecked);
            if (!skipSleep)
            {
                StateHasChanged();
                await Task.Run(() => System.Threading.Thread.Sleep(80)); 

            }
            UnCheckedListItems = ListItems.Where(li => !li.IsChecked).ToList();
            CheckedListItems = ListItems.Where(li => li.IsChecked).ToList();
            StateHasChanged();

        }
    }
}
