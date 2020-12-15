using DataModels;
using DataModels.Measurements;
using System;
using System.Collections.Generic;
using System.Text;

namespace PunterHomeDomain.Interfaces
{
    public interface IShoppingListService
    {
        void AddProductToShoppingList(Guid shoppingListId, int productQuantyId);
        void RemoveProductFromShoppingList(Guid itemId);
        List<ShoppingListItemModel> GetItemsForShoppingList(Guid shoppingListId);
        List<ShoppingListModel> GetShoppingLists();
        void UpdateShoppingListCount(Guid shoppingListItemId, int delta);
        void AddMinimumAmountToShoppingList(Guid shoppingListId, Guid productId, MeasurementAmount amount);
    }
}
