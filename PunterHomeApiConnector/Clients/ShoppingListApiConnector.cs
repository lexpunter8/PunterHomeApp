using PunterHomeApiConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PunterHomeApiConnector.Clients
{
    public class ShoppingListApiConnector : IShoppingListApiConnector
    {
        private readonly PunterHomeApiClient myApiConnector;
        public ShoppingListApiConnector()
        {
            myApiConnector = new PunterHomeApiClient(new System.Net.Http.HttpClient());
        }
        public async Task DecreaseShoppingListProduct(Guid shoppingListId, int prodQuantityId)
        {
            await myApiConnector.ShoppingList_decreaseQuantityToProductShoppinglistItemAsync(shoppingListId, prodQuantityId);
        }

        public async Task DecreaseShoppingListRecipe(Guid shoppingListId, Guid RecipeId)
        {
            await myApiConnector.ShoppingList_DecreaseQuantityToRecipeShoppinglistItemAsync(shoppingListId, RecipeId);
        }

        public async Task IncreaseShoppingListProduct(Guid shoppingListId, int prodQuantityId)
        {
            await myApiConnector.ShoppingList_AddQuantityToProductShoppinglistItemAsync(shoppingListId, prodQuantityId);
        }

        public async Task IncreaseShoppingListRecipe(Guid shoppingListId, Guid RecipeId)
        {
            await myApiConnector.ShoppingList_AddQuantityToRecipeShoppinglistItemAsync(shoppingListId, RecipeId);
        }
    }
}
