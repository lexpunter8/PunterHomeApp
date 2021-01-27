using DataModels;
using DataModels.Measurements;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Enums;

namespace PunterHomeDomain.Services
{
    public class StaticShoppingListItem
    {

    }

    public class DynamicShoppingListItem
    {

    }

    public class ShoppingListItemDetailsModel
    {
        public List<ShoppingListItemModel> StaticItems { get; set; } = new List<ShoppingListItemModel>();
        public List<ShoppingListItemModel> DynamicItems { get; set; } = new List<ShoppingListItemModel>();

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double DynamicAmountRequested { get; set; }
        public double DynamicAmountAvailable { get; set; }
        public double StaticAmount { get; set; }
        public EUnitMeasurementType MeasurementType { get; set; }
    }

    public class ShoppingListService : IShoppingListService
    {
        private readonly IShoppingListDataAdapter myShoppingListDataAdapter;
        private readonly IProductDataAdapter myProductDataAdapter;
        private readonly IRecipeDataAdapter recipeDataAdapter;

        public ShoppingListService(IShoppingListDataAdapter shoppingListDataAdapter, IProductDataAdapter productDataAdapter, IRecipeDataAdapter recipeDataAdapter)
        {
            myShoppingListDataAdapter = shoppingListDataAdapter;
            myProductDataAdapter = productDataAdapter;
            this.recipeDataAdapter = recipeDataAdapter;
        }

        public List<ShoppingListModel> GetShoppingLists()
        {
            return myShoppingListDataAdapter.GetShoppingLists();
        }

        public List<ShoppingListItemDetailsModel> GetItemsForShoppingList(Guid shoppingListId)
        {
             var allItems = myShoppingListDataAdapter.GetItemsForShoppingList(shoppingListId);
            Dictionary<Guid, List<ShoppingListItemModel>> productIdWithShoppingItem = new Dictionary<Guid, List<ShoppingListItemModel>>();

            foreach (var item in allItems)
            {
                if (productIdWithShoppingItem.ContainsKey(item.ProductId))
                {
                    productIdWithShoppingItem[item.ProductId].Add(item);
                    continue;
                }
                productIdWithShoppingItem.Add(item.ProductId, new List<ShoppingListItemModel> { item });
            }

            var detailItems = new List<ShoppingListItemDetailsModel>();
            foreach (var item in productIdWithShoppingItem)
            {
                ShoppingListItemDetailsModel details = new ShoppingListItemDetailsModel();

                var measurementType = item.Value.First().MeasurementType;
                MeasurementClassObject amountObject = new MeasurementClassObject();

                item.Value.Where(i => i.RecipeItem == null).ToList().ForEach(a =>
                {
                    details.StaticItems.Add(a);
                    amountObject.Values.Add(new MeasurementAmount
                    {
                        Amount = a.MeasurementAmount,
                        Type = a.MeasurementType
                    });
                });

                double totalStaticAmount = amountObject.GetTotalAmount(measurementType);


                MeasurementClassObject amountDynamicObject = new MeasurementClassObject();

                item.Value.Where(i => i.RecipeItem != null).ToList().ForEach(a => {
                    details.DynamicItems.Add(a);
                    
                    amountDynamicObject.Values.Add(                    
                    new MeasurementAmount
                    {
                        Amount = a.MeasurementAmount,
                        Type = a.MeasurementType
                    }); 
                });

                double totalDynamicAmount = amountDynamicObject.GetTotalAmount(measurementType);

                details.DynamicAmountRequested = totalDynamicAmount;
                details.StaticAmount = totalStaticAmount;
                details.MeasurementType = measurementType;
                details.ProductId = item.Key;
                details.ProductName = item.Value.First().ProductName;
                var prod = myProductDataAdapter.GetProductById(item.Key);

                if (prod.MeasurementAmounts != null)
                {
                    details.DynamicAmountAvailable = prod.MeasurementAmounts.GetTotalAmount(measurementType);
                }
                detailItems.Add(details);
            }
            return detailItems;
        }


        public void AddProductToShoppingList(Guid shoppingListId, AddProductToShoppingListRequest request)
        {
            myShoppingListDataAdapter.AddProductToShoppingList(shoppingListId, request);
        }

        public void UpdateShoppingListCount(Guid shoppingListItemId, int delta)
        {
            myShoppingListDataAdapter.UpdateShoppingListCount(shoppingListItemId, delta);
        }

        public void RemoveProductFromShoppingList(Guid itemId)
        {
            myShoppingListDataAdapter.RemoveProductFromShoppingList(itemId);
        }

        public void AddItemToShoppingList(Guid listId, ShoppingListItemModel item)
        {
            myShoppingListDataAdapter.AddProductToShoppingList(listId, item);
        }

        public void AddMinimumAmountToShoppingList(Guid shoppingListId, Guid productId, MeasurementAmount amount)
        {
            //var product = myProductDataAdapter.GetProductById(productId);

            //IEnumerable<BaseMeasurement> quan = product.ProductQuantities.Where(p => p.ConvertTo(amount.Type) >= amount.Amount);

            //if (quan.Any())
            //{
            //    double newItemVal = quan.Min(m => m.UnitQuantityTypeVolume);
            //    myShoppingListDataAdapter.AddProductToShoppingList(shoppingListId, quan.First(q => q.UnitQuantityTypeVolume == newItemVal).ProductQuantityId);
            //    return;
            //}

            /////

            //var allQuans  = product.ProductQuantities;
            //var item = allQuans.FirstOrDefault();
            //var i = item.ConvertTo(amount.Type);
            //var count = Math.Ceiling(amount.Amount / i);

            //myShoppingListDataAdapter.AddProductToShoppingList(shoppingListId, item.ProductQuantityId, (int)count);

        }

        public void UpdateChecked(Guid itemId, bool isChecked)
        {
            myShoppingListDataAdapter.UpdateChecked(itemId, isChecked);
        }

        public void AddQuantityToProductForCheckedItems(Guid shoppingListId)
        {
        //    var checkedItems = myShoppingListDataAdapter.GetItemsForShoppingList(shoppingListId).Where(i => i.IsChecked);

        //    foreach (var item in checkedItems)
        //    {
        //        myProductDataAdapter.IncreaseProductQuantity(item.ProductQuantityId, item.Count);
        //        myShoppingListDataAdapter.RemoveProductFromShoppingList(item.Id);
        //    }
        }
    }
}
