using PunterHomeApiConnector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PunterHomeApiConnector.Clients
{
    public class RecipeStepApiConnector : IRecipeStepApiConnector
    {

        private readonly PunterHomeApiClient myApiConnector;
        public RecipeStepApiConnector()
        {
            myApiConnector = new PunterHomeApiClient(new System.Net.Http.HttpClient());
        }

        public async Task RemoveIngredient(Guid recipeStepId, Guid ingredientId)
        {
            var request = new RemoveIngredientFromRecipeStepRequest
            {
                IngredientId = ingredientId,
                RecipeStepId = recipeStepId
            };
            await myApiConnector.RecipeStep_RemoveIngredientAsync(request);
        }
    }

    public class ShoppingListApiConnector : IShoppingListApiConnector
    {
        private readonly PunterHomeApiClient myApiConnector;
        public ShoppingListApiConnector()
        {
            myApiConnector = new PunterHomeApiClient(new System.Net.Http.HttpClient());
        }
        public async Task DecreaseShoppingListProduct(Guid shoppingListId, int prodQuantityId)
        {
            //await myApiConnector.ShoppingList_AddMeasurementToShoppingListAsync(new AddMeasurementsToShoppingList
            //{
            //    MeasurementId = prodQuantityId,
            //    ShoppingListId = shoppingListId,
            //    Count = -1
            //});
        }

        public async Task DecreaseShoppingListRecipe(Guid shoppingListId, Guid RecipeId)
        {
            await myApiConnector.ShoppingList_AddRecipeToShoppingListAsync(new AddRecipeToShoppingListItem
            {
                ShoppingListId = shoppingListId,
                Amount = -1,
                RecipeId = RecipeId
            });
        }

        public async Task IncreaseShoppingListProduct(Guid shoppingListId, int prodQuantityId)
        {
            //await myApiConnector.Shopp ShoppingList_AddMeasurementToShoppingListAsync(new AddMeasurementsToShoppingList
            //{
            //    MeasurementId = prodQuantityId,
            //    ShoppingListId = shoppingListId,
            //    Count = 1
            //});
        }

        public async Task IncreaseShoppingListRecipe(Guid shoppingListId, Guid RecipeId)
        {
            await myApiConnector.ShoppingList_AddRecipeToShoppingListAsync(new AddRecipeToShoppingListItem
            {
                ShoppingListId = shoppingListId,
                Amount = 1,
                RecipeId = RecipeId
            });
        }


        public async Task<IEnumerable<ShoppingListDto>> GetItems()
        {
            var result = await myApiConnector.ShoppingList_GetAllAsync();
            return result.Where(w => w.Status != EShoppingListStatus.Closed);
        }

        public async Task<IEnumerable<ShoppingListItemDto>> GetTextIems(Guid shoppinglistId)
        {
            return await myApiConnector.ShoppingList_GetTextItemsForShoppingListAsync(shoppinglistId);
        }

        public async Task<IEnumerable<ShoppingListProductItemDto>> GetProductIems(Guid shoppinglistId)
        {
            return await myApiConnector.ShoppingList_GetProductItemsForShoppingListAsync(shoppinglistId);
        }

        public async Task<IEnumerable<ShoppingListRecipeItemDto>> GetRecipeIems(Guid shoppinglistId)
        {
            return await myApiConnector.ShoppingList_GetRecipeItemsForShoppingListAsync(shoppinglistId);
        }

        public async Task<Guid> CreateShoppingList(string name)
        {
            return await myApiConnector.ShoppingList_CreateShoppingListAsync(name);
        }

        public async Task AddTextItem(Guid shoppingListId, string textValue)
        {
            await myApiConnector.ShoppingList_AddTextItemToShoppingListAsync(shoppingListId, textValue);
        }

        public async Task RemoveTextItem(Guid shoppingListId, string textValue)
        {
            await myApiConnector.ShoppingList_RemoveTextItemFromShoppingListAsync(shoppingListId, textValue);
        }

        public async Task AddProductItem(Guid shoppingListId, Guid productId, double amount, int measurementType)
        {
            await myApiConnector.ShoppingList_AddProductItemToShoppingListAsync(shoppingListId, new AddProductToShoppingListRequest
            {
                MeasurementType = (EUnitMeasurementType2)measurementType,
                Amount = amount,
                ProductId = productId
            });

        }

        public async Task AddRecipeItem(Guid shoppingListId, Guid recipeId, int amount)
        {
            await myApiConnector.ShoppingList_AddRecipeItemToShoppingListAsync(shoppingListId, recipeId, amount);
        }

        public async Task SetProductChecked(Guid shoppingListId, Guid productId, bool v)
        {
            await myApiConnector.ShoppingList_SetProductCheckedAsync(shoppingListId, productId, v);
        }

        public async Task SetItemChecked(Guid shoppingListId, string text, bool v)
        {
            await myApiConnector.ShoppingList_SetItemCheckedAsync(shoppingListId, text, v);
        }

        public async Task SetShoppingListShopping(Guid shoppingListId)
        {
            await myApiConnector.ShoppingList_SetShoppingShoppingListAsync(shoppingListId);
        }


        public async Task CloseShoppingList(Guid shoppingListId)
        {
            await myApiConnector.ShoppingList_CloseShoppingListAsync(shoppingListId);
        }


        public async Task MoveUncheckedItemsToNewShoppingList(Guid shoppingListId)
        {
            await myApiConnector.ShoppingList_MoveUncheckedToNewShoppingListAsync(shoppingListId);
        }

        public async Task RemoveShoppingList(Guid id)
        {
            await myApiConnector.ShoppingList_CloseShoppingListAsync(id);
        }
    }
}
