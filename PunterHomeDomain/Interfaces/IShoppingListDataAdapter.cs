using DataModels;
using DataModels.Measurements;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Services;
using System;
using System.Collections.Generic;

namespace PunterHomeDomain.Interfaces
{
    public interface IShoppingListDataAdapter
    {
        List<ShoppingListItemModel> GetItemsForShoppingList(Guid shoppingListId);
        List<ShoppingListItemInfoModel> GetInfoItemsForShoppingListItem(Guid shoppingListItemId);
        List<ShoppingListModel> GetShoppingLists();

        void AddProductToShoppingList(Guid shoppingListId, AddProductToShoppingListRequest request);
        void RemoveProductFromShoppingList(Guid listItemId);

        void UpdateShoppingListCount(Guid shoppingListItemId, int delta);
        void UpdateChecked(Guid itemId, bool isChecked);
        void AddProductToShoppingList(Guid listId, ShoppingListItemModel item);
        void AddMeasurementsToItem(Guid shoppingListItemId, int measurementId, int count);
        List<MeasurementForShopItemModel> GetMeasurementsForCheckedItem(Guid id);
    }
}
