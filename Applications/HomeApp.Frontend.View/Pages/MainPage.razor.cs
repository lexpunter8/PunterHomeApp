using HomeApp.Frontend.View.Models;
using Microsoft.AspNetCore.Components;

namespace HomeApp.Frontend.View.Pages
{
    public partial class MainPage : ComponentBase
    {
        private bool myShowPopup;
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

        private void ShowPopup()
        {
            myShowPopup = true;
        }

        private void HidePopup()
        {
            myShowPopup = false;
        }

        private void RemoveAllItemsClicked()
        {
            ShoppingList.RemoveAllItems();
        }
    }
}
