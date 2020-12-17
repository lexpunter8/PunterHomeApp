using DataModels;
using DataModels.Measurements;
using PunterHomeDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PunterHomeDomain.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IShoppingListDataAdapter myShoppingListDataAdapter;
        private readonly IProductDataAdapter myProductDataAdapter;

        public ShoppingListService(IShoppingListDataAdapter shoppingListDataAdapter, IProductDataAdapter productDataAdapter)
        {
            myShoppingListDataAdapter = shoppingListDataAdapter;
            myProductDataAdapter = productDataAdapter;
        }

        public List<ShoppingListModel> GetShoppingLists()
        {
            return myShoppingListDataAdapter.GetShoppingLists();
        }

        public List<ShoppingListItemModel> GetItemsForShoppingList(Guid shoppingListId)
        {
            return myShoppingListDataAdapter.GetItemsForShoppingList(shoppingListId);
        }


        public void AddProductToShoppingList(Guid shoppingListId, int productQuantyId)
        {
            myShoppingListDataAdapter.AddProductToShoppingList(shoppingListId, productQuantyId);
        }

        public void UpdateShoppingListCount(Guid shoppingListItemId, int delta)
        {
            myShoppingListDataAdapter.UpdateShoppingListCount(shoppingListItemId, delta);
        }

        public void RemoveProductFromShoppingList(Guid itemId)
        {
            myShoppingListDataAdapter.RemoveProductFromShoppingList(itemId);
        }

        public void AddMinimumAmountToShoppingList(Guid shoppingListId, Guid productId, MeasurementAmount amount)
        {
            var product = myProductDataAdapter.GetProductById(productId);

            IEnumerable<BaseMeasurement> quan = product.ProductQuantities.Where(p => p.ConvertTo(amount.Type) >= amount.Amount);

            if (quan.Any())
            {
                double newItemVal = quan.Min(m => m.UnitQuantityTypeVolume);
                myShoppingListDataAdapter.AddProductToShoppingList(shoppingListId, quan.First(q => q.UnitQuantityTypeVolume == newItemVal).ProductQuantityId);
                return;
            }

            ///

            var allQuans  = product.ProductQuantities;
            var item = allQuans.FirstOrDefault();
            var i = item.ConvertTo(amount.Type);
            var count = Math.Ceiling(amount.Amount / i);

            myShoppingListDataAdapter.AddProductToShoppingList(shoppingListId, item.ProductQuantityId, (int)count);

        }

        public void UpdateChecked(Guid itemId, bool isChecked)
        {
            myShoppingListDataAdapter.UpdateChecked(itemId, isChecked);
        }

        public void AddQuantityToProductForCheckedItems(Guid shoppingListId)
        {
            var checkedItems = myShoppingListDataAdapter.GetItemsForShoppingList(shoppingListId).Where(i => i.IsChecked);

            foreach (var item in checkedItems)
            {
                myProductDataAdapter.IncreaseProductQuantity(item.ProductQuantityId, item.Count);
                myShoppingListDataAdapter.RemoveProductFromShoppingList(item.Id);
            }
        }
    }
}
