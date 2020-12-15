using DataModels;
using System;
using System.Collections.Generic;

namespace PunterHomeDomain.Interfaces
{
    public interface IShoppingListDataAdapter
    {
        List<ShoppingListItemModel> GetItemsForShoppingList(Guid shoppingListId);
        List<ShoppingListModel> GetShoppingLists();

        void AddProductToShoppingList(Guid shoppingListId, int productQuantyId, int count = 1);
        void RemoveProductFromShoppingList(Guid shoppingListId);

        void UpdateShoppingListCount(Guid shoppingListItemId, int delta);
    }
}
