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
        List<ShoppingListModel> GetShoppingLists();
        List<ShoppingListProductModel> GetProductsForShoppingListId(Guid shoppingListId);

        void AddProductToShoppingList(Guid shoppingListId, AddProductToShoppingListRequest request);
        void RemoveProductFromShoppingList(Guid listItemId, Guid shoppingListId);

        void UpdateShoppingListCount(Guid shoppingListItemId, int delta);
        void UpdateChecked(Guid itemId, bool isChecked);
        void AddProductToShoppingList(Guid listId, ShoppingListItemModel item);
        void AddMeasurementsToItem(Guid shoppingListItemId, int measurementId, int count);
        List<MeasurementForShopItemModel> GetMeasurementsForCheckedItem(Guid id);

        void UpdaterecipeQuantity(Guid shoppingListId, Guid recipeId, int delta);
        void UpdateProductQuantity(Guid shoppingListId, int quantityId, int delta);
        void SaveItemToShoppingList(Guid shoppingListID, string text);
    }
}
