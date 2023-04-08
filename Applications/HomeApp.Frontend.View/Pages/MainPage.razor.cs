using System.Net.Http.Json;
using HomeApp.Frontend.View.Models;
using Microsoft.AspNetCore.Components;
using HomeApp.Shared;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.JsonPatch;

namespace HomeApp.Frontend.View.Pages
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsyncInternal<T>(this HttpClient client,
        string requestUri,
        JsonPatchDocument<T> patchDocument)
        where T : class
        {
            var writer = new StringWriter();
            var serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.Serialize(writer, patchDocument);
            var json = writer.ToString();

            var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
            return await client.PatchAsync(requestUri, content);
        }

        public static async Task<T?> PatchInternal<T>(this HttpClient client, string requestUri, JsonPatchDocument<T> patchDocument) where T : class
        {
            var result = await client.PatchAsyncInternal(requestUri, patchDocument);

            if (!result.IsSuccessStatusCode)
            {
                return default;
            }
            
            var content = await result.Content.ReadFromJsonAsync<T>();
            return content;
        }
    }
    public partial class MainPage : ComponentBase
    {
        private bool myShowPopup;
        private ShoppingListViewModel ShoppingList { get; set; }
        private string myNewItemName { get; set; }

        [Inject]
        public HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ShoppingList = new ShoppingListViewModel(Http);
            ShoppingListItemDto[]? result = await Http.GetFromJsonAsync<ShoppingListItemDto[]>("shoppinglistitem");
            if (result != null)
            {
                ShoppingList.Items = result.Select(s => new ShoppingListItemViewModel(Http, s, ShoppingList)).ToList();
            }
        }

        private async Task HandleAddItem()
        {
            HttpResponseMessage result = await Http.PostAsJsonAsync("shoppinglistitem", new ShoppingListItemDto
            {
                ItemName = myNewItemName,
                AmountValueString = string.Empty
            });

            if (result.StatusCode != System.Net.HttpStatusCode.Created)
            {
                return;
            }
            ShoppingListItemDto? createdResult = await result.Content.ReadFromJsonAsync<ShoppingListItemDto>();
            if (createdResult == null)
            {
                return;
            }
            ShoppingList.AddItem(new ShoppingListItemViewModel(Http, createdResult, ShoppingList));

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
