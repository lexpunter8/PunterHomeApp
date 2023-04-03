using HomeApp.Frontend.View.Models;
using Microsoft.AspNetCore.Components;

namespace HomeApp.Frontend.View.Components
{
    public partial class ShoppingListItemComponent : ComponentBase
    {
        public bool IsEditting { get; set; }

        [Parameter]
        public ShoppingListItemViewModel Item { get; set; }

        private void Edit()
        {
            if (Item.IsChecked)
            {
                return;
            }
            IsEditting = true;
        }

        private void StopEdit()
        {
            IsEditting = false;
        }

        private void OnCheckedChanged(bool isChecked)
        {
            Item.IsChecked = isChecked;
            StopEdit();
        }

        private void IncreaseCount()
        {
            Item.Count++;
            StateHasChanged();
        }

        private void DecreaseCount()
        {
            Item.Decrease();
            StateHasChanged();
        }
    }
}
