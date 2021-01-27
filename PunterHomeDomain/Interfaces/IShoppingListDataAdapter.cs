using DataModels;
using PunterHomeDomain.ApiModels;
using System;
using System.Collections.Generic;

namespace PunterHomeDomain.Interfaces
{
    public interface IShoppingListDataAdapter
    {
        List<ShoppingListItemModel> GetItemsForShoppingList(Guid shoppingListId);
        List<ShoppingListModel> GetShoppingLists();

        void AddProductToShoppingList(Guid shoppingListId, AddProductToShoppingListRequest request);
        void RemoveProductFromShoppingList(Guid listItemId);

        void UpdateShoppingListCount(Guid shoppingListItemId, int delta);
        void UpdateChecked(Guid itemId, bool isChecked);
        void AddProductToShoppingList(Guid listId, ShoppingListItemModel item);
    }
}
