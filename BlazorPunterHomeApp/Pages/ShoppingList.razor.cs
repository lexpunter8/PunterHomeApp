using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Pages
{
    public partial class ShoppingList : ComponentBase
    {
        [Inject]
        public BlazorShoppingListService ShoppingListService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await Refresh();
        }

        public List<ShoppingListItem> ListItems { get; set; } = new List<ShoppingListItem>();

        public async void AddQuantityToItem(ShoppingListItem item)
        {
            await ShoppingListService.UpdateCountForItem(item.Id, true);
            ListItems.FirstOrDefault(i => i.Id == item.Id).Quantity += 1;
            StateHasChanged();
        }
        public async void DecreaseQuantityToItem(ShoppingListItem item)
        {
            await ShoppingListService.UpdateCountForItem(item.Id, false);
            ListItems.FirstOrDefault(i => i.Id == item.Id).Quantity -= 1;
            StateHasChanged();
        }

        public async void DeleteItem(ShoppingListItem item)
        {
            await ShoppingListService.DeleteItem(item.Id);
            ListItems.Remove(ListItems.FirstOrDefault(i => i.Id == item.Id));
            StateHasChanged();
        }

        public async Task Refresh()
        {
            ListItems = await ShoppingListService.GetShoppingListItems();
            StateHasChanged();
        }
    }

}
