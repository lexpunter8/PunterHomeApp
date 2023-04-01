using HomeApp.Frontend.View.Models;
using Microsoft.AspNetCore.Components;

namespace HomeApp.Frontend.View.Components
{
    public partial class ShoppingListComponent : ComponentBase
    {
        [Parameter]
        public ShoppingListViewModel ShoppingList { get; set; }

        protected override void OnParametersSet()
        {
            ShoppingList.PropertyChanged += ShoppingList_PropertyChanged;
            base.OnParametersSet();
        }

        private void ShoppingList_PropertyChanged(object? sender, EventArgs e)
        {
            StateHasChanged();
        }
    }
}
