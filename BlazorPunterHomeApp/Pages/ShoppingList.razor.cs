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

        public void AddQuantityToItem(ShoppingListItem item)
        {
            ListItems.FirstOrDefault(i => i.Id == item.Id).Quantity += 1;
        }
        public void DecreaseQuantityToItem(ShoppingListItem item)
        {
            ListItems.FirstOrDefault(i => i.Id == item.Id).Quantity -= 1;
        }

        public async Task Refresh()
        {
            ListItems = await ShoppingListService.GetShoppingListItems();
            StateHasChanged();
        }
    }

}
