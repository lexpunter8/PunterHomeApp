using Microsoft.AspNetCore.Components;

namespace HomeApp.Frontend.View.Components
{
    public partial class ShoppingListItemComponent : ComponentBase
    {
        public bool IsEditting { get; set; }
        public bool IsChecked { get; set; }

        [Parameter]
        public int Count { get; set; }

        private void Edit()
        {
            if (IsChecked)
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
            IsChecked = isChecked;
            StopEdit();
        }

        private void IncreaseCount()
        {
            Count++;
            StateHasChanged();
        }

        private void DecreaseCount()
        {
            Count--;
            StateHasChanged();
        }
    }
}
