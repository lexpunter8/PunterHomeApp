using HomeApp.Frontend.View.Models;
using Microsoft.AspNetCore.Components;

namespace HomeApp.Frontend.View.Pages
{
    public partial class MainPage : ComponentBase
    {
        private ShoppingListViewModel ShoppingList { get; set; } = new ShoppingListViewModel();
        private string myNewItemName { get; set; }

        private void HandleAddItem()
        {
            ShoppingList.AddItem(new ShoppingListItemViewModel
            {
                AmountValueString = string.Empty,
                ItemName = myNewItemName
            });

            myNewItemName = string.Empty;
        }
    }
}
