using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PunterHomeApiConnector.Interfaces
{
    public interface IShoppingListApiConnector
    {
        Task IncreaseShoppingListProduct(Guid shoppingListId, int prodQuantityId); 
        Task DecreaseShoppingListProduct(Guid shoppingListId, int prodQuantityId); 
        Task IncreaseShoppingListRecipe(Guid shoppingListId, Guid RecipeId); 
        Task DecreaseShoppingListRecipe(Guid shoppingListId, Guid RecipeId);
        Task<IEnumerable<ShoppingListDto>> GetItems();
        Task<IEnumerable<ShoppingListItemDto>> GetTextIems(Guid shoppinglistId);
        Task<IEnumerable<ShoppingListProductItemDto>> GetProductIems(Guid shoppinglistId);

        Task<IEnumerable<ShoppingListRecipeItemDto>> GetRecipeIems(Guid shoppinglistId);

        Task<Guid> CreateShoppingList(string name);

        Task AddTextItem(Guid shoppingListId, string textValue);
        Task RemoveTextItem(Guid shoppingListId, string textValue);
        Task AddProductItem(Guid shoppingListId, Guid productId, double amount, int measurementType);
        Task AddRecipeItem(Guid shoppingListId, Guid recipeId, int amount);
        Task SetProductChecked(Guid shoppingListId, Guid productId, bool v);
        Task SetItemChecked(Guid shoppingListId, string text, bool v);
        Task SetShoppingListShopping(Guid shoppingListId);
        Task CloseShoppingList(Guid shoppingListId);
        Task MoveUncheckedItemsToNewShoppingList(Guid shoppingListId);
        Task RemoveShoppingList(Guid id);
    }
}
