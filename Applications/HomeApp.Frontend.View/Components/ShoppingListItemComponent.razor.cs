using HomeApp.Frontend.View.Models;
using Microsoft.AspNetCore.Components;

namespace HomeApp.Frontend.View.Components
{
    public partial class ShoppingListItemComponent : ComponentBase
    {
        public bool IsEditting { get; set; }

        [Parameter]
        public ShoppingListItemViewModel Item { get; set; }

        protected override void OnParametersSet()
        {
            Item.PropertyChanged += Item_PropertyChanged;
            base.OnParametersSet();
        }

        private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StateHasChanged();
        }

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
        }

        private void DecreaseCount()
        {
            Item.Decrease();
        }
    }
}
