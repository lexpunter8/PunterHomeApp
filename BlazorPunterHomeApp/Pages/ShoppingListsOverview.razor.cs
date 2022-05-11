using Microsoft.AspNetCore.Components;
using PunterHomeApiConnector;
using PunterHomeApiConnector.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Pages
{
    public partial class ShoppingListsOverview : ComponentBase
    {
        [Inject]
        IShoppingListApiConnector ShoppingListApiConnector { get; set; }
        public IEnumerable<ShoppingListDto> ShoppingLists { get; private set; } = new List<ShoppingListDto>();

        public TextInputModel NewShoppingListValue { get; set; } = new TextInputModel();

        protected override async Task OnInitializedAsync()
        {
            ShoppingLists = await ShoppingListApiConnector.GetItems();
            await base.OnInitializedAsync();
        }

        public async void CreateShoppingList()
        {
            if (NewShoppingListValue.Text == string.Empty)
            {
                return;
            }
            await ShoppingListApiConnector.CreateShoppingList(NewShoppingListValue.Text);
            NewShoppingListValue.Text = string.Empty;

            ShoppingLists = await ShoppingListApiConnector.GetItems();

        }

        public string GetLinkSuffix(EShoppingListStatus status)
        {
            if (status == EShoppingListStatus.Shopping)
            {
                return "/shop";
            }
            return string.Empty;
        }

        public async void RemoveItem(ShoppingListDto s)
        {
            await ShoppingListApiConnector.RemoveShoppingList(s.Id);

            ShoppingLists = await ShoppingListApiConnector.GetItems();
            StateHasChanged();
        }

        public string GetStatusString(EShoppingListStatus status)
        {
            switch (status)
            {
                case EShoppingListStatus.Active:
                    return "";
                case EShoppingListStatus.Shopping:
                    return "Winkelen";
                    default: return string.Empty;
            }
        }
    }
}