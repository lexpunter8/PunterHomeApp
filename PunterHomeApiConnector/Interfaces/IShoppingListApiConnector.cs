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
    }
}
