using DataModels;
using DataModels.Measurements;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PunterHomeDomain.Interfaces
{
    public interface IShoppingListService
    {
        void AddProductToShoppingList(Guid shoppingListId, AddProductToShoppingListRequest request);
        void RemoveProductFromShoppingList(Guid itemId);
        List<ShoppingListItemDetailsModel> GetItemsForShoppingList(Guid shoppingListId);
        List<ShoppingListModel> GetShoppingLists();
        void UpdateShoppingListCount(Guid shoppingListItemId, int delta);
        void AddMinimumAmountToShoppingList(Guid shoppingListId, Guid productId, MeasurementAmount amount);
        void UpdateChecked(Guid itemId, bool isChecked);

        public void AddQuantityToProductForCheckedItems(Guid shoppingListId);
    }
}
