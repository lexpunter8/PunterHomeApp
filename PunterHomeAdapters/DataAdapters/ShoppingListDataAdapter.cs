using DataModels;
using DataModels.Measurements;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters.Models;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using PunterHomeDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PunterHomeAdapters.DataAdapters
{

    public class ShoppingListDataAdapter : IShoppingListDataAdapter
    {
        private DbContextOptions<HomeAppDbContext> myDbOptions;

        public ShoppingListDataAdapter(DbContextOptions<HomeAppDbContext> options)
        {
            myDbOptions = options;
        }

        public List<ShoppingListModel> GetShoppingLists()
        {
            using var context = new HomeAppDbContext(myDbOptions);

            return context.ShoppingLists.Select(s => s.Convert()).ToList();
        }

        public List<ShoppingListItemModel> GetItemsForShoppingList(Guid shoppingListId)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var items = new List<ShoppingListItemModel>();
            var listItems = context.ShoppingListItems.Include(i => i.Product).Include(i => i.ItemInfos).ThenInclude(i => i.RecipeItem).ThenInclude(r => r.Recipe).Where(i => i.ShoppingList.Id == shoppingListId).ToList();

            foreach (var i in listItems)
            {
                var newItem = new ShoppingListItemModel
                {
                    Id = i.Id,
                    ShoppingListId = shoppingListId,
                    IsChecked = i.IsChecked,
                    ProductId = i.Product.Id,
                    ProductName = i.Product.Name,
                    InfoItems = ConvertInfoItems(i.ItemInfos)
                };

                items.Add(newItem);

            }
            return items;
        }

        private List<ShoppingListItemInfoModel> ConvertInfoItems(List<DbShoppingListItemInfo> itemInfos)
        {
            var newList = new List<ShoppingListItemInfoModel>();
            itemInfos.ForEach(i => newList.Add(new ShoppingListItemInfoModel
            {
                Id = i.Id,
                MeasurementAmount = i.MeasurementAmount,
                MeasurementType = i.MeasurementType,
                RecipeItem = ConvertRecipeItem(i.RecipeItem),
                Reason = i.Reason
            }));

            return newList;
        }

        private RecipeShoppingListItemModel ConvertRecipeItem(DbRecipeShoppingListItem recipeItem)
        {
            if (recipeItem == null)
            {
                return null;
            }
            return new RecipeShoppingListItemModel
            {
                NrOfPersons = recipeItem.NrOfPersons,
                RecipeId = recipeItem.RecipeId,
                ShoppingListItemId = recipeItem.ShoppingListItemId,
                RecipeName = recipeItem.Recipe.Name
            };
        }

        public void AddProductToShoppingList(Guid shoppingListId, AddProductToShoppingListRequest request)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var existingItem = context.ShoppingListItems.FirstOrDefault(o => o.ProductId == request.ProductId);
            bool addNew = false;
            if (existingItem == null)
            {
                addNew = true;
                existingItem = new DbShoppingListItem
                {
                    IsChecked = false,
                    Product = context.Products.First(p => p.Id == request.ProductId),

                    ShoppingList = context.ShoppingLists.First()
                };

            }
            

            var newItemInfo = new DbShoppingListItemInfo
            {
                MeasurementType = request.MeasurementType,
                MeasurementAmount = request.MeasurementAmount,
                Reason = request.Reason,
                ShoppingListItem = existingItem
            };

            if (request.RecipeId != null && request.RecipeId != Guid.Empty)
            {
                newItemInfo.RecipeItem = new DbRecipeShoppingListItem
                {
                    RecipeId = request.RecipeId,
                    NrOfPersons = request.NrOfPersons
                };
            }

            if (addNew)
            {

                context.ShoppingListItems.Add(existingItem);
            }
            context.ShoppingListItemInfos.Add(newItemInfo);
            context.SaveChanges();

            //    var list = context.ShoppingLists.FirstOrDefault(s => s.Id == shoppingListId);
            //    var productQuan = context.ProductQuantities.FirstOrDefault(s => s.Id == productQuantyId);
            //    if (list == null)
            //    {
            //        throw new KeyNotFoundException($"{nameof(DbShoppingList)} with id {shoppingListId} not found");
            //    }

            //    if (productQuan == null)
            //    {
            //        throw new KeyNotFoundException($"{nameof(DbProductQuantity)} with id {productQuantyId} not found");
            //    }

            //    var item = context.ShoppingListItems.Include(i => i.ShoppingList).Include(i => i.ProductQuantities)
            //        .FirstOrDefault(s => s.ShoppingList.Id == shoppingListId && s.ProductQuantities.Id == productQuantyId);

            //    if (item != null)
            //    {
            //        item.Count += count;
            //        context.SaveChanges();
            //        return;
            //    }

            //    var newItem = new DbShoppingListItem
            //    {
            //        ShoppingList = list,
            //        Count = count,
            //        ProductQuantities = productQuan
            //    };

            //    context.ShoppingListItems.Add(newItem);
            //    context.SaveChanges();
        }

        public void UpdateShoppingListCount(Guid shoppingListItemId, int delta)
        {
            //using var context = new HomeAppDbContext(myDbOptions);

            //var item = context.ShoppingListItems.FirstOrDefault(s => s.Id == shoppingListItemId);

            //if (item == null)
            //{
            //    throw new KeyNotFoundException($"{nameof(DbShoppingListItem)} with id {shoppingListItemId} not found");
            //}

            //item.Count += delta;

            //context.SaveChanges();
        }

        public void RemoveProductFromShoppingList(Guid itemId)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            DbShoppingListItem item = context.ShoppingListItems.Include(i => i.ShoppingListItemMeasurements).Include(s => s.ItemInfos).ThenInclude(i => i.RecipeItem).FirstOrDefault(s => s.Id == itemId);
            
            if (item == null)
            {
                throw new KeyNotFoundException($"{nameof(DbShoppingListItem)} with id {itemId} not found");
            }

            context.ShoppingListItems.Remove(item);
            context.SaveChanges();
        }

        public void UpdateChecked(Guid itemId, bool isChecked)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            context.ShoppingListItems.FirstOrDefault(sl => sl.Id == itemId).IsChecked = isChecked;

            if (!isChecked)
            {
                var toRemove = context.MeasurementsForShoppingListItem.Where(i => i.ShoppingListItemId == itemId);
                context.MeasurementsForShoppingListItem.RemoveRange(toRemove);
            }

            context.SaveChanges();
        }

        public List<ShoppingListItemInfoModel> GetInfoItemsForShoppingListItem(Guid shoppingListItemId)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            IQueryable<DbShoppingListItemInfo> items = context.ShoppingListItemInfos.Include(i => i.RecipeItem).Where(ii => ii.ShoppingListItemId == shoppingListItemId);

            var result = items.Select(i => new ShoppingListItemInfoModel
            {
                Id = i.Id,
                MeasurementAmount = i.MeasurementAmount,
                MeasurementType = i.MeasurementType,
                Reason = i.Reason,
                RecipeItem = GetRecipeShoppingListItem(i.RecipeItem)
            });

            return result.ToList();
        }

        private RecipeShoppingListItemModel GetRecipeShoppingListItem(DbRecipeShoppingListItem item)
        {
            if (item == null)
            {
                return null;
            }

            return new RecipeShoppingListItemModel
            {
                NrOfPersons = item.NrOfPersons,
                RecipeId = item.RecipeId,
                RecipeName = item.Recipe.Name
            };
        }

        public void AddProductToShoppingList(Guid listId, ShoppingListItemModel item)
        {
            throw new NotImplementedException();
        }

        public void AddMeasurementsToItem(Guid shoppingListItemId, int measurementId, int count)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            context.MeasurementsForShoppingListItem.Add(new DbShoppingListItemMeasurement
            {
                Count = count,
                ShoppingListItemId = shoppingListItemId,
                ProductQuantityId = measurementId
            });

            context.SaveChanges();
        }

        public List<MeasurementForShopItemModel> GetMeasurementsForCheckedItem(Guid id)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var result =  context.MeasurementsForShoppingListItem.Include(m => m.ProductQuantity)
                .Where(m => m.ShoppingListItemId == id).ToList();
            return result.Select(m => GetBaseMeasurement(m))
                .ToList();
        }

        private MeasurementForShopItemModel GetBaseMeasurement(DbShoppingListItemMeasurement m)
        {
            var n = BaseMeasurement.GetMeasurement(m.ProductQuantity.UnitQuantityType);
            n.Barcode = m.ProductQuantity.Barcode;
            n.ProductQuantityId = m.ProductQuantity.Id;
            n.UnitQuantityTypeVolume = m.ProductQuantity.QuantityTypeVolume;
            return new MeasurementForShopItemModel(n)
            {
                Count = m.Count
            };
        }
    }
}
