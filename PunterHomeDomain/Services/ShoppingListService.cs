﻿using DataModels;
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
    public class QuantityWithInfo
    {
        public QuantityWithInfo(BaseMeasurement measurement)
        {
            Measurement = measurement;
        }

        public BaseMeasurement Measurement { get; }
        public double MaxPossibleForRequestMin => RequestedAmount / NormalizedValue;
        public double DifferenceMin => RequestedAmount - (NormalizedValue * (MaxPossibleForRequestMin >= 1 ? Math.Floor(MaxPossibleForRequestMin) : 1));
        public double DifferencePercentageMin => MaxPossibleForRequestMin - Math.Floor(MaxPossibleForRequestMin);
        public double NormalizedValue { get; internal set; }
        public double RequestedAmount { get; internal set; }


        public double MaxPossibleForRequestMax => Math.Ceiling(RequestedAmount / NormalizedValue);
        public double DifferenceMax => Math.Abs(RequestedAmount - (NormalizedValue * MaxPossibleForRequestMax));
    }

    public class DynamicShoppingListItem
    {

    }

    public class ShoppingListShopItem
    {
        public Guid ProductId { get; set; }
        public BaseMeasurement Measurement { get; set; }
        public int Amount { get; set; }
        public bool IsChecked { get; set; }
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public List<MeasurementForShopItemModel> MeasurementsForChecked { get; set; } = new List<MeasurementForShopItemModel>();
    }

    public class ShoppingListItemDetailsModel
    {
        public List<ShoppingListItemInfoModel> StaticItems { get; set; } = new List<ShoppingListItemInfoModel>();
        public List<ShoppingListItemInfoModel> DynamicItems { get; set; } = new List<ShoppingListItemInfoModel>();

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double DynamicAmountRequested { get; set; }
        public double DynamicAmountAvailable { get; set; }
        public double StaticAmount { get; set; }
        public EUnitMeasurementType MeasurementType { get; set; }
        public bool IsChecked { get; set; }
        public double TotalAmount => StaticAmount + Math.Max(0, (DynamicAmountRequested - DynamicAmountAvailable));
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
        public List<ShoppingListShopItem> GetShoppingListShopItems(Guid shoppingListId)
        {
            List<ShoppingListItemDetailsModel> items = GetItemsForShoppingList(shoppingListId);
            List<ShoppingListShopItem> shopItems = new List<ShoppingListShopItem>();
            var shoppingListITems = myShoppingListDataAdapter.GetItemsForShoppingList(shoppingListId);

            foreach (var item in items)
            {
                var totRequestedAmount = item.StaticAmount + (item.DynamicAmountRequested < item.DynamicAmountAvailable ? 0 : item.DynamicAmountRequested - item.DynamicAmountAvailable);
                if (totRequestedAmount == 0)
                {
                    continue;
                }
                var product = myProductDataAdapter.GetProductById(item.ProductId);

                var list = product.ProductQuantities.ToList();
                list.Sort(new ProductQuantitySorter());

                var quantitiesWithHigherAmount = list.Where(i => i.ConvertTo(item.MeasurementType) > totRequestedAmount);

                // if any with higher amount then required, we can add the lowest , which is the first
                if (quantitiesWithHigherAmount.Any() && false)
                {
                    var newShopItem = new ShoppingListShopItem
                    {
                        Id = item.Id,
                        ProductName = item.ProductName,
                        Amount = 1,
                        Measurement = quantitiesWithHigherAmount.First(),
                        IsChecked = item.IsChecked, /// TODO,
                        ProductId = item.ProductId,
                        MeasurementsForChecked = myShoppingListDataAdapter.GetMeasurementsForCheckedItem(item.Id)
                    };
                    shopItems.Add(newShopItem);
                    continue;
                }

                // TODO if not lowest
                var result = GetQuantityWithInfos(list, item.MeasurementType, totRequestedAmount);

                var resultZeroDiff = result.Where(r => r.DifferenceMin == 0);

                if (resultZeroDiff.Any())
                {
                    var minAmount = resultZeroDiff.Min(r => r.MaxPossibleForRequestMin);
                    var itemToUse = resultZeroDiff.First(r => r.MaxPossibleForRequestMin == minAmount);
                    shopItems.Add(new ShoppingListShopItem
                    {
                        Amount = Convert.ToInt32(itemToUse.MaxPossibleForRequestMin),
                        Measurement = itemToUse.Measurement,
                        IsChecked = item.IsChecked, /// TODO,
                        ProductId = item.ProductId,
                        Id = item.Id,
                        ProductName = item.ProductName,
                        MeasurementsForChecked = myShoppingListDataAdapter.GetMeasurementsForCheckedItem(item.Id)
                    });
                    continue;
                }

                result.OrderBy(r => r.DifferenceMax).ThenBy(s => s.MaxPossibleForRequestMax);

                shopItems.Add(new ShoppingListShopItem
                {
                    Amount = Convert.ToInt32(result.First().MaxPossibleForRequestMax),
                    Measurement = result.First().Measurement,
                    IsChecked = item.IsChecked, /// TODO,
                    ProductId = item.ProductId,
                        Id = item.Id,
                    ProductName = item.ProductName,
                    MeasurementsForChecked = myShoppingListDataAdapter.GetMeasurementsForCheckedItem(item.Id)
                });
            }


            return shopItems;
        }

        public List<QuantityWithInfo> GetQuantityWithInfos(List<BaseMeasurement> measurements, EUnitMeasurementType measurementType, double totRequestedAmount)
        {
            var infos = new List<QuantityWithInfo>();

            foreach (var item in measurements)
            {
                var newInfo = new QuantityWithInfo(item);
                newInfo.NormalizedValue = item.ConvertTo(measurementType);
                newInfo.RequestedAmount = totRequestedAmount;

                infos.Add(newInfo);
            }

            infos.OrderBy(i => i.MaxPossibleForRequestMin);
            return infos;
        }

        public List<ShoppingListItemDetailsModel> GetItemsForShoppingList(Guid shoppingListId)
        {
            List<ShoppingListItemModel> allProducts = myShoppingListDataAdapter.GetItemsForShoppingList(shoppingListId);
            
            var detailItems = new List<ShoppingListItemDetailsModel>();
            foreach (ShoppingListItemModel sItem in allProducts)
            {
                ProductDetails product = myProductDataAdapter.GetProductById(sItem.ProductId);
                //List<ShoppingListItemInfoModel> infoItems = myShoppingListDataAdapter.GetInfoItemsForShoppingListItem(sItem.Id);
                ShoppingListItemDetailsModel details = new ShoppingListItemDetailsModel();

                var measurementType = sItem.InfoItems.First().MeasurementType;
                MeasurementClassObject amountObject = new MeasurementClassObject();

                sItem.InfoItems.Where(i => i.RecipeItem == null).ToList().ForEach(a =>
                {
                    details.StaticItems.Add(a);
                    amountObject.Values.Add(new MeasurementAmount
                    {
                        Amount = a.MeasurementAmount,
                        Type = a.MeasurementType
                    });
                });

                MeasurementClassObject amountDynamicObject = new MeasurementClassObject();

                sItem.InfoItems.Where(i => i.RecipeItem != null).ToList().ForEach(a => {
                    if (a.RecipeItem.IsOnlyUnavailable)
                    {
                        details.StaticItems.Add(a);

                        amountObject.Values.Add(
                        new MeasurementAmount
                        {
                            Amount = a.MeasurementAmount,
                            Type = a.MeasurementType
                        });
                        return;
                    }
                    details.DynamicItems.Add(a);
                    
                    amountDynamicObject.Values.Add(                    
                    new MeasurementAmount
                    {
                        Amount = a.MeasurementAmount,
                        Type = a.MeasurementType
                    }); 
                });

                double totalDynamicAmount = amountDynamicObject.GetTotalAmount(measurementType);
                double totalStaticAmount = amountObject.GetTotalAmount(measurementType);

                details.DynamicAmountRequested = totalDynamicAmount;
                details.StaticAmount = totalStaticAmount;
                details.MeasurementType = measurementType;
                details.ProductId = sItem.ProductId;
                details.ProductName = sItem.ProductName;
                details.Id = sItem.Id;
                details.IsChecked = sItem.IsChecked;

                if (product.MeasurementAmounts != null)
                {
                    details.DynamicAmountAvailable = product.MeasurementAmounts.GetTotalAmount(measurementType);
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
            var checkedItems = myShoppingListDataAdapter.GetItemsForShoppingList(shoppingListId).Where(i => i.IsChecked);


            foreach (var item in checkedItems)
            {
                List<MeasurementForShopItemModel> checkedMeasurements = myShoppingListDataAdapter.GetMeasurementsForCheckedItem(item.Id);
                checkedMeasurements.ForEach(m => myProductDataAdapter.IncreaseProductQuantity(m.Measurement.ProductQuantityId, m.Count));
                
                myShoppingListDataAdapter.RemoveProductFromShoppingList(item.Id);
            }
        }

        public void AddMeasurementsToShopItems(List<AddMeasurementsToShoppingListItem> request)
        {
            foreach (var item in request)
            {
                myShoppingListDataAdapter.AddMeasurementsToItem(item.ShoppingListItemId, item.MeasurementId, item.Count);

            }
        }
    }
}
